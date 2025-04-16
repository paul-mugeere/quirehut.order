namespace quirehut.order.application.commands

open System

type Command<'data> = {
    Userid: string
    TimeStamp: DateTime
    Data: 'data
}