namespace VeterinaryServices.Repository.Repositories

open System.Threading.Tasks
open MongoDB.Driver
open VeterinaryServices.Domain.Models
open VeterinaryServices.Domain.Constants

type CustomerRepository(client: IMongoClient) =

    member private __.collection = client.GetDatabase(MongoConfig.CustomerDB).GetCollection<Customer>("profiles")

    interface ICustomerRepository with

        member __.CountAsync(filter: FilterDefinition<Customer>): Task<int64> = __.collection.CountDocumentsAsync filter
