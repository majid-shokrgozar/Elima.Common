using Elima.Common.Results;
using FluentValidation;
using MediatR;

namespace PersonalWeb.Shared.PipelineBehavior;

public class ValidationPipelineBehavior<TRequest, TResponse>
    (IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!validators.Any())
            return await next();

        var validationError = validators
            .Select(validator => validator.Validate(request))
            .SelectMany(validationResult => validationResult.Errors)
            .Where(validationFailure => validationFailure is not null)
            .Select(failure => new ValidationError(
                    failure.PropertyName,
                    failure.ErrorMessage,
                    failure.AttemptedValue,
                    failure.ErrorCode
                ))
            .Distinct()
            .ToList();

        if (validationError.Any())
            return CreateValidationResult<TResponse>(validationError);

        return await next();
    }

    private static TResult CreateValidationResult<TResult>(List<ValidationError> validationErrors)
        where TResult : Result
    {
        if (typeof(TResult) == typeof(Result))
            return (TResult)Result.InvalidRequest([.. validationErrors]);

       // var methed = typeof(Result<>)
       //     .GetGenericTypeDefinition()
       //     .MakeGenericType(typeof(TResult).GenericTypeArguments[0])
       //     .GetMethod(nameof(Result.InvalidRequest));

       //object? validationResult1 = methed?.Invoke(null, [validationErrors]);

       object validationResult = typeof(Result<>)
            .GetGenericTypeDefinition()
            .MakeGenericType(typeof(TResult).GenericTypeArguments[0])
            .GetMethod(nameof(Result.InvalidRequest))!
            .Invoke(null, new object?[] { validationErrors })!;

        return (TResult)validationResult;

    }
}
