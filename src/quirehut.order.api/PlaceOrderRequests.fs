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

type ShippingAddressInfo = ShippingAddressInfo of AddressInfo
type BillingAddressInfo = BillingAddressInfo of AddressInfo

type CustomerInfo = {
    Fullname: string
    EmailAddress: string
}

type PlaceOrderRequest ={
    customerInfo: CustomerInfo
    BillingAddress: BillingAddressInfo
    ShippingAddress: ShippingAddressInfo
    OrderLines: OrderLineInfo list
}

type OrderConfirmation = {
    OrderId: string
    Message: string
}