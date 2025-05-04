using AutoMapper;
using EntityAxis.EntityFramework;
using EntityAxis.KeyMappers;
using Microsoft.EntityFrameworkCore;
using SampleApp.Domain;
using SampleApp.Persistence;
using SampleApp.Persistence.Entities;

namespace SampleApp.Persistence;

/// <summary>
/// Command service for managing orders.
/// </summary>
public class OrderCommandService(
    IDbContextFactory<SampleAppDbContext> context,
    IMapper mapper,
    IKeyMapper<string, Guid> keyMapper)
    : EntityFrameworkCommandService<Order, OrderDbEntity, SampleAppDbContext, string, Guid>(context, mapper, keyMapper);
