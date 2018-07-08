//using HomeQI.Adream.Identity;
//using System.Collections.Generic;
//using System.Linq;

//namespace HomeQI.ADream.Infrastructure.Identity
//{
//    public class ADreamResult : IdentityResult
//    {
//        public object Obj { get; set; }
//        public new IEnumerable<IdentityError> Errors => _errors;
//        public new static ADreamResult Success { get; }
//        public new static ADreamResult Failed(params IdentityError[] errors)
//        {
//            var result = new ADreamResult { Succeeded = false };
//            if (errors != null)
//            {
//                result._errors.AddRange(errors);
//            }
//            return result;
//        }
//        private static readonly ADreamResult _success = new ADreamResult { Succeeded = true };
//        private List<IdentityError> _errors = new List<IdentityError>();
//    }
//}
