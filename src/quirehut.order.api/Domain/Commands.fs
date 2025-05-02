namespace quirehut.order.domain

open System

type Command<'data> = {
    Userid: string
    TimeStamp: DateTime
    Data: 'data
}


type PlaceOrderError =
    | ValidationError of ValidationError list

and ValidationError =
    { FieldName: string
      ErrorDescription: string }

type PlaceOrderCommand = Command<UnvalidatedOrder>


