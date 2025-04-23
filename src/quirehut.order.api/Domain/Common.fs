namespace quirehut.order.domain

type Undefined = exn

module common =
    open System
    let ThrowErrorIf errorMessage f x =
        if f x
            then failwith errorMessage
        x



