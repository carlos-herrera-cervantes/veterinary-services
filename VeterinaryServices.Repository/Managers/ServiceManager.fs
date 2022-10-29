namespace VeterinaryServices.Repository.Managers

open System
open System.Threading.Tasks
open MongoDB.Driver
open VeterinaryServices.Domain.Models

type ServiceManager(client: IMongoClient) =

    let database = Environment.GetEnvironmentVariable("DEFAULT_DB")

    member private this._collection = client.GetDatabase(database).GetCollection<Service>("services")

    interface IServiceManager with

        member this.CreateAsync(service: Service) = this._collection.InsertOneAsync service

        member this.UpdateAsync(filter: FilterDefinition<Service>, service: Service) =
            service.UpdatedAt <- DateTime.UtcNow
            this._collection.ReplaceOneAsync(filter, service) |> ignore
            Task.CompletedTask

        member this.DeleteAsync(filter: FilterDefinition<Service>) =
            this._collection.DeleteOneAsync filter |> ignore
            Task.CompletedTask
