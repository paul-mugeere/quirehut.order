namespace quirehut.order.domain

type UnvalidatedOrderLine =
    { Id: OrderLineId
      Quantity: UnitQuantity }

type UnvalidatedOrder =
    { OrderId: OrderId
      CustomerInfo: UnvalidatedCustomerInfo
      ShippingAddress: UnvalidatedShippingAddress
      OrderLines: UnvalidatedOrderLine list }

type ValidatedOrderLine =
    { Id: OrderLineId
      OrderId: OrderId
      Quantity: UnitQuantity }

type ValidatedOrder =
    { OrderId: OrderId
      CustomerInfo: CustomerInfo
      ShippingAddress: ShippingAddress
      BillingAddress: BillingAddress
      OrderLines: ValidatedOrderLine list }

type PricedOrderLine =
    { Id: OrderLineId
      OrderId: OrderId
      Quantity: UnitQuantity
      Price: Price }

type PricedOrder =
    { OrderId: OrderId
      CustomerId: CustomerId
      ShippingAddress: ShippingAddress
      BillingAddress: BillingAddress
      OrderLines: PricedOrderLine list
      AmountToBill: BillingAmount }

type Order =
    | Unvalidated of UnvalidatedOrder
    | Validated
    | Priced
