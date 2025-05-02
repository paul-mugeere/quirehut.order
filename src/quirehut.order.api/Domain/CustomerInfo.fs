namespace quirehut.order.domain

open System
     
type UnvalidatedAddress =
    {
       AddressLine1 : string
       AddressLine2 : string option
       City: string
       PostalCode: string
    }
      
type Address = {
       AddressLine1 : string
       AddressLine2 : string option
       City: string
       PostalCode: string
    }

module Address=
    
    let private ThrowErrorIf predicate value errorMessage =
        if predicate value
            then failwith errorMessage
        else value
    let create addressLine1 addressLine2 city postalCode =
        let requiredAddressLine1 = ThrowErrorIf (String.IsNullOrEmpty) addressLine1 "Address Line 1 is cannot be null" 
        let requiredCity = ThrowErrorIf (String.IsNullOrEmpty) city "City is cannot be null" 
        let requiredPostalCode = ThrowErrorIf (String.IsNullOrEmpty) postalCode "Postalcode is cannot be null"
        
        let address={
            AddressLine1 = requiredAddressLine1
            AddressLine2 = addressLine2 
            City = requiredCity
            PostalCode = requiredPostalCode
        }
        address

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

type EmailAddress = private EmailAddress of string
module EmailAddress =
    let value email:EmailAddress = email
    let create email =
        EmailAddress email
    
type CustomerInfo = {
    Fullname: Fullname
    EmailAddress: EmailAddress
}
