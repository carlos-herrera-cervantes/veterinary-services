namespace VeterinaryServices.Tests.Repository.Repositories

open System
open MongoDB.Driver
open Xunit
open VeterinaryServices.Repository.Repositories
open VeterinaryServices.Domain.Models

[<Collection(nameof(ServiceRepository))>]
type ServiceRepositoryTests() =

    let uri = Environment.GetEnvironmentVariable("MONGO_DB_URI")

    member private __.mongoClient = MongoClient(uri)

    [<Fact(DisplayName = "Should return an empty list")>]
    member __.GetAllAsyncShouldReturnEmptyList(): Async<unit> =
        async {
            let serviceRepository = ServiceRepository(__.mongoClient) :> IServiceRepository
            let! services =
                serviceRepository
                    .GetAllAsync(Builders<Service>.Filter.Eq((fun s -> s.Name), "Hair Cut"), 0, 10)
            Assert.Empty(services)
        }

    [<Fact(DisplayName = "Should return null when document does not exist")>]
    member __.GetOneAsyncShouldReturnNull(): Async<unit> =
        async {
            let serviceRepository = ServiceRepository(__.mongoClient) :> IServiceRepository
            let! service =
                serviceRepository.GetOneAsync(Builders<Service>.Filter.Eq((fun s -> s.Name), "Hair Cut"))
                |> Async.AwaitTask
            Assert.Null(service)
        }

    [<Fact(DisplayName = "Should return 0 documents")>]
    member __.CountAsyncShouldReturnZeroDocuments(): Async<unit> =
        async {
            let serviceRepository = ServiceRepository(__.mongoClient) :> IServiceRepository
            let! counter =
                serviceRepository.CountAsync(Builders<Service>.Filter.Eq((fun s -> s.Name), "Hair Cut"))
                |> Async.AwaitTask
            Assert.Equal(int64(0), counter)
        }
