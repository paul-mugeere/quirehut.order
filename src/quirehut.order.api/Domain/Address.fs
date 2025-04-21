namespace quirehut.order.domain
      
type UnvalidatedAddress =
    {
       AddressLine1 : string
       AddressLine2 : string option
       AddressLine3 : string option
       City: string
       PostalCode: string
    }
      
type Address = {
       AddressLine1 : string
       AddressLine2 : string option
       AddressLine3 : string option
       City: string
       PostalCode: string
    }

module Address=
    let create =
        let address={
            AddressLine1 = failwith "validation ToDo"
            AddressLine2 = failwith "todo"
            AddressLine3 = failwith "todo"
            City = failwith "todo"
            PostalCode = failwith "todo"
        }
        address
