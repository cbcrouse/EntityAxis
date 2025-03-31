using AutoMapper;
using EntityAxis.Abstractions;
using EntityAxis.MediatR.Commands;
using EntityAxis.MediatR.Internal;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using EntityAxis.MediatR.Exceptions;

namespace EntityAxis.MediatR.Handlers;

/// <summary>
/// Handles updating an existing entity using a command model and AutoMapper.
/// </summary>
/// <typeparam name="TModel">The update model type.</typeparam>
/// <typeparam name="TEntity">The type of the entity being updated.</typeparam>
/// <typeparam name="TKey">The type of the entity's identifier.</typeparam>
/// <remarks>
/// An AutoMapper mapping must be explicitly configured from <typeparamref name="TModel"/> to <typeparamref name="TEntity"/>.
/// This handler relies on <see cref="IMapper"/>.Map(source, destination)
/// to transform the update model into the existing <typeparamref name="TEntity"/> instance.
/// <para>
/// The entity's <c>Id</c> is preserved during mapping to prevent accidental overwrites. Consumers should avoid
/// modifying identifiers through update commands. If identifier updates are needed, implement a custom handler.
/// </para>
/// </remarks>
public class UpdateEntityHandler<TModel, TEntity, TKey> : IRequestHandler<UpdateEntityCommand<TModel, TEntity, TKey>, TKey>
    where TEntity : class, IEntityId<TKey>
    where TModel : IUpdateCommandModel<TEntity, TKey>
{
    private readonly IGetById<TEntity, TKey> _getByIdService;
    private readonly IUpdate<TEntity, TKey> _updateService;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateEntityHandler{TModel, TEntity, TKey}"/> class.
    /// </summary>
    /// <param name="getByIdService">The service used to retrieve the existing entity by ID.</param>
    /// <param name="updateService">The service used to persist the updated entity.</param>
    /// <param name="mapper">The AutoMapper instance to patch the existing entity from the update model.</param>
    public UpdateEntityHandler(
        IGetById<TEntity, TKey> getByIdService,
        IUpdate<TEntity, TKey> updateService,
        IMapper mapper)
    {
        _getByIdService = getByIdService;
        _updateService = updateService;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<TKey> Handle(UpdateEntityCommand<TModel, TEntity, TKey> request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var entity = await _getByIdService.GetByIdAsync(request.UpdateModel.Id, cancellationToken);
        if (entity == null)
        {
            throw new EntityNotFoundException(typeof(TEntity).Name, request.UpdateModel.Id);
        }

        TKey originalId = entity.Id;

        try
        {
            _mapper.Map(request.UpdateModel, entity);
        }
        catch (AutoMapperMappingException ex)
        {
            var message = AutoMapperErrorFormatter.Format<TModel, TEntity>(ex);
            throw new InvalidOperationException(message, ex);
        }

        // Ensure ID was not altered during mapping
        entity.Id = originalId;

        await _updateService.UpdateAsync(entity, cancellationToken);
        return entity.Id;
    }
}
