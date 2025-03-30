using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace EntityAxis.EntityFramework;

/// <summary>
/// Provides base functionality for EF Core services by exposing the DbContext factory and AutoMapper instance.
/// </summary>
/// <typeparam name="TDbContext">The application's <see cref="DbContext"/> implementation.</typeparam>
public abstract class EntityFrameworkServiceBase<TDbContext>
    where TDbContext : DbContext
{
    /// <summary>
    /// Gets the <see cref="IDbContextFactory{TContext}"/> used to create instances of the <typeparamref name="TDbContext"/>.
    /// </summary>
    protected IDbContextFactory<TDbContext> ContextFactory { get; }

    /// <summary>
    /// Gets the <see cref="IMapper"/> used to map between entity and model types.
    /// </summary>
    protected IMapper Mapper { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="EntityFrameworkServiceBase{TDbContext}"/> class.
    /// </summary>
    /// <param name="contextFactory">The factory used to create DbContext instances.</param>
    /// <param name="mapper">The AutoMapper instance.</param>
    protected EntityFrameworkServiceBase(IDbContextFactory<TDbContext> contextFactory, IMapper mapper)
    {
        ContextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
        Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }
}
