module GiraffeDemo.App

open Saturn
open Giraffe

open GiraffeDemo.Invoices
let WebApp = scope {
    not_found_handler (setStatusCode 404 >=> text "Not Found")
    forward "/invoices" invoiceController
}
