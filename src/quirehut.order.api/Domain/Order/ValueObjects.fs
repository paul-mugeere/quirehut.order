namespace quirehut.order.domain

type BillingAmount = Undefined
type Price = Undefined
type CustomerId = Undefined
type OrderLineId = Undefined
type price = Undefined
type OrderId = Undefined
type ShippingAddress = Undefined
type UnvalidatedShippingAddress = Undefined
type BillingAddress = Undefined
type EmailAddress = Undefined

type UnvalidatedAddress = UnvalidatedAddress of string
type ProductId = ProductId of string

type UnitQuantity = private UnitQuantity of int
module UnitQuantity =
        let value (UnitQuantity quantity) = quantity
            
        let create quantity =
            if quantity < 1
                then Error "UnitQuantity can not be negative"
            else if quantity > 100
                then Error "UnitQuantity can not be 0more than 100"
            else Ok (UnitQuantity quantity)   
          