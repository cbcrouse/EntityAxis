using AutoMapper;
using EntityAxis.EntityFramework;
using EntityAxis.KeyMappers;
using Microsoft.EntityFrameworkCore;
using SampleApp.Domain;
using SampleApp.Persistence.Entities;

namespace SampleApp.Persistence;

/// <summary>
/// Query service for retrieving <see cref="Product"/> entities from the database.
/// </summary>
public class ProductQueryService(
    IDbContextFactory<SampleAppDbContext> contextFactory,
    IMapper mapper,
    IKeyMapper<int, int> keyMapper)
    : EntityFrameworkQueryService<Product, ProductDbEntity, SampleAppDbContext, int, int>(contextFactory, mapper, keyMapper);