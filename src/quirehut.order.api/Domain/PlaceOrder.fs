namespace quirehut.order.domain

open Microsoft.FSharp.Core

type PlaceOrderError =
    | ValidationError of ValidationError list

and ValidationError =
    { FieldName: string
      ErrorDescription: string }

type PlaceOrderCommand = Command<UnvalidatedOrder>



