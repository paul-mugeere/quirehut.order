namespace quirehut.order.domain

module Order =
    
    type BillingAmount = Undefined

    type OrderLine =
        { Id: OrderLineId
          OrderId: OrderId
          Quantity: UnitQuantity
          Price: price }
    type UnvalidatedOrder =
        { OrderId: OrderId
          CustomerInfo: UnvalidatedCustomerInfo
          ShippingAddress: UnvalidatedShippingAddress
          OrderLines: OrderLine list}
    type ValidatedOrder =
        { OrderId: OrderId
          CustomerInfo: CustomerInfo
          ShippingAddress: ShippingAddress
          OrderLines: OrderLine list}
    type PricedOrder =
        { OrderId: OrderId
          CustomerId: CustomerId
          ShippingAddress: ShippingAddress
          BillingAddress: BillingAddress
          OrderLines: OrderLine list
          AmountToBill: BillingAmount }
    type Order =
        | Unvalidated of UnvalidatedOrder
        | Validated
        | Priced

