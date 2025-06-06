namespace quirehut.order.api.HttpHandlers

open System
open Microsoft.AspNetCore.Http
open Microsoft.Extensions.Logging


module HttpHandlers =
    let getOrders (logger: ILogger) (customerId:string) =
        logger.LogInformation("GetOrders for customer {CustomerId} called", customerId)
        Results.Ok [| "Order1"; "Order2"; "Order3" |]