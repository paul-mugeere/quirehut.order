module Tests

open System
open Xunit
open quirehut.order.domain
open quirehut.order.domain.PlaceOrder



[<Fact>]
let ``Given product does not exists, validateOrderLines should return ValidationError`` () =
    let unvalidatedLine = {
        Id = "order-1"
        ProductId = "invalid-product"
        Quantity = 1
    }
    
    let checkProductExists _ = false
    let orderId = OrderId.create "test-order-id"
    let result = validateOrderLines orderId checkProductExists [unvalidatedLine]
    
    match result with
    | Error e -> Assert.True(e.FieldName = "Products" && e.ErrorDescription = "Product of Id: invalid-product does not exist")
    | Ok _ -> Assert.Fail("Expected validation error")

[<Fact>]
let ``Given valid inputs, validateOrder should succeed`` () =
    let address: UnvalidatedAddress = {
        AddressLine1 = "test-address-line-1"
        AddressLine2 = "test-address-line-2" |> option.Some
        City = "test-city"
        PostalCode = "test-postal-code"
    }
    let checkAddressExists _ = CheckedAddress address
    let unvalidatedOrder: UnvalidatedOrder =
        { OrderId = "test-order"
          CustomerInfo = {
              Firstname = "John"
              Lastname = "Doe"
              EmailAddress = "john@example.com"
          }
          ShippingAddress = address
          OrderLines = [{
              Id = "test-order-line"
              ProductId = "test-product-id"
              Quantity = 1
          }]
        }
    
    let checkProductExists _ = true
    let result = validateOrder checkAddressExists checkProductExists unvalidatedOrder
    
    match result with 
    | Ok validatedOrder -> 
        Assert.True(validatedOrder.OrderLines |> List.exists (fun line -> line.ProductId = ProductId.create "test-product-id")
        )
    | Error _ -> Assert.Fail"Expected successful validation"


[<Fact>]
let ``Given valid order, placeOrder should succeed`` () =
    let address: UnvalidatedAddress = {
        AddressLine1 = "test-address-line-1"
        AddressLine2 = "test-address-line-2" |> option.Some
        City = "test-city"
        PostalCode = "test-postal-code"
    }
    let unvalidatedOrder: UnvalidatedOrder =
        { OrderId = "test-order"
          CustomerInfo = {
              Firstname = "John"
              Lastname = "Doe"
              EmailAddress = "john@example.com"
          }
          ShippingAddress = address
          OrderLines = [{
              Id = "test-order-line"
              ProductId = "test-product-id"
              Quantity = 1
          }]
        }
    
    let checkProductExists _ = true
    let getProductPrice _ = 100M |> Price.create
    let createAcknowledgementMessage _ = "test-acknowledgement-message" |> HtmlString
    let sendOrderAcknowledgement _ = Sent
    let checkAddressExists _ = CheckedAddress address
    let placeOrderCommand = { 
        Data = unvalidatedOrder; 
        Userid = "test-user";
        TimeStamp = DateTime.Now
    }
    
    let result = placeOrder checkAddressExists checkProductExists getProductPrice createAcknowledgementMessage sendOrderAcknowledgement placeOrderCommand
    
    match result with
    | Ok events -> Assert.True(events |> List.exists (fun e -> e.IsOrderPlaced))
    | Error e -> Assert.Fail("Expected successful placement")
 
 
[<Fact>]
let ``Given timeout, validateAddress should return ValidationError`` () =
    
    let unvalidatedAddress: UnvalidatedAddress = {
        AddressLine1 = "test-address-line-1"
        AddressLine2 = "test-address-line-2" |> option.Some
        City = "test-city"
        PostalCode = "test-postal-code"
    }
    
    let checkAddressExists _ = raise (TimeoutException("Address validation service timed out"))
    
    let result = checkAddressExistsAdapted checkAddressExists unvalidatedAddress
    
    match result with
    | Error e -> 
        Assert.True(e.ServiceInfo.Name = "Address Validation Service" && e.Exception.Message = "Address validation service timed out")

    | Ok _ -> Assert.Fail"Timeout error expected"