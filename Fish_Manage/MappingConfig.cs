using AutoMapper;
using Fish_Manage.Models;
using Fish_Manage.Models.DTO.Order;
using Fish_Manage.Models.DTO.OrderDetail;
using Fish_Manage.Models.DTO.Product;
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

            CreateMap<Order, OrderDTO>();
            CreateMap<OrderDTO, Order>();

            CreateMap<Order, OrderCreateDTO>().ReverseMap();
            CreateMap<Order, OrderUpdateDTO>().ReverseMap();

            CreateMap<OrderCreateDTO, Order>();
            CreateMap<OrderDetailCreateDTO, OrderDetail>();

            CreateMap<ApplicationUser, UserDTO>();




        }
    }
}
