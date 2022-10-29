namespace VeterinaryServices.Repository.Repositories

open System
open MongoDB.Driver
open VeterinaryServices.Domain.Models

type PetRepository(client: IMongoClient) =

    let database = Environment.GetEnvironmentVariable("PET_DB")

    member private this._collection = client.GetDatabase(database).GetCollection<Pet>("profiles")

    interface IPetRepository with

        member this.CountAsync(filter: FilterDefinition<Pet>) =
            this._collection.CountDocumentsAsync filter
