namespace quirehut.order.domain

open System

type BillingAmount = Undefined
type Price = Undefined
type price = Undefined

type ProductId = private ProductId of string
module ProductId =
    let value  productId:ProductId = productId
    let create productId =
        if String.IsNullOrEmpty(productId)
            then failwith "Product Id cannot be null"
        ProductId productId

type HtmlString = HtmlString of string
