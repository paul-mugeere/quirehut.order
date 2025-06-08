module Tests

open System.Net.Http
open System.Text.Json
open System.Threading.Tasks
open Json.Schema
open Microsoft.FSharp.Core
open Xunit
open quirehut.order.api
open quirehut.order.api.Program
open Microsoft.AspNetCore.TestHost


let placeOrderRequest: PlaceOrderRequest =
    { customerInfo =
        { Fullname = "John Doe"
          EmailAddress = "john.doe@example.com" }
      BillingAddress = {
              AddressLine1 = "123 Billing St"
              AddressLine2 = Some "Suite 100"
              City = "Billing City"
              PostalCode = "12345" }
      ShippingAddress ={
              AddressLine1 = "456 Shipping Ave"
              AddressLine2 = None
              City = "Shipping City"
              PostalCode = "67890" }
      OrderLines =
        [ { ProductId = "PROD-001"; Quantity = 2 }
          { ProductId = "PROD-002"; Quantity = 1 } ] }

let serializeToJson content =
    JsonSerializer.Serialize(content, JsonSerializerOptions(PropertyNamingPolicy = null))

let sendRequest (client: HttpClient) (method: string) (url: string) (content: HttpContent option) =
    task {
        let! response = 
            match method, content with
            | "GET", None -> client.GetAsync(url)
            | "POST", Some content -> client.PostAsync(url, content)
            | _ -> failwith "Unsupported HTTP method or content combination"
        response.EnsureSuccessStatusCode() |> ignore
        return! response.Content.ReadAsStringAsync()
    }


let runWithTestClient (runTest: HttpClient -> Task<unit>) =
    task {
        let builder = createWebApplicationBuilder [||]
        builder.WebHost.UseTestServer() |> ignore
        let app = createWebApplication builder
        do! app.StartAsync()

        let testClient = app.GetTestClient()
        do! runTest testClient
    }

let validateJsonSchema (content: string) (schemaPath: string) =
    let schema = JsonSchema.FromFile schemaPath
    let contentAsJson = content |> JsonDocument.Parse

    let isValid =
        contentAsJson.RootElement
        |> fun (x) -> schema.Evaluate(x, EvaluationOptions(RequireFormatValidation = true))
        |> _.IsValid

    isValid



[<Fact>]
let ``Given customer id, getOrders should return a customer order response with a valid schema`` () =
    let getCustomerOrder (client: HttpClient) =
        task {
            let schemaPath = "docs/customerOrder.schema.json"
            let! content = sendRequest client "GET" "api/orders/test-customer-id" None
            let isValidResponse =  validateJsonSchema content schemaPath
            Assert.True(isValidResponse, "Response does not match the expected schema")
        }

    runWithTestClient getCustomerOrder |> Async.AwaitTask


[<Fact>]
let ``Given placeOrder request, placeOrder should return an orderConfirmation response with valid schema`` () =
    let placeOrder (client: HttpClient) =
        task {
            let json = serializeToJson placeOrderRequest
            use content = new StringContent(json, System.Text.Encoding.UTF8, "application/json")
            let! responseContent = sendRequest client "POST" "api/orders" (Some content)
            let schemaPath = "docs/orderConfirmation.schema.json"
            let isValidResponse = validateJsonSchema responseContent schemaPath
            Assert.True(isValidResponse, "Response does not match the expected schema")
        }

    runWithTestClient placeOrder |> Async.AwaitTask
