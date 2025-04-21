namespace quirehut.order.domain

type OrderPlaced = Undefined
type BillableOrderPlaced = Undefined
type OrderAcknowledgementSent ={
    OrderId: OrderId
    EmailAddress: EmailAddress
}

type PlaceOrderEvent =
    | AcknowledgmentSent of OrderAcknowledgementSent
    | OrderPlaced of OrderPlaced
    | BillableOrderPlaced of BillableOrderPlaced
    
    
    




