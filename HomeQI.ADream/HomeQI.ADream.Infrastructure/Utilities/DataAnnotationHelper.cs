//using Microsoft.AspNetCore.Mvc.DataAnnotations.Internal;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.ComponentModel.DataAnnotations;
//using System.Linq;
//using System.Text;

//namespace HomeQI.ADream.Infrastructure.HomeQI.ADreamities
//{
//    public class ModelValidationError
//    {
//        public string FieldName { get; set; }
//        public string Message { get; set; }
//    }
//    public static class DataAnnotationHelper
//    {
//        public static IEnumerable<ModelValidationError> IsValid<T>(this T o)
//        {
//            var descriptor = GetTypeDescriptor(typeof(T));

//            foreach (PropertyDescriptor propertyDescriptor in descriptor.GetProperties())
//            {
//                var validations = propertyDescriptor.Attributes.OfType<ValidationAttribute>();
//                foreach (var validationAttribute in validations)
//                {
//                    var v = propertyDescriptor.GetValue(o);

//                    if (!validationAttribute.IsValid(v))
//                    {
//                        yield return new ModelValidationError() { FieldName = propertyDescriptor.Name, Message = validationAttribute.FormatErrorMessage(propertyDescriptor.Name) };
//                    }
//                }
//            }
//        }
//        private static ICustomTypeDescriptor GetTypeDescriptor(Type type)
//        {
//            return new AssociatedMetadataTypeTypeDescriptionProvider(type).GetTypeDescriptor(type);
//        }
//    }
//}
