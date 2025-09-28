using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using RecipeBook_API.Application.Abstraction;
using RecipeBook_API.Application.Models;
using RecipeBook_API.Contracts;
using RecipeBook_API.Contracts.Recipes;
using RecipeBook_API.Domain.Entities;
using RecipeBook_API.Domain.Infrastructure;
using RecipeBook_API.Validation;
using System.ComponentModel.DataAnnotations;

namespace RecipeBook_API.Application.Services
{
    public class RecipeService(AppDbContext db, IMapper mapper, RecipeValidator validator) : IRecipeService
    {
        public async Task<RecipeReadDto?> GetAsync(Guid id, Guid requesterId)
        {
            var e = await db.Recipes.Include(r => r.Tags).Include(r => r.Ingredients).Include(r => r.Steps)
                .SingleOrDefaultAsync(r => r.Id == id && r.OwnerId == requesterId);

            return e is null ? null : mapper.Map<RecipeReadDto>(e);
        }

        public async Task<PagedResult<RecipeReadDto>> SearchAsync(RecipeQuery q, Guid requesterId)
        {
            var qry = db.Recipes.AsNoTracking().Include(r => r.Tags).Include(r => r.Ingredients).Include(r => r.Steps)
                .Where(r => r.OwnerId == requesterId);

            if (!string.IsNullOrWhiteSpace(q.Q))
            {
                var x = q.Q.ToLower();
                qry = qry.Where(r => r.Title.ToLower().Contains(x) || (r.Description != null && r.Description.ToLower().Contains(x)));
            }

            if (q.Tags is { Length: > 0 })
            {
                qry = qry.Where(r => r.Tags.Any(t => q.Tags.Contains(t.Name)));
            }

            if (q.IncludeIngredients is { Length: > 0 })
            {
                qry = qry.Where(r => r.Ingredients.Any(i => q.IncludeIngredients.Contains(i.Name)));
            }

            if (q.ExcludeIngredients is { Length: > 0 })
            {
                qry = qry.Where(r => r.Ingredients.All(i => !q.ExcludeIngredients.Contains(i.Name)));
            }

            if (q.MaxKcal is not null)
            {
                qry = qry.Where(r => r.Nutrition != null && r.Nutrition.Kcal <= q.MaxKcal);
            }

            qry = q.Sort?.ToLower() switch
            {
                "kcal" => qry.OrderBy(r => r.Nutrition!.Kcal),
                "time" => qry.OrderBy(r => r.TotalMinutes),
                _ => qry.OrderByDescending(r => r.CreatedAt)
            };

            var total = await qry.CountAsync();
            var items = await qry.Skip((q.Page - 1) * q.PageSize).Take(q.PageSize)
                .ProjectTo<RecipeReadDto>(mapper.ConfigurationProvider).ToListAsync();

            return new PagedResult<RecipeReadDto>(items, total, q.Page, q.PageSize);
        }

        public async Task<RecipeReadDto> CreateAsync(RecipeCreateDto dto, Guid ownerId)
        {
            var entity = mapper.Map<Recipe>(dto);
            entity.Id = Guid.NewGuid();
            entity.OwnerId = ownerId;
            entity.CreatedAt = DateTimeOffset.UtcNow;
            entity.UpdatedAt = entity.CreatedAt;
            entity.Tags = await UpsertTagsAsync(dto.Tags);

            await ValidateAsync(entity);
            db.Recipes.Add(entity);
            await db.SaveChangesAsync();

            return mapper.Map<RecipeReadDto>(entity);
        }

        public async Task<bool> UpdateAsync(Guid id, RecipeUpdateDto dto, Guid requesterId)
        {
            var entity = await db.Recipes.Include(r => r.Tags).Include(r => r.Ingredients).Include(r => r.Steps)
                .SingleOrDefaultAsync(r => r.Id == id && r.OwnerId == requesterId);

            if (entity == null)
            {
                return false;
            }

            mapper.Map(dto, entity);
            entity.Tags = await UpsertTagsAsync(dto.Tags);
            entity.UpdatedAt = DateTimeOffset.UtcNow;

            await ValidateAsync(entity);
            await db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(Guid id, Guid requesterId)
        {
            var entity = await db.Recipes.SingleOrDefaultAsync(r => r.Id == id && r.OwnerId == requesterId);

            if (entity == null)
            {
                return false;
            }

            db.Recipes.Remove(entity);
            await db.SaveChangesAsync();

            return true;
        }

        private async Task<List<Tag>> UpsertTagsAsync(IEnumerable<string> tags)
        {
            var names = tags.Where(t => !string.IsNullOrWhiteSpace(t)).Select(t => t.Trim()).Distinct().ToArray();
            var existing = await db.Tags.Where(t => names.Contains(t.Name)).ToListAsync();
            var toAdd = names.Except(existing.Select(e => e.Name)).Select(n => new Tag { Id = Guid.NewGuid(), Name = n });

            existing.AddRange(toAdd);

            return existing;
        }

        private async Task ValidateAsync(Recipe entity)
        {
            var res = await validator.ValidateAsync(entity);
            if (!res.IsValid)
            {
                throw new FluentValidation.ValidationException(res.Errors);
            }
        }
    }
}