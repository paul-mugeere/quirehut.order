namespace quirehut.order.domain

type CustomerId = Undefined
type OrderLineId = Undefined
type price = Undefined
type OrderId = Undefined
type UnitQuantity = private UnitQuantity of int
type ShippingAddress = Undefined
type UnvalidatedShippingAddress = Undefined
type BillingAddress = Undefined

module UnitQuantity =
        let value (UnitQuantity quantity) = quantity
            
        let create quantity =
            if quantity < 1
                then Error "UnitQuantity can not be negative"
            else if quantity > 100
                then Error "UnitQuantity can not be 0more than 100"
            else Ok (UnitQuantity quantity)   
          