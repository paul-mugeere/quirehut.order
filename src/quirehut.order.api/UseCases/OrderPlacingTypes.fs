namespace quirehut.order.domain

type AddressValidationError = AddressValidationError of string
type CheckedAddress = CheckedAddress of UnvalidatedAddress
type AsyncResult<'success,'failure> = Async<Result<'success,'failure>>

type CheckProductExists = ProductId -> bool
type CheckAddressExists = UnvalidatedAddress -> CheckedAddress // AsyncResult<CheckedAddress, AddressValidationError>
type ValidateOrder = CheckAddressExists -> CheckProductExists -> UnvalidatedOrder -> ValidatedOrder // AsyncResult<ValidatedOrder, ValidationError list>

type GetProductPrice = ProductId -> Price
type PricingError = PricingError of string
type PriceOrder = GetProductPrice -> ValidatedOrder -> Result<PricedOrder,PricingError>

type SendResult = Sent | NotSent

type SendOrderAcknowledgement = OrderAcknowledgement -> Async<SendResult>

type CreateOrderAcknowledgementMessage = PricedOrder -> HtmlString
type AcknowledgeOrder = CreateOrderAcknowledgementMessage -> SendOrderAcknowledgement -> PricedOrder -> Async<OrderAcknowledgementSent option>

type CreateEvent =  PricedOrder -> PlaceOrderEvent list
//type PlaceOrder = PlaceOrderCommand -> Async<Result<PlaceOrderEvent list, PlaceOrderError>>
