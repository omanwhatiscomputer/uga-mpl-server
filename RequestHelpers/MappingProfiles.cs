using AutoMapper;
using uga_mpl_server.DTO.Product;
using uga_mpl_server.DTO.User;
using uga_mpl_server.Entities;

namespace uga_mpl_server.RequestHelpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        // User mappings
        CreateMap<User, UserDTO>();
        CreateMap<CreateUserDTO, User>();
        CreateMap<UpdateUserDTO, User>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        // Product mappings
        CreateMap<Product, ProductDTO>()
            .ForMember(dest => dest.SellerName,
                opt => opt.MapFrom(src => src.Seller.FirstName + " " + src.Seller.LastName));
        CreateMap<CreateProductDTO, Product>();
        CreateMap<UpdateProductDTO, Product>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
    }
}
