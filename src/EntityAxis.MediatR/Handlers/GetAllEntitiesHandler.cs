using EntityAxis.Abstractions;
using EntityAxis.MediatR.Queries;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EntityAxis.MediatR.Handlers;

/// <summary>
/// Handles retrieving all entities of a given type.
/// </summary>
/// <typeparam name="TEntity">The type of the entities being retrieved.</typeparam>
/// <typeparam name="TKey">The type of the entity's identifier.</typeparam>
public class GetAllEntitiesHandler<TEntity, TKey> : IRequestHandler<GetAllEntitiesQuery<TEntity, TKey>, List<TEntity>>
    where TEntity : class, IEntityId<TKey>
{
    private readonly IGetAll<TEntity, TKey> _getAllService;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetAllEntitiesHandler{TEntity, TKey}"/> class.
    /// </summary>
    public GetAllEntitiesHandler(IGetAll<TEntity, TKey> getAllService)
    {
        _getAllService = getAllService;
    }

    /// <inheritdoc />
    public Task<List<TEntity>> Handle(GetAllEntitiesQuery<TEntity, TKey> request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return _getAllService.GetAllAsync(cancellationToken);
    }
}