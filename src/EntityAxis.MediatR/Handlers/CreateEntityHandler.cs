using AutoMapper;
using EntityAxis.Abstractions;
using EntityAxis.MediatR.Commands;
using EntityAxis.MediatR.Internal;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EntityAxis.MediatR.Handlers;

/// <summary>
/// Handles the creation of a new entity using a command model and AutoMapper.
/// </summary>
/// <typeparam name="TModel">The model type used to create the entity.</typeparam>
/// <typeparam name="TEntity">The type of the entity being created.</typeparam>
/// <typeparam name="TKey">The type of the entity's identifier.</typeparam>
/// <remarks>
/// An AutoMapper mapping must be explicitly configured from <typeparamref name="TModel"/> to <typeparamref name="TEntity"/>.
/// This handler relies on <see cref="IMapper"/>.Map&lt;TDestination&gt;(object) to transform the model into a <typeparamref name="TEntity"/>.
/// Failure to configure the mapping will result in a runtime <see cref="AutoMapperMappingException" />.
/// </remarks>
public class CreateEntityHandler<TModel, TEntity, TKey> : IRequestHandler<CreateEntityCommand<TModel, TEntity, TKey>, TKey>
    where TEntity : IEntityId<TKey>
{
    private readonly ICreate<TEntity, TKey> _createService;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateEntityHandler{TModel, TEntity, TKey}"/> class.
    /// </summary>
    /// <param name="createService">The service used to persist the new entity.</param>
    /// <param name="mapper">The AutoMapper instance to map the model to the entity. Must be configured with a valid CreateMap.</param>
    public CreateEntityHandler(ICreate<TEntity, TKey> createService, IMapper mapper)
    {
        _createService = createService;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<TKey> Handle(CreateEntityCommand<TModel, TEntity, TKey> request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        TEntity entity;
        try
        {
            entity = _mapper.Map<TEntity>(request.CreateModel);
        }
        catch (AutoMapperMappingException ex)
        {
            var message = AutoMapperErrorFormatter.Format<TModel, TEntity>(ex);
            throw new InvalidOperationException(message, ex);
        }

        return await _createService.CreateAsync(entity, cancellationToken);
    }
}
