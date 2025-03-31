using EntityAxis.Abstractions;
using EntityAxis.MediatR.Commands;
using EntityAxis.MediatR.Exceptions;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EntityAxis.MediatR.Handlers;

/// <summary>
/// Handles deletion of an entity by its identifier.
/// </summary>
/// <typeparam name="TEntity">The type of the entity to be deleted.</typeparam>
/// <typeparam name="TKey">The type of the entity's identifier.</typeparam>
/// <remarks>
/// If the entity cannot be found, this handler will throw an <see cref="ApplicationException"/> by default.
/// Consumers may override this behavior by customizing the command or implementing a soft-delete variant.
/// </remarks>
public class DeleteEntityHandler<TEntity, TKey> : IRequestHandler<DeleteEntityCommand<TEntity, TKey>>
    where TEntity : class, IEntityId<TKey>
{
    private readonly IGetById<TEntity, TKey> _getByIdService;
    private readonly IDelete<TEntity, TKey> _deleteService;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteEntityHandler{TEntity, TKey}"/> class.
    /// </summary>
    /// <param name="getByIdService">The service used to retrieve the entity by ID before deletion.</param>
    /// <param name="deleteService">The service used to perform the deletion.</param>
    public DeleteEntityHandler(
        IGetById<TEntity, TKey> getByIdService,
        IDelete<TEntity, TKey> deleteService)
    {
        _getByIdService = getByIdService;
        _deleteService = deleteService;
    }

    /// <inheritdoc />
    public async Task Handle(DeleteEntityCommand<TEntity, TKey> request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var entity = await _getByIdService.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null)
        {
            throw new EntityNotFoundException(typeof(TEntity).Name, request.Id);
        }

        await _deleteService.DeleteAsync(request.Id, cancellationToken);
    }
}
