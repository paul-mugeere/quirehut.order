namespace quirehut.order.domain

open quirehut.order.application.commands

type AddressValidationError = AddressValidationError of string
type CheckedAddress = CheckedAddress of UnvalidatedAddress

type CheckProductExists = ProductId -> bool
type CheckAddressExists = UnvalidatedAddress -> Result<CheckedAddress, AddressValidationError>

type ValidateOrder = CheckAddressExists -> CheckProductExists -> UnvalidatedOrder -> Result<ValidatedOrder, ValidationError>


type GetProductPrice = ProductId -> Price
type PriceOrder = GetProductPrice -> ValidatedOrder -> PricedOrder

type HtmlString = HtmlString of string

type OrderAcknowledgement = {
    EmailAddress: EmailAddress
    Message: HtmlString
}

type CreateOrderAcknowledgementMessage = PricedOrder -> HtmlString

type SendResult = Sent | NotSent

type SendOrderAcknowledgement = OrderAcknowledgement -> SendResult

type AcknowledgeOrder = CreateOrderAcknowledgementMessage -> SendOrderAcknowledgement -> PricedOrder -> OrderAcknowledgementSent option

type CreateEvent =  PricedOrder -> PlaceOrderEvents list
