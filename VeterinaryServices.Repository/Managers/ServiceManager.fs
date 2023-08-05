namespace VeterinaryServices.Repository.Managers

open System
open System.Threading.Tasks
open MongoDB.Driver
open VeterinaryServices.Domain.Models
open VeterinaryServices.Domain.Constants

type ServiceManager(client: IMongoClient) =

    member private __.collection = client.GetDatabase(MongoConfig.DefaultDB).GetCollection<Service>("services")

    interface IServiceManager with

        member __.CreateAsync(service: Service): Task = __.collection.InsertOneAsync service

        member __.UpdateAsync(filter: FilterDefinition<Service>, service: Service): Task =
            service.UpdatedAt <- DateTime.UtcNow
            __.collection.ReplaceOneAsync(filter, service) |> ignore
            Task.CompletedTask

        member __.DeleteAsync(filter: FilterDefinition<Service>): Task =
            __.collection.DeleteOneAsync filter |> ignore
            Task.CompletedTask
