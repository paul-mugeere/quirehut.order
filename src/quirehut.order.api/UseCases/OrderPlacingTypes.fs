namespace quirehut.order.domain

type AddressValidationError = AddressValidationError of string
type CheckedAddress = CheckedAddress of UnvalidatedAddress
type AsyncResult<'success, 'failure> = Async<Result<'success, 'failure>>

type CheckProductExists = string -> bool
type CheckAddressExists = UnvalidatedAddress -> CheckedAddress // AsyncResult<CheckedAddress, AddressValidationError>
type ValidateOrder = CheckAddressExists -> CheckProductExists -> UnvalidatedOrder -> ValidatedOrder // AsyncResult<ValidatedOrder, ValidationError list>

type GetProductPrice = ProductId -> Price
type PricingError = PricingError of string
type PriceOrder = GetProductPrice -> ValidatedOrder -> PricedOrder // Result<PricedOrder,PricingError>

type SendOrderAcknowledgement = OrderAcknowledgement -> SendResult // Async<SendResult>

type CreateOrderAcknowledgementMessage = PricedOrder -> HtmlString

type AcknowledgeOrder =
    CreateOrderAcknowledgementMessage -> SendOrderAcknowledgement -> PricedOrder -> OrderAcknowledgementSent option
//-> Async<OrderAcknowledgementSent option>

type CreatePlaceOrderEvents = PricedOrder -> OrderAcknowledgementSent option -> PlaceOrderEvent list
type CreateBillableOrderPlacedEvent = PricedOrder -> BillableOrderPlaced option
type PlaceOrderWorkflow = PlaceOrderCommand -> PlaceOrderEvent list // Async<Result<PlaceOrderEvent list, PlaceOrderError>>
