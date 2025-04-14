namespace quirehut.order.domain

open quirehut.order.domain

type OrderPlaced = Undefined
type BillableOrderPlaced = Undefined
type OrderAcknowledgementSent ={
    OrderId: OrderId
    EmailAddress: EmailAddress
}

type PlaceOrderEvents =
    | AcknowledgmentSent of OrderAcknowledgementSent
    | OrderPlaced of OrderPlaced
    | BillableOrderPlaced of BillableOrderPlaced
    
    
    




