using EntityAxis.KeyMappers;
using EntityAxis.MediatR.Commands;
using EntityAxis.MediatR.Queries;
using EntityAxis.MediatR.Registration;
using EntityAxis.Registration;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SampleApp.Application.Models;
using SampleApp.Application.Validators;
using SampleApp.Domain;
using SampleApp.Infrastructure.Mapping;
using SampleApp.Persistence;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        // Register in-memory DbContext
        services.AddDbContextFactory<SampleAppDbContext>(options =>
        {
            options.UseInMemoryDatabase("SampleAppDb");
        });

        // Register AutoMapper
        services.AddAutoMapper(typeof(EntityProfile).Assembly);

        // Register key mappers
        services.AddSingleton<IKeyMapper<int, int>, IdentityKeyMapper<int>>();
        services.AddSingleton<IKeyMapper<string, Guid>, StringToGuidKeyMapper>();

        // Register command/query services
        services.AddEntityAxisCommandAndQueryServicesFromAssembly<ProductCommandService>();

        // Register MediatR handlers and validators
        services.AddMediatR(config => config.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));
        services.AddEntityAxisHandlers<ProductCreateModel, ProductUpdateModel, Product, int>();
        services.AddEntityAxisHandlers<OrderCreateModel, OrderUpdateModel, Order, string>();

        // Register FluentValidation validators
        services.AddValidatorsFromAssemblyContaining<ProductUpdateModelValidator>();
    })
    .Build();

using var scope = host.Services.CreateScope();
var services = scope.ServiceProvider;

try
{
    var mediator = services.GetRequiredService<IMediator>();

    Console.WriteLine("=== ENTITYAXIS CRUD DEMO ===\n");

    Console.WriteLine("[SEED] Creating sample products...\n");
    var createTasks = new List<Task<int>>();
    for (int i = 1; i <= 25; i++)
    {
        var model = new ProductCreateModel
        {
            Name = $"Product {i}",
            Description = $"Product {i} description",
            Price = 10.00m + i
        };

        var id = await mediator.Send(new CreateEntityCommand<ProductCreateModel, Product, int>(model));
        Console.WriteLine($"  -> Created: {model.Name} with ID: {id}");
    }

    Console.WriteLine("\n[READ] Fetching all products...\n");
    var allProducts = await mediator.Send(new GetAllEntitiesQuery<Product, int>());
    Console.WriteLine($"  -> Total products in database: {allProducts.Count}");

    Console.WriteLine("\n[PAGING] Fetching products in pages of 10:\n");

    int page = 1;
    const int pageSize = 10;
    while (true)
    {
        var paged = await mediator.Send(new GetPagedEntitiesQuery<Product, int>(page, pageSize));
        Console.WriteLine($"  Page {page}: {paged.Items.Count} item(s)");

        foreach (var product in paged.Items)
        {
            Console.WriteLine($"    - {product.Id,2}: {product.Name,-20} ${product.Price,6:0.00}");
        }

        if (paged.Items.Count < pageSize)
            break;

        page++;
    }

    Console.WriteLine("\n[UPDATE] Updating Product 5...\n");

    var updateModel = new ProductUpdateModel
    {
        Id = 5,
        Name = "Updated Product 5",
        Description = "Now even better",
        Price = 42.00m
    };

    var updatedId = await mediator.Send(new UpdateEntityCommand<ProductUpdateModel, Product, int>(updateModel));
    Console.WriteLine($"  -> Updated: {updateModel.Name} - {updateModel.Description} - ${updateModel.Price}");

    Console.WriteLine("\n[DELETE] Removing Products 1, 2, and 3...\n");

    foreach (var id in new[] { 1, 2, 3 })
    {
        await mediator.Send(new DeleteEntityCommand<Product, int>(id));
        Console.WriteLine($"  -> Deleted product ID {id}");
    }

    Console.WriteLine("\n[VERIFY] Verifying deletions...\n");

    foreach (var id in new[] { 1, 2, 3 })
    {
        var deleted = await mediator.Send(new GetEntityByIdQuery<Product, int>(id));
        Console.WriteLine($"  -> Product {id}: {(deleted == null ? "NOT FOUND" : "STILL EXISTS")}");
    }

    // === DEMONSTRATING TKey (string) mapped to TDbKey (Guid) for Order entity ===

    Console.WriteLine("\n[SEED] Creating multiple orders...\n");

    var orderIds = new List<string>();

    for (int i = 1; i <= 5; i++)
    {
        var model = new OrderCreateModel
        {
            CustomerName = $"Customer {i}",
            TotalAmount = 100.00m + (i * 10)
        };

        var id = await mediator.Send(new CreateEntityCommand<OrderCreateModel, Order, string>(model));
        orderIds.Add(id);
        Console.WriteLine($"  -> Created order for {model.CustomerName} with ID: {id} (Valid Guid: {Guid.TryParse(id, out _)} )");
    }

    Console.WriteLine("\n[READ] Fetching all orders...\n");
    var allOrders = await mediator.Send(new GetAllEntitiesQuery<Order, string>());
    Console.WriteLine($"  -> Total orders in database: {allOrders.Count}");
    foreach (var order in allOrders)
    {
        Console.WriteLine($"    - {order.Id}: {order.CustomerName} (${order.TotalAmount})");
    }

    Console.WriteLine("\n[PAGING] Fetching orders in pages of 2:\n");
    int orderPage = 1;
    const int orderPageSize = 2;
    while (true)
    {
        var paged = await mediator.Send(new GetPagedEntitiesQuery<Order, string>(orderPage, orderPageSize));
        if (paged.Items.Count == 0) break;

        Console.WriteLine($"  Page {orderPage}:");
        foreach (var order in paged.Items)
        {
            Console.WriteLine($"    - {order.Id}: {order.CustomerName} (${order.TotalAmount})");
        }

        if (paged.Items.Count < orderPageSize)
            break;

        orderPage++;
    }

    Console.WriteLine("\n[UPDATE] Updating first order...\n");
    var firstOrderId = orderIds.First();
    var updateOrder = new OrderUpdateModel
    {
        Id = firstOrderId,
        CustomerName = "Customer 1 - Updated",
        TotalAmount = 250.00m
    };
    await mediator.Send(new UpdateEntityCommand<OrderUpdateModel, Order, string>(updateOrder));
    Console.WriteLine($"  -> Updated order: {updateOrder.CustomerName} (${updateOrder.TotalAmount})");

    Console.WriteLine("\n[DELETE] Deleting first order...\n");
    await mediator.Send(new DeleteEntityCommand<Order, string>(firstOrderId));
    Console.WriteLine($"  -> Deleted order ID: {firstOrderId}");

    Console.WriteLine("\n[VERIFY] Verifying deletion...\n");
    var deletedOrder = await mediator.Send(new GetEntityByIdQuery<Order, string>(firstOrderId));
    Console.WriteLine($"  -> Order {firstOrderId}: {(deletedOrder == null ? "NOT FOUND" : "STILL EXISTS")}");

    Console.WriteLine("\n=== DEMO COMPLETE ===");
}
catch (Exception ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"[ERROR] {ex.Message}");
    Console.ResetColor();
}
