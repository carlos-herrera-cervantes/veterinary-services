namespace VeterinaryServices.Repository.Managers

open System
open System.Threading.Tasks
open MongoDB.Driver
open VeterinaryServices.Domain.Models
open VeterinaryServices.Domain.Constants

type ServiceCatalogManager(client: IMongoClient) =

    member private __.collection = client.GetDatabase(MongoConfig.DefaultDB).GetCollection("service_catalogs")

    interface IServiceCatalogManager with

        member __.CreateAsync(serviceCatalog: ServiceCatalog): Task =
            __.collection.InsertOneAsync serviceCatalog

        member __.UpdateAsync(filter: FilterDefinition<ServiceCatalog>, serviceCatalog: ServiceCatalog): Task =
            serviceCatalog.UpdatedAt <- DateTime.UtcNow
            __.collection.ReplaceOneAsync(filter, serviceCatalog) |> ignore
            Task.CompletedTask

        member __.DeleteAsync(filter: FilterDefinition<ServiceCatalog>): Task =
            __.collection.DeleteOneAsync filter |> ignore
            Task.CompletedTask
