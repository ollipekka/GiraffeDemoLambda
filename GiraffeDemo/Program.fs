module GiraffeDemo.Program

open Saturn
open Giraffe.Core
open Giraffe.ResponseWriters

 
open GiraffeDemo.Invoice



let topRouter = scope {
    not_found_handler (setStatusCode 404 >=> text "Not Found")
    forward "/invoices" invoiceController
}

let app = application {
    router topRouter
    url "http://0.0.0.0:8085/"
}


[<EntryPoint>]
let main _argv =
    run app
    0



