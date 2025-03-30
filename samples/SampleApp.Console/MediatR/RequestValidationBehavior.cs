using FluentValidation;
using MediatR;

namespace SampleApp.Console.MediatR;

/// <summary>
/// A MediatR pipeline behavior that applies FluentValidation to incoming requests
/// before passing them to the next handler in the pipeline.
/// </summary>
/// <typeparam name="TRequest">The type of the request being handled.</typeparam>
/// <typeparam name="TResponse">The response type expected from the handler.</typeparam>
public class RequestValidationBehavior<TRequest, TResponse>(
    IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    /// <summary>
    /// Intercepts the MediatR pipeline and validates the request using all registered
    /// <see cref="IValidator{T}"/> implementations for <typeparamref name="TRequest"/>.
    /// </summary>
    /// <param name="request">The incoming request to validate.</param>
    /// <param name="next">The next delegate in the pipeline.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>The result of the request handler if validation passes.</returns>
    /// <exception cref="ValidationException">
    /// Thrown if any validation rules fail.
    /// </exception>
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!validators.Any())
            return await next();

        var context = new ValidationContext<TRequest>(request);

        var failures = await Task.WhenAll(
            validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var errors = failures
            .SelectMany(r => r.Errors)
            .Where(f => f != null)
            .ToList();

        if (errors.Count != 0)
            throw new ValidationException(errors);

        return await next();
    }
}
