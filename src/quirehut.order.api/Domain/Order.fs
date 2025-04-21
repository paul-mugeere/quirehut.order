namespace quirehut.order.domain

type OrderId = private OrderId of string
module OrderId =
    open System
    let value orderId:OrderId = orderId
    
    let create orderId =
        if String.IsNullOrEmpty orderId
            then failwith "Order Id must not be null or empty"
        else
            OrderId orderId


type UnvalidatedOrderLine =
    { Id: OrderLineId
      Quantity: UnitQuantity }

type UnvalidatedOrder =
    { OrderId: string
      CustomerInfo: UnvalidatedCustomerInfo
      ShippingAddress: UnvalidatedAddress
      OrderLines: UnvalidatedOrderLine list }

type ValidatedOrderLine =
    { Id: OrderLineId
      OrderId: OrderId
      Quantity: UnitQuantity }

type ValidatedOrder =
    { OrderId: OrderId
      CustomerInfo: CustomerInfo
      ShippingAddress: Address
      BillingAddress: Address
      OrderLines: ValidatedOrderLine list }

type PricedOrderLine =
    { Id: OrderLineId
      OrderId: OrderId
      Quantity: UnitQuantity
      Price: Price }

type PricedOrder =
    { OrderId: OrderId
      CustomerId: CustomerId
      ShippingAddress: Address
      BillingAddress: Address
      OrderLines: PricedOrderLine list
      AmountToBill: BillingAmount }

type Order =
    | Unvalidated of UnvalidatedOrder
    | Validated
    | Priced


