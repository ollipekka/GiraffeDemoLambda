module GiraffeDemo.Invoice

open System
open Giraffe.ResponseWriters
open Giraffe

open GiraffeDemo.Database

open Saturn

[<CLIMutable>]
type Invoice = {
    Id: Guid
    Sum: decimal
    Supplier: string
}

let listInvoices (db: IConnectToDatabase) =
    let ctx = db.GetDataContext ()
    ctx.Dbo.Invoices 
        |> Seq.map(fun i -> {Id = i.Id; Sum = i.Sum; Supplier = i.Supplier} )
        |> Seq.toArray

let addInvoice (db: IConnectToDatabase) (param: Invoice) = 
    let ctx = db.GetDataContextDisableTransactions()
    let invoice = ctx.Dbo.Invoices.Create()
    invoice.Id <- Guid.NewGuid()
    invoice.Sum <- param.Sum
    invoice.Supplier <- param.Supplier
    ctx.SubmitUpdates()
    let invoiceId = invoice.Id
    invoiceId
    
 
let invoiceController = controller {
    
    not_found_handler (text "Invoices 404")
    index(fun ctx ->
        // Getting injected dependency.
        let db = ctx.GetService<IConnectToDatabase>() 
        listInvoices db |> Controller.json ctx )
    add(fun ctx -> "add" |> Controller.text ctx )
    create(fun ctx -> task {
        let! input = Controller.getModel<Invoice> ctx
        
        // Getting injected dependency.
        let db = ctx.GetService<IConnectToDatabase>()
        let invoiceId = addInvoice db input
        return! Controller.json ctx invoiceId
    })
    show (fun (ctx, id) -> (sprintf "Show handler - %s" id) |> Controller.text ctx)
    edit (fun (ctx, id) -> (sprintf "Edit handler - %s" id) |> Controller.text ctx)
} 
