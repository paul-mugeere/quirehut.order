namespace quirehut.order.domain

open System

type Command<'data> = {
    Userid: string
    TimeStamp: DateTime
    Data: 'data
}

type PlaceOrderCommand = Command<UnvalidatedOrder>


