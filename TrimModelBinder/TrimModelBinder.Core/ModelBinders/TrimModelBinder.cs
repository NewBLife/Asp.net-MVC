using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace TrimModelBinder.Core.ModelBinders
{
    public class TrimModelBinder : ComplexTypeModelBinder
    {
        public TrimModelBinder(IDictionary<ModelMetadata, IModelBinder> propertyBinders) : base(propertyBinders) { }

        protected override void SetProperty(ModelBindingContext bindingContext, string modelName, ModelMetadata propertyMetadata, ModelBindingResult result)
        {
            var value = result.Model as string;

            result= string.IsNullOrWhiteSpace(value) ? result : ModelBindingResult.Success(value.Trim());

            base.SetProperty(bindingContext, modelName, propertyMetadata, result);
        }
    }

    public class TrimModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context.Metadata.IsComplexType && !context.Metadata.IsCollectionType)
            {
                var propertyBinders = new Dictionary<ModelMetadata, IModelBinder>();
                for (int i = 0; i < context.Metadata.Properties.Count; i++)
                {
                    var property = context.Metadata.Properties[i];
                    propertyBinders.Add(property, context.CreateBinder(property));
                }
                return new TrimModelBinder(propertyBinders);
            }
            return null;
        }
    }

    //public class TrimModelBinder : IModelBinder
    //{
    //    public Task BindModelAsync(ModelBindingContext bindingContext)
    //    {
    //        if (bindingContext == null)
    //        {
    //            throw new ArgumentNullException(nameof(bindingContext));
    //        }

    //        //Get the value
    //        var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
    //        if (valueProviderResult == ValueProviderResult.None)
    //        {
    //            // no entry
    //            return Task.CompletedTask;
    //        }
            
    //        bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueProviderResult);

    //        //Set the value, this has to match the property type.
    //        var typeConverter = TypeDescriptor.GetConverter(bindingContext.ModelType);
    //        var propValue = typeConverter.ConvertFromString(valueProviderResult.FirstValue.Trim());
    //        bindingContext.Result = ModelBindingResult.Success(propValue);

    //        return Task.CompletedTask;
    //    }
    //}
}
