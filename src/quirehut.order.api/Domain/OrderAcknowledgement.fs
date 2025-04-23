namespace quirehut.order.domain

type SendResult = Sent | NotSent
type HtmlString = HtmlString of string
type OrderAcknowledgement = {
    EmailAddress: EmailAddress
    Message: HtmlString
}

