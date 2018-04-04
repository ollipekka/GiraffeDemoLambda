module GiraffeDemo.Invoices

open System

open Amazon.DynamoDBv2;
open Amazon.DynamoDBv2.DocumentModel;
open Amazon.DynamoDBv2.DataModel;


open Giraffe.ResponseWriters
open Giraffe
open Saturn
open Microsoft.AspNetCore.Http
open System.Threading

type InvoiceParam = {
    Sum: decimal
    Supplier: string
}


[<CLIMutable>]
[<DynamoDBTable("InvoiceTable")>]
type Invoice = {
    [<DynamoDBHashKey>]
    Id: Guid
    [<DynamoDBRangeKey>]
    Date: DateTime
    Sum: decimal
    Supplier: string
}

type HttpContext with
    member this.GetDynamoContext () =
        let dynamo = this.GetService<IAmazonDynamoDB>()
        new DynamoDBContext(dynamo)

let private listInvoices (ctx: HttpContext) = task {

    use context = ctx.GetDynamoContext()
    let rollingWeek = DateTime.Now - TimeSpan.FromDays(7.0);

    let! resultSet = context
                        .ScanAsync<Invoice>([ ScanCondition("Date", ScanOperator.GreaterThan, rollingWeek) ])
                        .GetRemainingAsync()

    return! resultSet |> Controller.json ctx 
}

let private addInvoice (ctx: HttpContext) = task { 
    use context = ctx.GetDynamoContext()
    let! model = Controller.getModel<InvoiceParam> ctx

    let invoice = {
        Id = Guid.NewGuid()
        Date = DateTime.Now
        Supplier = model.Supplier
        Sum = model.Sum

    }
    
    context.SaveAsync<Invoice>(invoice, CancellationToken.None) |> Async.AwaitTask |> ignore
    return! invoice.Id |> Controller.json ctx 
}

let invoiceController = controller {
    not_found_handler (text "Invoices 404")
    index listInvoices
    add(fun ctx -> "add" |> Controller.text ctx )
    create addInvoice
    show (fun (ctx, id) -> (sprintf "Show handler - %s" id) |> Controller.text ctx)
    edit (fun (ctx, id) -> (sprintf "Edit handler - %s" id) |> Controller.text ctx)
} 
