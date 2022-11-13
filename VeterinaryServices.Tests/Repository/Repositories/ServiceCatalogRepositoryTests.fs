namespace VeterinaryServices.Tests.Repository.Repositories

open System
open MongoDB.Driver
open Xunit
open VeterinaryServices.Repository.Repositories
open VeterinaryServices.Domain.Models

[<Collection(nameof(ServiceCatalogRepository))>]
type ServiceCatalogRepositoryTests() =

    let uri = Environment.GetEnvironmentVariable("MONGO_DB_URI")

    member private this._mongoClient = MongoClient(uri)

    [<Fact(DisplayName = "Should return an empty list")>]
    member this.GetAllAsyncShouldReturnEmptyList() =
        async {
            let serviceCatalogRepository = new ServiceCatalogRepository(this._mongoClient) :> IServiceCatalogRepository
            let! catalogs =
                serviceCatalogRepository
                    .GetAllAsync(Builders<ServiceCatalog>.Filter.Eq((fun sc -> sc.Name), "dummy"), 0, 10)
            Assert.Empty(catalogs)
        }

    [<Fact(DisplayName = "Should return null when document does not exist")>]
    member this.GetOneAsyncShouldReturnNull() =
        async {
            let serviceCatalogRepository = new ServiceCatalogRepository(this._mongoClient) :> IServiceCatalogRepository
            let! catalog =
                serviceCatalogRepository.GetOneAsync(Builders<ServiceCatalog>.Filter.Eq((fun sc -> sc.Name), "dummy"))
                |> Async.AwaitTask
            Assert.Null(catalog)
        }

    [<Fact(DisplayName = "Should return 0 documents")>]
    member this.CountAsyncShouldReturnZeroDocuments() =
        async {
            let serviceCatalogRepository = new ServiceCatalogRepository(this._mongoClient) :> IServiceCatalogRepository
            let! counter =
                serviceCatalogRepository.CountAsync(Builders<ServiceCatalog>.Filter.Eq((fun sc -> sc.Name), "dummy"))
                |> Async.AwaitTask
            Assert.Equal(int64(0), counter)
        }
