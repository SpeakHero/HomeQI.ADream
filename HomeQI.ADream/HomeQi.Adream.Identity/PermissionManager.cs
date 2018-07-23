using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace HomeQI.Adream.Identity
{
    public class PermissionManager : PermissionManager<IdentityPermission>
    {
        public PermissionManager(IPermissionStore<IdentityPermission> store) : base(store)
        {

        }
        public async Task<IdentityResult> EditAllowAnonymousAsync(IdentityPermission
                identityPermission, bool enable = true)
        {
            identityPermission.IsAllowAnonymous = enable;
            return await Store.UpdateAsync(identityPermission,
                CancellationToken, nameof(IdentityPermission.IsAllowAnonymous));
        }
        public async Task<IdentityResult> EditEnable(IdentityPermission
            identityPermission, bool enable = true)
        {
            identityPermission.IsEnable = enable;
            return await Store.UpdateAsync(identityPermission,
                CancellationToken, nameof(IdentityPermission.IsEnable));
        }
        public static IList<IdentityPermission> GetAllActionByAssembly()
        {

            var result = new List<IdentityPermission>();

            var types = Assembly.Load(new AssemblyName("HomeQI.ADream.Identity.Web")).GetTypes();
            foreach (var type in types)
            {
                if (string.IsNullOrEmpty(type.Namespace))
                {
                    continue;
                }
                if (type.Namespace.Contains("HomeQI.ADream.Identity.Web.Controllers"))//如果是Controller
                {
                    var members = type.GetMethods();
                    IEnumerable<AreaAttribute> areaName = type.GetTypeInfo().
                        BaseType?.GetTypeInfo().
                        GetCustomAttributes(typeof(AreaAttribute), true)
                        as AreaAttribute[];
                    foreach (var member in members)
                    {
                        var name = member.ReturnType.Name;
                        if ((member.ToString().Contains("IActionResult")
                            || name == "IActionResult") && member.IsPublic)//如果是Action
                        {

                            if (!(member.Name == "Json" ||
                                type.Name.Contains("Base")))
                            {
                                var pars = member.GetParameters().Select(o =>
                                new { o.ParameterType.FullName, o.Name });
                                IEnumerable<DescriptionAttribute> description =
                                    member.GetCustomAttributes(typeof(DescriptionAttribute), true) as DescriptionAttribute[];
                                var allowAnonymous = member.GetCustomAttributes(typeof(AllowAnonymousAttribute), true);
                                var ap = new IdentityPermission
                                {
                                    Action = member.Name
                                };
                                if (pars != null)
                                {
                                    if (pars.Count() > 0) ap.Params =
                                            Serializer.ToJson(pars);
                                }
                                ap.IsAllowAnonymous = allowAnonymous.Count() > 0;
                                ap.Controller = type.Name.Replace("Controller", ""); // 去掉“Controller”后缀
                                if (description?.Count() > 0)
                                {
                                    ap.Description = description?.FirstOrDefault().Description;
                                }

                                if (areaName?.Count() > 0)
                                {
                                    ap.AreaName = areaName?.FirstOrDefault().RouteValue;
                                }


                                if (ap.Action.Contains("OnAction"))
                                {
                                    continue;
                                }
                                result.Add(ap);
                            }

                        }
                    }
                }
            }
            return result;
        }

    }
}
