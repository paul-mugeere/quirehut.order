namespace quirehut.order.domain.events


open quirehut.order.domain

type AcknowledgmentSent = Undefined
type OrderPlaced = Undefined
type BillableOrderPlaced = Undefined

type PlaceOrderEvents =
    { AcknowledgmentSent: AcknowledgmentSent
      OrderPlaced: OrderPlaced
      BillableOrderPlaced: BillableOrderPlaced }



