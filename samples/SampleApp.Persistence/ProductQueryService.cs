using AutoMapper;
using EntityAxis.EntityFramework;
using Microsoft.EntityFrameworkCore;
using SampleApp.Domain;
using SampleApp.Persistence.Entities;

namespace SampleApp.Persistence;

/// <summary>
/// Query service for retrieving <see cref="Product"/> entities from the database.
/// </summary>
public class ProductQueryService(IDbContextFactory<ProductDbContext> contextFactory, IMapper mapper)
    : EntityFrameworkQueryService<Product, ProductDbEntity, ProductDbContext, int>(contextFactory, mapper);