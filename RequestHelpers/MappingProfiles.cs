using AutoMapper;
using uga_mpl_server.DTO.Product;
using uga_mpl_server.DTO.User;
using uga_mpl_server.Entities;
using uga_mpl_server.Enums;

namespace uga_mpl_server.RequestHelpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        // User mappings
        CreateMap<User, UserSummaryDTO>();

        CreateMap<User, UserDTO>()
            .ForMember(dest => dest.Wishlist, opt => opt.Ignore())
            .ForMember(dest => dest.Subscriptions, opt => opt.Ignore());

        CreateMap<CreateUserDTO, User>();

        CreateMap<UpdateUserDTO, User>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        // Product mappings
        CreateMap<Product, ProductSummaryDTO>()
            .ForMember(dest => dest.SellerName,
                opt => opt.MapFrom(src => src.Seller.FirstName + " " + src.Seller.LastName))
            .ForMember(dest => dest.Category,
                opt => opt.MapFrom(src => src.Category.ToString()));

        CreateMap<Product, ProductDTO>()
            .ForMember(dest => dest.SellerName,
                opt => opt.MapFrom(src => src.Seller.FirstName + " " + src.Seller.LastName))
            .ForMember(dest => dest.Category,
                opt => opt.MapFrom(src => src.Category.ToString()))
            .ForMember(dest => dest.Subscribers, opt => opt.Ignore())
            .ForMember(dest => dest.WishlistedBy, opt => opt.Ignore());

        CreateMap<CreateProductDTO, Product>()
            .ForMember(dest => dest.Category,
                opt => opt.MapFrom(src => Enum.Parse<Category>(src.Category, true)));

        CreateMap<UpdateProductDTO, Product>()
            .ForMember(dest => dest.Category,
                opt => opt.MapFrom(src => Enum.Parse<Category>(src.Category!, true)))
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
    }
}
