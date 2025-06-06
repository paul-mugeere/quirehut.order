module Tests

open System
open System.Net.Http
open System.Threading.Tasks
open Microsoft.FSharp.Core
open Xunit
open quirehut.order.api.Program
open Microsoft.AspNetCore.TestHost

let runWithTestClient (runTest: HttpClient ->Task<unit>) =
   task{
       let builder = createWebApplicationBuilder [||]
       builder.WebHost.UseTestServer() |> ignore
       let app = createWebApplication builder
       do! app.StartAsync()
       
       let testClient = app.GetTestClient()
       do! runTest testClient
   }


[<Fact>]
let ``Get Customer order returns a success response`` () =
    let placeOrderRequest (client: HttpClient) =
        task {
            let! response = client.GetAsync "api/orders/test-customer-id"
            response.EnsureSuccessStatusCode() |> ignore
            let! content = response.Content.ReadAsStringAsync()
            Assert.NotEmpty(content)
        }

    runWithTestClient placeOrderRequest |> Async.AwaitTask 