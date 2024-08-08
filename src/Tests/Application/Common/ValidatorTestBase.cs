using FluentValidation;
using FluentValidation.TestHelper;

namespace Tests.Application.Common;

public abstract class ValidatorTestBase<TModel>
{
    protected abstract TModel CreateValidObject();

    protected TestValidationResult<TModel> Validate(Action<TModel> mutate)
    {
        var model = CreateValidObject();
        mutate(model);

        var validator = CreateValidator();

        return validator.TestValidate(model);
    }

    protected abstract IValidator<TModel> CreateValidator();
}