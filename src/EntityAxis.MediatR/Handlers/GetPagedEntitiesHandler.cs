using EntityAxis.Abstractions;
using EntityAxis.MediatR.Queries;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace EntityAxis.MediatR.Handlers;

/// <summary>
/// Handles retrieving a page of entities using pagination parameters.
/// </summary>
/// <typeparam name="TEntity">The type of the entities being retrieved.</typeparam>
/// <typeparam name="TKey">The type of the entity's identifier.</typeparam>
public class GetPagedEntitiesHandler<TEntity, TKey> : IRequestHandler<GetPagedEntitiesQuery<TEntity, TKey>, PagedResult<TEntity>>
    where TEntity : class, IEntityId<TKey>
{
    private readonly IGetPaged<TEntity, TKey> _getPagedService;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetPagedEntitiesHandler{TEntity, TKey}"/> class.
    /// </summary>
    public GetPagedEntitiesHandler(IGetPaged<TEntity, TKey> getPagedService)
    {
        _getPagedService = getPagedService;
    }

    /// <inheritdoc />
    public Task<PagedResult<TEntity>> Handle(GetPagedEntitiesQuery<TEntity, TKey> request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return _getPagedService.GetPagedAsync(request.Page, request.PageSize, cancellationToken);
    }
}