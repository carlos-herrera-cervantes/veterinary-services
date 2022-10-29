namespace VeterinaryServices.Web.Extensions

open System
open Microsoft.Extensions.DependencyInjection
open MongoDB.Driver

module MongoDbExtensions =

    type IServiceCollection with

    member this.AddMongoDbClient() =
        let connectionString = Environment.GetEnvironmentVariable("MONGO_DB_URI")
        let client = MongoClient(connectionString)
        this.AddSingleton<IMongoClient>(fun _ -> client :> IMongoClient) |> ignore
        this
