module GiraffeDemo.Program

open System

open Saturn
open Giraffe.Core
open Giraffe.ResponseWriters

open Giraffe
open GiraffeDemo.Invoice
open GiraffeDemo.Database
 
open Microsoft.Extensions.DependencyInjection

open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.Builder

open GiraffeDemo.Startup

let topRouter = scope {
    not_found_handler (setStatusCode 404 >=> text "Not Found")
    forward "/invoices" invoiceController
}



[<EntryPoint>]
let main _argv =    
    WebHostBuilder()
        .UseKestrel()
        .UseStartup<Startup>()
        .Build()
        .Run()
    0



