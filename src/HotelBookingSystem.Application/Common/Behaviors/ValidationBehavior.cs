using FluentResults;
using FluentValidation;
using MediatR;
using ValidationException = System.ComponentModel.DataAnnotations.ValidationException;

namespace HotelBookingSystem.Application.Common.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!_validators.Any())
            return await next();

        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken))
        );

        var failures = validationResults
            .SelectMany(r => r.Errors)
            .Where(f => f != null)
            .ToList();

        if (failures.Any())
        {
            var responseType = typeof(TResponse);

            // Возвращаем FluentResults.Result
            if (responseType == typeof(Result))
            {
                var fail = Result.Fail("Validation failed");

                return (TResponse)(object)fail!;
            }

            // Возвращаем FluentResults.Result<T>
            if (responseType.IsGenericType && responseType.GetGenericTypeDefinition() == typeof(Result<>))
            {
                var fail = typeof(Result)
                    .GetMethods()
                    .First(m => m.Name == "Fail" && m.IsGenericMethod)
                    .MakeGenericMethod(responseType.GenericTypeArguments[0])
                    .Invoke(null, new object[] { "Validation failed" });

                typeof(Result)
                    .GetMethod("WithMetadata")!
                    .Invoke(fail, new object[] { "ValidationErrors", failures });

                return (TResponse)fail!;
            }

            throw new ValidationException(
                $"Validation failed for {typeof(TRequest).Name}: {string.Join(", ", failures.Select(f => f.ErrorMessage))}"
            );
        }

        return await next();
    }
}