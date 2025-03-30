using AutoMapper;
using EntityAxis.EntityFramework;
using Microsoft.EntityFrameworkCore;
using SampleApp.Domain;
using SampleApp.Persistence.Entities;

namespace SampleApp.Persistence;

public class ProductCommandService(IDbContextFactory<ProductDbContext> contextFactory, IMapper mapper)
    : EntityFrameworkCommandService<Product, ProductDbEntity, ProductDbContext, int>(contextFactory, mapper);