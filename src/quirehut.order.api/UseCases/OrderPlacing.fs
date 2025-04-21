namespace quirehut.order.domain

open quirehut.order.domain

module PlaceOrder =
    let validateOrder: ValidateOrder =
        fun checkAddressExists checkProductExists unvalidatedOrder ->
            let orderId = unvalidatedOrder.OrderId |> OrderId.create

            { OrderId = orderId
              CustomerInfo = failwith "todo"
              ShippingAddress = failwith "todo"
              BillingAddress = failwith "todo"
              OrderLines = failwith "todo" }
