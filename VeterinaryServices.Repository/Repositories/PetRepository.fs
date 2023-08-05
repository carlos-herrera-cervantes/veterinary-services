namespace VeterinaryServices.Repository.Repositories

open System.Threading.Tasks
open MongoDB.Driver
open VeterinaryServices.Domain.Models
open VeterinaryServices.Domain.Constants

type PetRepository(client: IMongoClient) =

    member private __.collection = client.GetDatabase(MongoConfig.PetDB).GetCollection<Pet>("profiles")

    interface IPetRepository with

        member __.CountAsync(filter: FilterDefinition<Pet>): Task<int64> = __.collection.CountDocumentsAsync filter
