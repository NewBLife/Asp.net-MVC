using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TrimModelBinder.Framwork.ModelBinders
{
    //public class TrimModelBinder : DefaultModelBinder
    //{
    //    protected override void SetProperty(ControllerContext controllerContext,
    //      ModelBindingContext bindingContext,
    //      System.ComponentModel.PropertyDescriptor propertyDescriptor, object value)
    //    {
    //        if (propertyDescriptor.PropertyType == typeof(string))
    //        {
    //            string stringValue = (string)value;

    //            value = string.IsNullOrWhiteSpace(stringValue) ? stringValue : stringValue.Trim();
    //        }

    //        base.SetProperty(controllerContext, bindingContext,
    //                            propertyDescriptor, value);
    //    }
    //}

    public class TrimModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var valueResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            string attemptedValue = valueResult?.AttemptedValue;

            return string.IsNullOrWhiteSpace(attemptedValue) ? attemptedValue : attemptedValue.Trim();
        }
    }
}