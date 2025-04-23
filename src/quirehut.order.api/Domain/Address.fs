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
    open common
    let create addressLine1 addressLine2 city postalCode =
        let requiredAddressLine1 = ThrowErrorIf "Address Line 1 is cannot be null" String.IsNullOrEmpty addressLine1
        let requiredCity = ThrowErrorIf "City is cannot be null" String.IsNullOrEmpty city
        let requiredPostalCode = ThrowErrorIf "Postalcode is cannot be null" String.IsNullOrEmpty postalCode
        
        let address={
            AddressLine1 = requiredAddressLine1
            AddressLine2 = addressLine2 
            City = requiredCity
            PostalCode = requiredPostalCode
        }
        address
