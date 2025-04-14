namespace quirehut.order.application.commands

open Microsoft.FSharp.Core
open quirehut.order.domain

type PlaceOrderError =
    | ValidationError of ValidationError list

and ValidationError =
    { FieldName: string
      ErrorDescription: string }

type PlaceOrder = Command<UnvalidatedOrder>



