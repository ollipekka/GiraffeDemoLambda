module GiraffeDemo.Database

open FSharp.Data.Sql


[<Literal>] 
let DBVendor = Common.DatabaseProviderTypes.MSSQLSERVER

[<Literal>] 
let ConnectionString = @"Server=localhost;Database=GiraffeDemo;User Id=demo;Password=demo"
type sql = SqlDataProvider<DBVendor, ConnectionString>