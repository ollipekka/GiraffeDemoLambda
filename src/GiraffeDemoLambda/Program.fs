module GiraffeDemo.Program

open Microsoft.AspNetCore.Hosting

open GiraffeDemo.Startup


[<EntryPoint>]
let main _argv =    
    WebHostBuilder()
        .UseKestrel()
        .UseStartup<Startup>()
        .Build()
        .Run()
    0



