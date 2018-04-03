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

let topRouter = scope {
    not_found_handler (setStatusCode 404 >=> text "Not Found")
    forward "/invoices" invoiceController
}


let configureApp (app : IApplicationBuilder) =
    app.UseGiraffe topRouter

let configureServices(services: IServiceCollection) =
    services.AddGiraffe() |> ignore
    services.ConfigureDatabase() |> ignore

[<EntryPoint>]
let main _argv =    
    WebHostBuilder()
        .UseKestrel()
        .Configure(Action<IApplicationBuilder> configureApp)
        .ConfigureServices(Action<IServiceCollection> configureServices)
        .Build()
        .Run()
    0



