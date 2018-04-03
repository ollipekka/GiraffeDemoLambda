module GiraffeDemo.Database

open System
open FSharp.Data.Sql
open Microsoft.Extensions.DependencyInjection

open FSharp.Data.Sql.Transactions

[<Literal>] 
let DBVendor = Common.DatabaseProviderTypes.MSSQLSERVER

[<Literal>] 
let ConnectionString = @"Server=localhost;Database=GiraffeDemo;User Id=demo;Password=demo"
type private SQLDatabase = SqlDataProvider<DBVendor, ConnectionString>

type IConnectToDatabase =
    abstract member GetDataContext: unit -> SQLDatabase.dataContext
    abstract member GetDataContextDisableTransactions: unit -> SQLDatabase.dataContext


type private ConnectToDatabase () =
    interface IConnectToDatabase with
        member _this.GetDataContext () = SQLDatabase.GetDataContext (connectionString = ConnectionString)
           
        member _this.GetDataContextDisableTransactions ()  = 
            let transactionOptions = {IsolationLevel = IsolationLevel.DontCreateTransaction; Timeout = TimeSpan.FromSeconds(1.0)}
            SQLDatabase.GetDataContext (connectionString = ConnectionString, transactionOptions = transactionOptions)
type IServiceCollection with
    member this.ConfigureDatabase() = 
        this.AddSingleton<IConnectToDatabase>(ConnectToDatabase() :> IConnectToDatabase)