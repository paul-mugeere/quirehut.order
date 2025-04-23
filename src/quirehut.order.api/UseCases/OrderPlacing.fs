namespace quirehut.order.domain

open System
open System.Collections.Generic
open quirehut.order.domain
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
        let checkProduct = 
            ThrowErrorIf error checkProductExists productId
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
