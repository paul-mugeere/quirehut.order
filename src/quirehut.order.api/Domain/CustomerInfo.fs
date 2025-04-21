namespace quirehut.order.domain

open System

type CustomerId = Undefined
type Fullname = { Firstname: string; Lastname: string }
module Fullname =
    let value fullname : Fullname = fullname
    let private ThrowErrorIfNullOrEmpty name =
        if String.IsNullOrEmpty name
            then failwith "The name value cannot be null or empty"
        name
    let create firstname lastname =
        let fullname =
                { Firstname = firstname |> ThrowErrorIfNullOrEmpty
                  Lastname = lastname |> ThrowErrorIfNullOrEmpty
                }
        fullname
  
type UnvalidatedCustomerInfo =
    { Firstname: string
      Lastname: string
      EmailAddress: string
    }

type EmailAddress = EmailAddress of string
type CustomerInfo = {
    Fullname: Fullname
    EmailAddress: EmailAddress
}
