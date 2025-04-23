namespace quirehut.order.domain

open System
open System.Collections.Generic
open Microsoft.FSharp.Collections
open quirehut.order.domain
open quirehut.order.domain.Price
open quirehut.order.domain.common

module PlaceOrder =

    let toCustomerInfo (unvalidatedCustomerInfo: UnvalidatedCustomerInfo) : CustomerInfo =
        let fullname =
            Fullname.create unvalidatedCustomerInfo.Firstname unvalidatedCustomerInfo.Lastname

        let email = unvalidatedCustomerInfo.EmailAddress |> EmailAddress.create

        { Fullname = fullname
          EmailAddress = email }

    let toAddress (unvalidatedAddress: CheckedAddress) : Address =
        let (CheckedAddress checkedAddress) = unvalidatedAddress

        let address =
            Address.create
                checkedAddress.AddressLine1
                checkedAddress.AddressLine2
                checkedAddress.City
                checkedAddress.PostalCode

        address

    let toProductId checkProductExists productId =
        let error = "Product of Id{productId} does not exist"
        let checkProduct = ThrowErrorIf error checkProductExists productId
        checkProduct |> ProductId.create

    let toValidatedOrderLine checkProductExists (unvalidatedOrderLine: UnvalidatedOrderLine) : ValidatedOrderLine =
        let orderLineId = unvalidatedOrderLine.Id |> OrderLineId.create
        let productId = unvalidatedOrderLine.ProductId |> toProductId checkProductExists
        let quantity = unvalidatedOrderLine.Quantity |> UnitQuantity.create

        { Id = orderLineId
          ProductId = productId
          OrderId = option.None
          Quantity = quantity }

    let validateOrder: ValidateOrder =
        fun checkAddressExists checkProductExists unvalidatedOrder ->
            let orderId = unvalidatedOrder.OrderId |> OrderId.create
            let customerInfo = unvalidatedOrder.CustomerInfo |> toCustomerInfo

            let shippingAddress =
                unvalidatedOrder.ShippingAddress |> checkAddressExists |> toAddress

            let orderLines =
                unvalidatedOrder.OrderLines
                |> List.map (toValidatedOrderLine checkProductExists)

            { OrderId = orderId
              CustomerInfo = customerInfo
              ShippingAddress = shippingAddress
              OrderLines = orderLines }


    let toPricedOrderLine (getProductPrice: GetProductPrice) (orderLine: ValidatedOrderLine) : PricedOrderLine =
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
                validatedOrder.OrderLines |> List.map (toPricedOrderLine getProductPrice)

            let amountToBill =
                pricedOrderLines |> List.map (_.LinePrice) |> BillingAmount.sumPrices

            { OrderId = validatedOrder.OrderId
              CustomerInfo = validatedOrder.CustomerInfo
              BillingAddress = validatedOrder.ShippingAddress
              OrderLines = pricedOrderLines
              AmountToBill = amountToBill }

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

    let createBillable: CreateBillingEvent =
        fun pricedOrder ->
            let billingAmount = pricedOrder.AmountToBill |> BillingAmount.value

            if billingAmount > 0M then
                let order =
                    { OrderId = pricedOrder.OrderId
                      AmountToBill = pricedOrder.AmountToBill
                      BillingAddress = pricedOrder.BillingAddress }

                Some order
            else
                None
