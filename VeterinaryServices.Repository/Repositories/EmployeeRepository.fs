namespace VeterinaryServices.Repository.Repositories

open System.Threading.Tasks
open MongoDB.Driver
open VeterinaryServices.Domain.Models
open VeterinaryServices.Domain.Constants

type EmployeeRepository(client: IMongoClient) =

    member private __.collection = client.GetDatabase(MongoConfig.EmployeeDB).GetCollection<Employee>("profiles")

    interface IEmployeeRepository with

        member __.CountAsync(filter: FilterDefinition<Employee>): Task<int64> = __.collection.CountDocumentsAsync filter
