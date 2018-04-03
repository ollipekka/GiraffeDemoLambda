module GiraffeDemo.Serverless

open Microsoft.AspNetCore.Hosting

open GiraffeDemo.Startup

type LambdaEntryPoint () =
    inherit Amazon.Lambda.AspNetCoreServer.APIGatewayProxyFunction()

    override _this.Init (builder: IWebHostBuilder) =
        builder.UseStartup<Startup>() |> ignore