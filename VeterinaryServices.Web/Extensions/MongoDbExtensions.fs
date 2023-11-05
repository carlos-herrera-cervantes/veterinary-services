namespace VeterinaryServices.Web.Extensions

open System
open Microsoft.Extensions.DependencyInjection
open MongoDB.Driver

module MongoDbExtensions =

    type IServiceCollection with

        member __.AddMongoDbClient() =
            let connectionString = Environment.GetEnvironmentVariable("MONGO_DB_URI")
            let client = MongoClient(connectionString)
            __.AddSingleton<IMongoClient>(fun _ -> client :> IMongoClient) |> ignore
            __
