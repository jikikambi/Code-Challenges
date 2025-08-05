using CodeChallenge.ApplicationLayer;
using CodeChallenge.InfrastructureLayer;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Order.Service.Api.Application.Mappers;
using Order.Service.Api.Application.RequestHandlers.Commands.Create;
using Order.Service.Api.Application.RequestHandlers.Commands.Delete;
using Order.Service.Api.Application.RequestHandlers.Queries;
using Order.Service.Shared.Request;
using System.Diagnostics.CodeAnalysis;
using OrderRdm = CodeChallenge.DomainLayer.Entities.Order;
using Order.Service.Shared.Response;

namespace Order.Service.Api;

[ExcludeFromCodeCoverage(Justification = JustificationReason.NoLogic)]
public static class ServiceCollectionExtensions
{
    public static WebApplication RegisterServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddHealthChecks();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Order Service API",
                Version = "v1",
                Description = "Handles orders"
            });
        });

        builder.Services.AddPersistenceServices(builder.Configuration);
        builder.Services.AddMappings();
        builder.Services.AddRequestHandlers(builder.Configuration);

        return builder.Build();
    }

    public static async Task<WebApplication> SetupMiddleware(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            await app.ResetDatabaseAsync();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.RegisterEndPoints();

        return app;
    }

    public static void RegisterEndPoints(this WebApplication app)
    {
        app.MapPost("/orders", async ([FromBody] CreateOrderRequest request, IMediator mediator) =>
        {
            var req = new CreateOrderCommandRequest(request);
            var orderId = await mediator.Send(req);
            return Results.Created($"/orders/{orderId}", new CreateOrderResponse(orderId));
        });

        app.MapGet("/orders/{orderNumber:guid}", async (Guid orderNumber, IMediator mediator) =>
        {
            var orderDetail = new OrderQueryRequest(orderNumber);
            var request = new GetOrderByIdQueryRequest(orderDetail);

            var order = await mediator.Send(request);
            return order is not null ? Results.Ok(order) : Results.NotFound();
        });

        app.MapDelete("/orders/{orderNumber:guid}", async (Guid orderNumber, IMediator mediator) =>
        {
            var command = new DeleteOrderRequest(orderNumber);
            var request = new DeleteOrderCommandRequest(command);

            await mediator.Send(request);
            return Results.NoContent();
        });

        app.MapHealthChecks("/health");
    }

    public static void AddMappings(this IServiceCollection services)
    {
        services.AddScoped<IOrderResponseMapper, OrderResponseMapper>();
        services.AddScoped<ICreateOrderMapper, CreateOrderMapper>();
    }

    public static void AddRequestHandlers(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatRForApi<
            Program,
            GetOrderByIdQueryRequest,
            OrderResponse,
            OrderQueryRequest
            >(configuration);

        services.AddMediatRForApi<
            Program,
            CreateOrderCommandRequest,
            Guid,
            CreateOrderRequest
            >(configuration);

        services.AddMediatRForApi<
            Program,
            DeleteOrderCommandRequest,
            Unit,
            DeleteOrderRequest
            >(configuration);
    }

    public static async Task ResetDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        try
        {
            var context = scope.ServiceProvider.GetRequiredService<OrderDbContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<ResetDatabase>>();

            logger.LogInformation("Resetting the database...");

            await context.Database.EnsureDeletedAsync();
            await context.Database.MigrateAsync();

            if (!context.Orders.Any())
            {
                context.Orders.Add(new OrderRdm(
                    new("Fake Street 04"), new("fake@example.com"), new("1111-2222-3333-0000"),
                    [
                        new("fake01", "Fake Item", 1, 9.99m)
                    ]));

                await context.SaveChangesAsync();
                logger.LogInformation("Database faked with default order.");
            }
        }
        catch (Exception ex)
        {
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<ResetDatabase>>();
            logger.LogError(ex, "An error occurred while resetting the database.");
        }
    }

    public class ResetDatabase { }

}