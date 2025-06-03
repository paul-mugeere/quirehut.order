namespace quirehut.order.domain

open System
open Microsoft.AspNetCore.Components.Forms
open Microsoft.FSharp.Collections
open quirehut.order.domain
open quirehut.order.domain.ResultBuilder

module PlaceOrder =

    let toCustomerInfo (unvalidatedCustomerInfo: UnvalidatedCustomerInfo) : Result<CustomerInfo, ValidationError> =
        try
            let fullname =
                Fullname.create unvalidatedCustomerInfo.Firstname unvalidatedCustomerInfo.Lastname

            let email = unvalidatedCustomerInfo.EmailAddress |> EmailAddress.create

            let customerInfo =
                { Fullname = fullname
                  EmailAddress = email }

            Ok customerInfo
        with ex ->
            ValidationError.create "CustomerInfo" ex.Message |> Error


    let toAddress (unvalidatedAddress: CheckedAddress) : Address =
        let (CheckedAddress checkedAddress) = unvalidatedAddress

        let address =
            Address.create
                checkedAddress.AddressLine1
                checkedAddress.AddressLine2
                checkedAddress.City
                checkedAddress.PostalCode

        address

    let toValidQuantity qty =
        try
            qty |> UnitQuantity.create |> Ok
        with ex ->
            ValidationError.create "Quantity" ex.Message |> Error

    let toValidOrderLineId orderLineId =
        try
            orderLineId |> OrderLineId.create |> Ok
        with ex ->
            ValidationError.create "OrderLine" ex.Message |> Error

    let checkProductIdExists checkProductExists productId =
        let error = $"Product of Id: {productId} does not exist"

        match productId |> checkProductExists with
        | false -> error |> ValidationError.create "Products" |> Result.Error
        | true -> productId |> ProductId.create |> Result.Ok

    let validateOrderLine
        (orderId: OrderId)
        (checkProductExists: CheckProductExists)
        (unvalidatedOrderLine: UnvalidatedOrderLine)
        : Result<ValidatedOrderLine, ValidationError> =

        result {
            let! productId = unvalidatedOrderLine.ProductId |> checkProductIdExists checkProductExists
            and! quantity = unvalidatedOrderLine.Quantity |> toValidQuantity
            and! orderLineId = unvalidatedOrderLine.Id |> toValidOrderLineId

            return
                { Id = orderLineId
                  ProductId = productId
                  OrderId = orderId |> Some
                  Quantity = quantity }
        }

    let validateOrderLines
        (orderId: OrderId)
        checkProductExists
        (unvalidatedOrderLines: UnvalidatedOrderLine list)
        : Result<ValidatedOrderLine list, ValidationError> =

        let validatedOrderLines =
            unvalidatedOrderLines |> List.map (validateOrderLine orderId checkProductExists)

        if
            List.exists
                (function
                | Error _ -> true
                | _ -> false)
                validatedOrderLines
        then
            let error =
                validatedOrderLines
                |> List.choose (function
                    | Ok _ -> None
                    | Error e -> Some e)
                |> List.head

            let error = ValidationError.create error.FieldName error.ErrorDescription
            Error error
        else
            validatedOrderLines
            |> List.choose (function
                | Ok line -> Some line
                | _ -> None)
            |> Ok


    let checkAddressExistsAdapted
        (checkAddressExists: CheckAddressExists)
        (unvalidatedAddress: UnvalidatedAddress)
        : Result<Address, RemoteServiceError> =

        let serviceInfo =
            { Name = "Address Validation Service"
              Endpoint = Uri("https://address-validation-service.com/api/validate") }

        try
            (checkAddressExists unvalidatedAddress) |> toAddress |> Ok
        with :? TimeoutException as ex ->
            Error
                { ServiceInfo = serviceInfo
                  Exception = ex }


    let validateOrder: ValidateOrder =
        fun checkAddressExists checkProductExists unvalidatedOrder ->
            result {
                let orderId = unvalidatedOrder.OrderId |> OrderId.create

                let! orderLines =
                    unvalidatedOrder.OrderLines
                    |> validateOrderLines orderId checkProductExists
                    |> Result.mapError PlaceOrderError.ValidationError

                let! customerInfoResult =
                    unvalidatedOrder.CustomerInfo
                    |> toCustomerInfo
                    |> Result.mapError PlaceOrderError.ValidationError

                let! shippingAddressResult =
                    unvalidatedOrder.ShippingAddress
                    |> checkAddressExistsAdapted checkAddressExists
                    |> Result.mapError PlaceOrderError.RemoteServiceError

                return
                    { OrderId = orderId
                      CustomerInfo = customerInfoResult
                      ShippingAddress = shippingAddressResult
                      OrderLines = orderLines }
            }

    let pricedOrderLine (getProductPrice: GetProductPrice) (orderLine: ValidatedOrderLine) : PricedOrderLine =
        let productPrice = orderLine.ProductId |> getProductPrice
        let quantity = orderLine.Quantity |> UnitQuantity.value
        let linePrice = Price.multiplyBy quantity productPrice

        { Id = orderLine.Id
          ProductId = orderLine.ProductId
          OrderId = orderLine.OrderId.Value
          Quantity = orderLine.Quantity
          LinePrice = linePrice }

    let priceOrder: PriceOrder =
        fun getProductPrice validatedOrder ->
            let pricedOrderLines =
                validatedOrder.OrderLines |> List.map (pricedOrderLine getProductPrice)

            let amountToBill =
                pricedOrderLines |> List.map (fun x -> x.LinePrice) |> BillingAmount.sumPrices

            { OrderId = validatedOrder.OrderId
              CustomerInfo = validatedOrder.CustomerInfo
              BillingAddress = validatedOrder.ShippingAddress
              OrderLines = pricedOrderLines
              AmountToBill = amountToBill }
            |> Ok

    let acknowledgeOrder: AcknowledgeOrder =
        fun createOrderAcknowledgementMessage sendOrderAcknowledgement pricedOrder ->
            let letter = createOrderAcknowledgementMessage pricedOrder

            let acknowledgement =
                { EmailAddress = pricedOrder.CustomerInfo.EmailAddress
                  Message = letter }

            let sendAcknowledgementResult = sendOrderAcknowledgement acknowledgement

            match sendAcknowledgementResult with
            | Sent ->
                let event =
                    { OrderId = pricedOrder.OrderId
                      EmailAddress = pricedOrder.CustomerInfo.EmailAddress }

                Some event
            | NotSent -> None

    let createBillableOrderPlacedEvent: CreateBillableOrderPlacedEvent =
        fun pricedOrder ->
            let billingAmount = pricedOrder.AmountToBill |> BillingAmount.value

            if billingAmount > 0M then
                let orderPlaced =
                    { OrderId = pricedOrder.OrderId
                      AmountToBill = pricedOrder.AmountToBill
                      BillingAddress = pricedOrder.BillingAddress }

                Some orderPlaced
            else
                None

    let createPlaceOrderEvents: CreatePlaceOrderEvents =
        fun pricedOrder acknowledgementEvent ->
            let asOptionalSingleton opt =
                match opt with
                | Some x -> [ x ]
                | None -> []

            let orderPlacedEvents = pricedOrder |> PlaceOrderEvent.OrderPlaced |> List.singleton

            let acknowledgementSentEvents =
                acknowledgementEvent
                |> Option.map PlaceOrderEvent.AcknowledgmentSent
                |> asOptionalSingleton

            let billableOrderPlacedEvents =
                pricedOrder
                |> createBillableOrderPlacedEvent
                |> Option.map PlaceOrderEvent.BillableOrderPlaced
                |> asOptionalSingleton

            [ yield! orderPlacedEvents
              yield! acknowledgementSentEvents
              yield! billableOrderPlacedEvents ]


    let placeOrder
        checkAddressExists
        checkProductExists
        getProductPrice
        createAcknowledgementMessage
        sendOrderAcknowledgement
        : PlaceOrderWorkflow =
        fun placeOrderCommand ->

            let unValidatedOrder = placeOrderCommand.Data

            let validate unvalidatedOrder =
                unvalidatedOrder
                |> Result.bind (validateOrder checkAddressExists checkProductExists)

            let price validatedOrder =
                validatedOrder
                |> Result.bind (priceOrder getProductPrice)
                |> Result.mapError PlaceOrderError.PricingError

            let placeOrder input =
                input
                |> validate
                |> Result.bind (fun order -> price (Ok order))
                |> Result.map (fun order ->
                    let event =
                        acknowledgeOrder createAcknowledgementMessage sendOrderAcknowledgement order

                    createPlaceOrderEvents order event)

            let orderPlaced =
                unValidatedOrder
                |> Ok
                |> placeOrder
                |> Result.mapError (function
                    | ValidationError e -> PlaceOrderError.ValidationError e
                    | PricingError e -> PlaceOrderError.PricingError e
                    | RemoteServiceError e -> PlaceOrderError.RemoteServiceError e)

            orderPlaced
