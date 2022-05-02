using AutoMapper;
using PasswordManager.Contracts;
using PasswordManager.Database.Models.Entities;

namespace PasswordManager.Web
{
    public class ControllersMappingProfile : Profile
    {
        public ControllersMappingProfile()
        {
            CreateMap<PasswordDto, Password>(MemberList.Source)
                .ForMember(x=> x.CryptPasswordValue,
                    opt => opt.MapFrom(
                        src => src.Value));
            CreateMap<Password, PasswordDto>(MemberList.Destination)
                .ForMember(x=> x.Value,
                    opt => opt.MapFrom(
                        src => src.CryptPasswordValue));

            CreateMap<DataType, DataTypeDto>(MemberList.Destination);
            CreateMap<TypeField, TypeFieldDto>(MemberList.Destination);
        }
    }
}