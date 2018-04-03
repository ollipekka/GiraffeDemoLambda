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

let listInvoices () =
    "List Invoices"

let addInvoice (param: Invoice) = 
    printfn "%A" param
    
 
let invoiceController = controller {
    
    not_found_handler (text "Invoices 404")
    index(fun ctx ->  listInvoices () |> Controller.json ctx )
    add(fun ctx -> "add" |> Controller.text ctx )
    create(fun ctx -> task {
        let! input = Controller.getModel<Invoice> ctx
        
        let invoiceId = addInvoice input
        return! Controller.json ctx invoiceId
    })
    show (fun (ctx, id) -> (sprintf "Show handler - %s" id) |> Controller.text ctx)
    edit (fun (ctx, id) -> (sprintf "Edit handler - %s" id) |> Controller.text ctx)
} 
