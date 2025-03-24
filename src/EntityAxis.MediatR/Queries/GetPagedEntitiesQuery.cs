using EntityAxis.Abstractions;
using MediatR;

namespace EntityAxis.MediatR.Queries;

/// <summary>
/// Represents a MediatR query for retrieving a page of entities.
/// </summary>
/// <typeparam name="TEntity">The type of the entity to retrieve.</typeparam>
/// <typeparam name="TKey">The type of the entity's identifier.</typeparam>
public class GetPagedEntitiesQuery<TEntity, TKey> : IRequest<PagedResult<TEntity>>
    where TEntity : IEntityId<TKey>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetPagedEntitiesQuery{TEntity, TKey}"/> class.
    /// </summary>
    /// <param name="page">The page number to retrieve.</param>
    /// <param name="pageSize">The number of items per page.</param>
    public GetPagedEntitiesQuery(int page, int pageSize)
    {
        Page = page;
        PageSize = pageSize;
    }

    /// <summary>
    /// The page number to retrieve (1-based).
    /// </summary>
    public int Page { get; }

    /// <summary>
    /// The number of items to retrieve per page.
    /// </summary>
    public int PageSize { get; }
}