using AutoMapper;
using HomeQI.Adream.Identity;
using HomeQI.ADream.Identity.ViewModels.ManageViewModels;
using HomeQI.ADream.Identity.ViewModels.PermissionViewModel;
using HomeQI.ADream.Identity.ViewModels.RoleManagerViewModels;
using System;

namespace HomeQI.ADream.Identity.ViewModels
{
    public class IdentityMappingProfile : Profile
    {
        public IdentityMappingProfile()
        {
            Configure();
        }

        protected IdentityMappingProfile(string profileName) : base(profileName)
        {
        }

        protected IdentityMappingProfile(string profileName, Action<IProfileExpression> configurationAction) : base(profileName, configurationAction)
        {
        }

        protected virtual void Configure()
        {
            CreateMap<IdentityPermission, CreatePermissionDto>();
            CreateMap<IdentityPermission, EditPermissionDto>();
            CreateMap<CreatePermissionDto, IdentityPermission>();
            CreateMap<EditPermissionDto, IdentityPermission>();
            /////////////////////
            CreateMap<IdentityRole, CreateRoleDto>();
            CreateMap<IdentityRole, EditRoleDto>();
            CreateMap<EditRoleDto, IdentityRole>();
            CreateMap<CreateRoleDto, IdentityRole>();
            CreateMap<IdentityRole, DeleteRoleViewModel>();
            CreateMap<DeleteRoleViewModel, IdentityRole>();
            /////////////////////////////////
            CreateMap<UserDto, IdentityUser>();
            CreateMap<IdentityUser, UserDto>();
            CreateMap<CreateUserDto, IdentityUser>();
            CreateMap<IdentityUser, CreateUserDto>();
            CreateMap<EditeUserDto, IdentityUser>();
            CreateMap<IdentityUser, EditeUserDto>();
            //CreateMap<IdentityRole, GoodsDto>()
            //    //映射发生之前
            //    .BeforeMap((source, dto) =>
            //    {
            //        //可以较为精确的控制输出数据格式
            //        dto.CreateTime = Convert.ToDateTime(source.CreateTime).ToString("yyyy-MM-dd");
            //    })
            //    //映射发生之后
            //    .AfterMap((source, dto) =>
            //    {
            //        //code ...
            //    });

            //CreateMap<GoodsDto, GoodsEntity>();
        }
    }
}
