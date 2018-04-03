namespace GiraffeDemo

open Giraffe

open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open GiraffeDemo

module Startup =
    type Startup (configuration: IConfiguration) =
        member _this.Configuration = configuration

        member _this.Configure (app : IApplicationBuilder) =
            app.UseGiraffe App.WebApp

        member _this.ConfigureServices (services: IServiceCollection) =
            services.AddGiraffe() |> ignore

open Startup
type public LambdaEntryPoint () =
    inherit Amazon.Lambda.AspNetCoreServer.APIGatewayProxyFunction()

    override _this.Init (builder: IWebHostBuilder) =
        builder.UseStartup<Startup>() |> ignore
