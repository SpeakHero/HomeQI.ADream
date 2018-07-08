using System.Threading.Tasks;
using HomeQI.ADream.Identity.Core;
using HomeQI.ADream.Identity.Entites;
using HomeQI.ADream.Identity.Manager;

namespace HomeQI.ADream.Identity.Validators
{
    public interface IRoleValidator
    {
        Task<IdentityResult> ValidateAsync(IRoleManager manager, Role role);
    }
}