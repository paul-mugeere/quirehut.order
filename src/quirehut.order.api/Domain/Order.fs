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
type OrderLineId = private OrderId of string
module OrderLineId =
    open System
    let value orderLineId:OrderLineId = orderLineId
    
    let create orderLineId =
        if String.IsNullOrEmpty orderLineId
            then failwith "Order line Id must not be null or empty"
        else
            OrderId orderLineId


type UnvalidatedOrderLine =
    { Id: string
      ProductId : string
      Quantity: int }

type UnvalidatedOrder =
    { OrderId: string
      CustomerInfo: UnvalidatedCustomerInfo
      ShippingAddress: UnvalidatedAddress
      OrderLines: UnvalidatedOrderLine list }

type ValidatedOrderLine =
    { Id: OrderLineId
      ProductId: ProductId
      OrderId: OrderId option
      Quantity: UnitQuantity }

type ValidatedOrder =
    { OrderId: OrderId
      CustomerInfo: CustomerInfo
      ShippingAddress: Address
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


