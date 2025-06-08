namespace quirehut.order.api.HttpHandlers

open System
open Microsoft.AspNetCore.Http
open Microsoft.Extensions.Logging
open quirehut.order.api

module HttpHandlers =
    let getOrders (logger: ILogger) (customerId:string) =
        logger.LogInformation("GetOrders for customer {CustomerId} called", customerId)
        let demoOrder:CustomerOrder = 
            {
                OrderId = "12345"
                CustomerId = customerId
                OrderDate = DateTime.UtcNow
                ShippingAddress = { AddressLine1 = "123 Main St"; AddressLine2 = None; City = "Anytown"; PostalCode = "12345" }
                OrderLines = 
                    [
                        { ProductId = "prod1"; ProductName = "Product 1"; Quantity = 2 }
                        { ProductId = "prod2"; ProductName = "Product 2"; Quantity = 1 }
                    ]
            }
        Results.Ok demoOrder
        
    let placeOrder (logger: ILogger) (request:PlaceOrderRequest) =
        logger.LogInformation("PlaceOrder called with request: {Request}", request)
        let orderId = Guid.NewGuid().ToString()
        let confirmation:OrderConfirmation = 
            {
                OrderId = orderId
                Message = "Order placed successfully"
            }
        Results.Ok confirmation