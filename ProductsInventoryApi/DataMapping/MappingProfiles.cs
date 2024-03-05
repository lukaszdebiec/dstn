using AutoMapper;
using ProductsInventoryApi.DTOs;
using ProductsInventoryApi.Models;

namespace ProductsInventoryApi.DataMapping
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductDto>();
            CreateMap<CreateProductDto, Product>();
        }
    }
}