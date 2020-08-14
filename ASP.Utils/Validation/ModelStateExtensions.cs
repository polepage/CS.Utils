using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;

namespace ASP.Utils.Validation
{
    public static class ModelStateExtensions
    {
        public static bool IsValidPartial(this ModelStateDictionary modelState, params string[] keys)
        {
            return keys.All(k => modelState.GetValidationState(k) == ModelValidationState.Valid);
        }
    }
}
