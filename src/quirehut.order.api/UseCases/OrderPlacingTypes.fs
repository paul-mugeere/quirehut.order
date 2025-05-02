namespace quirehut.order.domain

type AddressValidationError = AddressValidationError of string
type CheckedAddress = CheckedAddress of UnvalidatedAddress
type AsyncResult<'success, 'failure> = Async<Result<'success, 'failure>>


type PricingError = PricingError of string

type ValidationError =
    { FieldName: string
      ErrorDescription: string }
module ValidationError =
    
    let create fieldName errorDescription =
        { FieldName = fieldName
          ErrorDescription = errorDescription }
    let value validationError =
        validationError

type PlaceOrderError =
    | ValidationError of ValidationError
    | PricingError of PricingError

type CheckProductExists = string -> bool
type CheckAddressExists = UnvalidatedAddress -> CheckedAddress // AsyncResult<CheckedAddress, AddressValidationError>
type ValidateOrder = CheckAddressExists -> CheckProductExists -> UnvalidatedOrder -> Result<ValidatedOrder, ValidationError>

type GetProductPrice = ProductId -> Price
type PriceOrder = GetProductPrice -> ValidatedOrder -> Result<PricedOrder,PricingError>

type SendOrderAcknowledgement = OrderAcknowledgement -> SendResult // Async<SendResult>

type CreateOrderAcknowledgementMessage = PricedOrder -> HtmlString

type AcknowledgeOrder =
    CreateOrderAcknowledgementMessage -> SendOrderAcknowledgement -> PricedOrder -> OrderAcknowledgementSent option
//-> Async<OrderAcknowledgementSent option>

type CreatePlaceOrderEvents = PricedOrder -> OrderAcknowledgementSent option -> PlaceOrderEvent list
type CreateBillableOrderPlacedEvent = PricedOrder -> BillableOrderPlaced option
type PlaceOrderWorkflow = PlaceOrderCommand -> Result<PlaceOrderEvent list, PlaceOrderError>

