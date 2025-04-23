namespace quirehut.order.domain

type UnitQuantity = private UnitQuantity of int

module UnitQuantity =
        let value (UnitQuantity quantity) = quantity
            
        let create quantity =
            if quantity < 1
                then failwith "UnitQuantity can not be negative"
            else if quantity > 100
                then failwith "UnitQuantity can not be more than 100"
            else (UnitQuantity quantity)   
          