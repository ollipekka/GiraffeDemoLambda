module GiraffeDemo.Startup

open Giraffe
open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open GiraffeDemo

type Startup (configuration: IConfiguration) =
    member _this.Configuration = configuration

    member _this.Configure (app : IApplicationBuilder) =
        app.UseGiraffe App.WebApp

    member _this.ConfigureServices (services: IServiceCollection) =
        services.AddGiraffe() |> ignore
