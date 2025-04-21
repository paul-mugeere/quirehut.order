namespace quirehut.order.domain

type AddressValidationError = AddressValidationError of string
type CheckedAddress = CheckedAddress of UnvalidatedAddress
type AsyncResult<'success,'failure> = Async<Result<'success,'failure>>

type CheckProductExists = ProductId -> bool
type CheckAddressExists = UnvalidatedAddress -> AsyncResult<CheckedAddress, AddressValidationError>

type ValidateOrder = CheckAddressExists -> CheckProductExists -> UnvalidatedOrder -> AsyncResult<ValidatedOrder, ValidationError list>


type GetProductPrice = ProductId -> Price
type PricingError = PricingError of string
type PriceOrder = GetProductPrice -> ValidatedOrder -> Result<PricedOrder,PricingError>

type HtmlString = HtmlString of string

type OrderAcknowledgement = {
    EmailAddress: EmailAddress
    Message: HtmlString
}

type CreateOrderAcknowledgementMessage = PricedOrder -> HtmlString

type SendResult = Sent | NotSent

type SendOrderAcknowledgement = OrderAcknowledgement -> Async<SendResult>

type AcknowledgeOrder = CreateOrderAcknowledgementMessage -> SendOrderAcknowledgement -> PricedOrder -> Async<OrderAcknowledgementSent option>

type CreateEvent =  PricedOrder -> PlaceOrderEvent list


type PlaceOrder = PlaceOrderCommand -> Async<Result<PlaceOrderEvent list, PlaceOrderError>>
