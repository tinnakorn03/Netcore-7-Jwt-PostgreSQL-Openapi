namespace NetcoreJwtJsonbOpenapi.Helpers;

using AutoMapper;
using NetcoreJwtJsonbOpenapi.Entities;
using NetcoreJwtJsonbOpenapi.Models.Users;
using NetcoreJwtJsonbOpenapi.Models.Contact;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
       
        CreateMap<CreateRequest, User>();                       // CreateRequest -> User
        CreateMap<ContactFormRequest, Contact>();               // ContactFormRequest -> Contact


        // UpdateRequest -> User
        CreateMap<UpdateRequest, User>()
            .ForAllMembers(x => x.Condition(
                (src, dest, prop) =>
                {
                    // ignore both null & empty string properties
                    if (prop == null) return false;
                    if (prop.GetType() == typeof(string) && string.IsNullOrEmpty((string)prop)) return false;

                    // ignore null role
                    if (x.DestinationMember.Name == "Role" && src.Role == null) return false;

                    return true;
                }
            ));
    }
}