using AutoMapper;
using RecipeBook_API.Contracts.Recipes;
using RecipeBook_API.Domain.Entities;
using RecipeBook_API.Domain.ValueObjects;

namespace RecipeBook_API.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Ingredient, IngredientDto>().ReverseMap();
            CreateMap<InstructionStep, InstructionStepDto>().ReverseMap();
            CreateMap<Nutrition, NutritionDto>().ReverseMap();
            CreateMap<Recipe, RecipeReadDto>()
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags.Select(t => t.Name)));
            CreateMap<RecipeCreateDto, Recipe>().ForMember(dest => dest.Tags, opt => opt.Ignore());
            CreateMap<RecipeUpdateDto, Recipe>().ForMember(dest => dest.Tags, opt => opt.Ignore());
        }
    }
}