namespace quirehut.order.domain.commands

open Microsoft.FSharp.Core

type PlaceOrderError =
    | ValidationError of ValidationError list

and ValidationError =
    { FieldName: string
      ErrorDescription: string }



// type PlaceOrder = UnvalidatedOrder -> Result<PlaceOrderEvents, PlaceOrderError>



