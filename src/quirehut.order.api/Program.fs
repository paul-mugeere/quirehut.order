namespace quirehut.order.api
#nowarn "20"
open System
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Http
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open quirehut.order.api.HttpHandlers
open quirehut.order.api.HttpHandlers.HttpHandlers

module Program =
    let exitCode = 0    
    let createWebApplicationBuilder (args:string[]) =
        let builder = WebApplication.CreateBuilder(args)
        builder.Services.ConfigureHttpJsonOptions(fun options ->
            options.SerializerOptions.PropertyNamingPolicy <- null
        ) |> ignore
        builder
    
    let createWebApplication (builder:WebApplicationBuilder) =
        builder.Services.AddAuthorization() |> ignore
        let app = builder.Build()
        app.UseHttpsRedirection()
        app.UseAuthorization()
        app.MapGet("healthz", Func<IResult>(fun () -> Results.Ok("Healthzing..."))) |> ignore
        app.MapGet("api/orders/{customerId}", Func<string, IResult>(fun (customerId) -> getOrders app.Logger customerId)) |> ignore
        app.MapPost("api/orders", Func<PlaceOrderRequest, IResult>(fun (request) -> placeOrder app.Logger request)) |> ignore
        app
    

    [<EntryPoint>]
    let main args =
        let builder = createWebApplicationBuilder args
        let app = createWebApplication builder
        app.Run()

        exitCode