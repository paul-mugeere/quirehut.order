namespace quirehut.order.domain

type OrderPlaced = PricedOrder
type BillableOrderPlaced = {
    OrderId: OrderId
    BillingAddress: Address
    AmountToBill: BillingAmount
}
type OrderAcknowledgementSent ={
    OrderId: OrderId
    EmailAddress: EmailAddress
}

type PlaceOrderEvent =
    | OrderPlaced of OrderPlaced
    | BillableOrderPlaced of BillableOrderPlaced
    | AcknowledgmentSent of OrderAcknowledgementSent
    
    
    




