namespace quirehut.order.domain

type UnitQuantity = private UnitQuantity of int

module UnitQuantity =
        let value (UnitQuantity quantity) = quantity
            
        let create quantity =
            if quantity < 1
                then Error "UnitQuantity can not be negative"
            else if quantity > 100
                then Error "UnitQuantity can not be more than 100"
            else Ok (UnitQuantity quantity)   
          