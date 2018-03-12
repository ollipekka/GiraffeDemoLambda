module GiraffeDemo.Invoice

open System
open Giraffe.ResponseWriters
open Giraffe
open FSharp.Data.Sql.Transactions

open GiraffeDemo.Database

open Saturn

[<CLIMutable>]
type Invoice = {
    Id: Guid
    Sum: decimal
    Supplier: string
}

let listInvoices () =
    let ctx = sql.GetDataContext()
    ctx.Dbo.Invoices 
        |> Seq.map(fun i -> {Id = i.Id; Sum = i.Sum; Supplier = i.Supplier} )
        |> Seq.toArray

let addInvoice (param: Invoice) =
    // Wow, just wow. https://github.com/dotnet/corefx/issues/24282 needs framework update.
    let transactionOptions = {IsolationLevel = IsolationLevel.DontCreateTransaction; Timeout = TimeSpan.FromSeconds(1.0)}
    let ctx = sql.GetDataContext(transactionOptions)
    let invoice = ctx.Dbo.Invoices.Create()
    invoice.Id <- Guid.NewGuid()
    invoice.Sum <- param.Sum
    invoice.Supplier <- param.Supplier
    ctx.SubmitUpdates()
    let invoiceId = invoice.Id
    invoiceId
    
 
let invoiceController = controller {
    
    not_found_handler (text "Invoices 404")
    index(fun ctx -> listInvoices() |> Controller.json ctx )
    add(fun ctx -> "add" |> Controller.text ctx )
    create(fun ctx -> task {
        let! input = Controller.getModel<Invoice> ctx
        let invoiceId = addInvoice input
        return! Controller.json ctx invoiceId
    })
    
    show (fun (ctx, id) -> (sprintf "Show handler - %s" id) |> Controller.text ctx)
    edit (fun (ctx, id) -> (sprintf "Edit handler - %s" id) |> Controller.text ctx)
} 
