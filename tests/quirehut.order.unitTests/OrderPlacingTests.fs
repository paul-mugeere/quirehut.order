module Tests

open System
open Xunit
open quirehut.order.domain
open quirehut.order.domain.PlaceOrder

[<Fact>]
let ``Given product does not exist, toValidatedOrderLine should fail`` () =

    let unvalidatedOrder: UnvalidatedOrderLine =
        { Id = "test-order-line"
          ProductId = "test-product-id"
          Quantity = 1 }

    let fakeCheckProductExists = fun _ -> false

    let validatedOrderLine =
        fun () -> validateOrderLine fakeCheckProductExists unvalidatedOrder |> ignore

    Assert.Throws<Exception>(validatedOrderLine)
