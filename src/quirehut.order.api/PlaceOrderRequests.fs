namespace quirehut.order.api

type OrderLineInfo = {
    ProductId: string
    Quantity: int
}

type AddressInfo = {
    AddressLine1: string
    AddressLine2: string option
    City: string
    PostalCode: string
}

type CustomerInfo = {
    Fullname: string
    EmailAddress: string
}

type PlaceOrderRequest ={
    customerInfo: CustomerInfo
    BillingAddress: AddressInfo
    ShippingAddress: AddressInfo
    OrderLines: OrderLineInfo list
}

type OrderConfirmation = {
    OrderId: string
    Message: string
}

type CustomerOrderLine = {
    ProductId: string
    ProductName: string
    Quantity: int
}

type CustomerOrder = {
    OrderId: string
    CustomerId: string
    OrderDate: System.DateTime
    ShippingAddress: AddressInfo
    OrderLines: CustomerOrderLine list
}
