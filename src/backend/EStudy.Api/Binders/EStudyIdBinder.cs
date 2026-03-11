using Microsoft.AspNetCore.Mvc.ModelBinding;
using Sqids;

namespace EStudy.Api.Binders;

public class EStudyIdBinder : IModelBinder
{
private readonly SqidsEncoder<long> _idEncoder;

public EStudyIdBinder(SqidsEncoder<long> idEncoder) => _idEncoder = idEncoder;

public Task BindModelAsync(ModelBindingContext bindingContext)
{
    var modelName = bindingContext.ModelName;

    var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);

    if (valueProviderResult == ValueProviderResult.None)
        return Task.CompletedTask;

    bindingContext.ModelState.SetModelValue(modelName, valueProviderResult);

    var value = valueProviderResult.FirstValue;

    if (string.IsNullOrWhiteSpace(value))
        return Task.CompletedTask;

    var id = _idEncoder.Decode(value).Single();

    bindingContext.Result = ModelBindingResult.Success(id);

    return Task.CompletedTask;
}
}