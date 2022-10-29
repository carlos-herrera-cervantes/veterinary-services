namespace VeterinaryServices.Repository.Repositories

open System
open MongoDB.Driver
open VeterinaryServices.Domain.Models

type CustomerRepository(client: IMongoClient) =

    let database = Environment.GetEnvironmentVariable("CUSTOMER_DB")

    member private this._collection = client.GetDatabase(database).GetCollection<Customer>("profiles")

    interface ICustomerRepository with

        member this.CountAsync(filter: FilterDefinition<Customer>) =
            this._collection.CountDocumentsAsync filter
