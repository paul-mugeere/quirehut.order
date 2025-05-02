namespace quirehut.order.domain

open System

type UnitQuantity = private UnitQuantity of int

module UnitQuantity =
        let value (UnitQuantity quantity) = quantity
            
        let create quantity =
            if quantity < 1
                then failwith "UnitQuantity can not be negative"
            else if quantity > 100
                then failwith "UnitQuantity can not be more than 100"
            else (UnitQuantity quantity) 
            
type Price = private Price of decimal
module Price =
    let value (Price price) = price
    let create price =
        Price price
    let multiplyBy qty  (Price p) = create (qty * p)

type BillingAmount = private BillingAmount of decimal
module BillingAmount =
    let value (BillingAmount amount) = amount
    let create amount = BillingAmount amount
    let sumPrices prices =
        let total = prices |> List.map Price.value |> List.sum
        create total
    
type ProductId = private ProductId of string
module ProductId =
    let value  productId:ProductId = productId
    let create productId =
        if String.IsNullOrEmpty(productId)
            then failwith "Product Id cannot be null"
        ProductId productId

type OrderId = private OrderId of string
module OrderId =
    let value orderId:OrderId = orderId
    
    let create orderId =
        if String.IsNullOrEmpty orderId
            then failwith "Order Id must not be null or empty"
        else
            OrderId orderId
type OrderLineId = private OrderLineId of string
module OrderLineId =
    let value orderLineId:OrderLineId = orderLineId
    
    let create orderLineId =
        if String.IsNullOrEmpty orderLineId
            then failwith "Order line Id must not be null or empty"
        else
            OrderLineId orderLineId

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
      ProductId: ProductId
      OrderId: OrderId
      Quantity: UnitQuantity
      LinePrice: Price }

type PricedOrder =
    { OrderId: OrderId
      CustomerInfo: CustomerInfo
      BillingAddress: Address
      OrderLines: PricedOrderLine list
      AmountToBill: BillingAmount }

type Order =
    | Unvalidated of UnvalidatedOrder
    | Validated
    | Priced
    

type SendResult = Sent | NotSent
type HtmlString = HtmlString of string
type OrderAcknowledgement = {
    EmailAddress: EmailAddress
    Message: HtmlString
}


