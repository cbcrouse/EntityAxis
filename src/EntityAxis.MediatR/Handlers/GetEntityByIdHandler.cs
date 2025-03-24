using System.Threading;
using System.Threading.Tasks;
using EntityAxis.Abstractions;
using EntityAxis.MediatR.Queries;
using MediatR;

namespace EntityAxis.MediatR.Handlers;

/// <summary>
/// Handles retrieving a single entity by its identifier.
/// </summary>
/// <typeparam name="TEntity">The type of the entity being retrieved.</typeparam>
/// <typeparam name="TKey">The type of the entity's identifier.</typeparam>
public class GetEntityByIdHandler<TEntity, TKey> : IRequestHandler<GetEntityByIdQuery<TEntity, TKey>, TEntity?>
    where TEntity : class, IEntityId<TKey>
{
    private readonly IGetById<TEntity, TKey> _getByIdService;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetEntityByIdHandler{TEntity, TKey}"/> class.
    /// </summary>
    public GetEntityByIdHandler(IGetById<TEntity, TKey> getByIdService)
    {
        _getByIdService = getByIdService;
    }

    /// <inheritdoc />
    public Task<TEntity?> Handle(GetEntityByIdQuery<TEntity, TKey> request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return _getByIdService.GetByIdAsync(request.Id, cancellationToken);
    }
}