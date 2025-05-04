using AutoMapper;
using EntityAxis.EntityFramework;
using EntityAxis.KeyMappers;
using Microsoft.EntityFrameworkCore;
using SampleApp.Domain;
using SampleApp.Persistence;
using SampleApp.Persistence.Entities;

namespace SampleApp.Persistence;

/// <summary>
/// Query service for retrieving orders.
/// </summary>
public class OrderQueryService(
    IDbContextFactory<SampleAppDbContext> context,
    IMapper mapper,
    IKeyMapper<string, Guid> keyMapper)
    : EntityFrameworkQueryService<Order, OrderDbEntity, SampleAppDbContext, string, Guid>(context, mapper, keyMapper);
