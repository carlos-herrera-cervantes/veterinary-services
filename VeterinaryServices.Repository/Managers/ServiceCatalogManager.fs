namespace VeterinaryServices.Repository.Managers

open System
open System.Threading.Tasks
open MongoDB.Driver
open VeterinaryServices.Domain.Models

type ServiceCatalogManager(client: IMongoClient) =

    let database = Environment.GetEnvironmentVariable("DEFAULT_DB")

    member private this._collection = client.GetDatabase(database).GetCollection("service_catalogs")

    interface IServiceCatalogManager with

        member this.CreateAsync(serviceCatalog: ServiceCatalog) =
            this._collection.InsertOneAsync serviceCatalog

        member this.UpdateAsync(filter: FilterDefinition<ServiceCatalog>, serviceCatalog: ServiceCatalog) =
            serviceCatalog.UpdatedAt <- DateTime.UtcNow
            this._collection.ReplaceOneAsync(filter, serviceCatalog) |> ignore
            Task.CompletedTask

        member this.DeleteAsync(filter: FilterDefinition<ServiceCatalog>) =
            this._collection.DeleteOneAsync filter |> ignore
            Task.CompletedTask
