namespace VeterinaryServices.Tests.Repository.Managers

open System
open MongoDB.Driver
open Xunit
open VeterinaryServices.Repository.Repositories
open VeterinaryServices.Repository.Managers
open VeterinaryServices.Domain.Models

[<Collection(nameof(ServiceCatalogManager))>]
type ServiceCatalogManagerTests() =

    let uri = Environment.GetEnvironmentVariable("MONGO_DB_URI")

    member private this._mongoClient = MongoClient(uri)

    [<Fact(DisplayName = "Should create, update and delete a document")>]
    member this.CreateUpdateAndDeleteAsyncShouldCreateUpdateAndDeleteDocument() =
        async {
            let newCatalog = ServiceCatalog()
            newCatalog.Name <- "Test Catalog"
            newCatalog.Description <- "Test description"

            let serviceCatalogManager = ServiceCatalogManager(this._mongoClient) :> IServiceCatalogManager
            do! serviceCatalogManager.CreateAsync(newCatalog) |> Async.AwaitTask

            let serviceCatalogRepository = ServiceCatalogRepository(this._mongoClient) :> IServiceCatalogRepository
            let! catalog =
                serviceCatalogRepository.GetOneAsync(Builders<ServiceCatalog>.Filter.Eq((fun sc -> sc.Name), "Test Catalog"))
                |> Async.AwaitTask
            Assert.NotNull(catalog)

            catalog.Name <- "Update Catalog"
            do! serviceCatalogManager.UpdateAsync(Builders<ServiceCatalog>.Filter.Eq((fun sc -> sc.Id), catalog.Id), catalog)
                |> Async.AwaitTask

            let! catalogAfterUpdate =
                serviceCatalogRepository.GetOneAsync(Builders<ServiceCatalog>.Filter.Eq((fun sc -> sc.Id), catalog.Id))
                |> Async.AwaitTask
            Assert.Equal("Update Catalog", catalogAfterUpdate.Name)

            do! serviceCatalogManager.DeleteAsync(Builders<ServiceCatalog>.Filter.Eq((fun sc -> sc.Id), catalog.Id))
                |> Async.AwaitTask
        }
