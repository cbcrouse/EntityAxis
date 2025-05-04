using AutoMapper;
using EntityAxis.EntityFramework;
using EntityAxis.KeyMappers;
using Microsoft.EntityFrameworkCore;
using SampleApp.Domain;
using SampleApp.Persistence.Entities;

namespace SampleApp.Persistence;

/// <summary>
/// Command service for managing <see cref="Product"/> entities in the database.
/// </summary>
public class ProductCommandService(
    IDbContextFactory<SampleAppDbContext> contextFactory,
    IMapper mapper,
    IKeyMapper<int, int> keyMapper)
    : EntityFrameworkCommandService<Product, ProductDbEntity, SampleAppDbContext, int, int>(contextFactory, mapper, keyMapper);