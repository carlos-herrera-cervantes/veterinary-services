namespace VeterinaryServices.Tests.Repository.Repositories

open System
open MongoDB.Driver
open Xunit
open VeterinaryServices.Repository.Repositories
open VeterinaryServices.Domain.Models

[<Collection(nameof(ServiceCatalogRepository))>]
type ServiceCatalogRepositoryTests() =

    let uri = Environment.GetEnvironmentVariable("MONGO_DB_URI")

    member private __.mongoClient = MongoClient(uri)

    [<Fact(DisplayName = "Should return an empty list")>]
    member __.GetAllAsyncShouldReturnEmptyList(): Async<unit> =
        async {
            let serviceCatalogRepository = new ServiceCatalogRepository(__.mongoClient) :> IServiceCatalogRepository
            let! catalogs =
                serviceCatalogRepository
                    .GetAllAsync(Builders<ServiceCatalog>.Filter.Eq((fun sc -> sc.Name), "dummy"), 0, 10)
            Assert.Empty(catalogs)
        }

    [<Fact(DisplayName = "Should return null when document does not exist")>]
    member __.GetOneAsyncShouldReturnNull(): Async<unit> =
        async {
            let serviceCatalogRepository = new ServiceCatalogRepository(__.mongoClient) :> IServiceCatalogRepository
            let! catalog =
                serviceCatalogRepository.GetOneAsync(Builders<ServiceCatalog>.Filter.Eq((fun sc -> sc.Name), "dummy"))
                |> Async.AwaitTask
            Assert.Null(catalog)
        }

    [<Fact(DisplayName = "Should return 0 documents")>]
    member __.CountAsyncShouldReturnZeroDocuments(): Async<unit> =
        async {
            let serviceCatalogRepository = new ServiceCatalogRepository(__.mongoClient) :> IServiceCatalogRepository
            let! counter =
                serviceCatalogRepository.CountAsync(Builders<ServiceCatalog>.Filter.Eq((fun sc -> sc.Name), "dummy"))
                |> Async.AwaitTask
            Assert.Equal(int64(0), counter)
        }
