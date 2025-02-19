using AutoMapper;
using Fish_Manage.Models;
using Fish_Manage.Repository.DTO;

namespace Fish_Manage
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Product, ProductDTO>();
            CreateMap<ProductDTO, Product>();

            CreateMap<Product, ProductCreateDTO>().ReverseMap();
            CreateMap<Product, ProductUpdateDTO>().ReverseMap();

            CreateMap<ApplicationUser, UserDTO>();


        }
    }
}
