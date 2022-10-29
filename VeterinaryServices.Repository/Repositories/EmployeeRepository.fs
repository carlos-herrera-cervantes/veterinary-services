namespace VeterinaryServices.Repository.Repositories

open System
open MongoDB.Driver
open VeterinaryServices.Domain.Models

type EmployeeRepository(client: IMongoClient) =

    let database = Environment.GetEnvironmentVariable("EMPLOYEE_DB")

    member private this._collection = client.GetDatabase(database).GetCollection<Employee>("profiles")

    interface IEmployeeRepository with

        member this.CountAsync(filter: FilterDefinition<Employee>) =
            this._collection.CountDocumentsAsync filter
