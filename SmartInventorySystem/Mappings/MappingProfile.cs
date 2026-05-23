namespace SmartInventorySystem.Mappings;
using AutoMapper;
using SmartInventorySystem.DTOs;
using SmartInventorySystem.Models;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Product, ProductDto>().ReverseMap();
        CreateMap<Product, LowStockProductDto>();
        CreateMap<CreateProductDto, Product>();
        CreateMap<UpdateProductDto, Product>();
    }
}