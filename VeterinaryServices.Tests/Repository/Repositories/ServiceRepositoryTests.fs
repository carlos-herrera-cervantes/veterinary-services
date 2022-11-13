namespace VeterinaryServices.Tests.Repository.Repositories

open System
open MongoDB.Driver
open Xunit
open VeterinaryServices.Repository.Repositories
open VeterinaryServices.Domain.Models

[<Collection(nameof(ServiceRepository))>]
type ServiceRepositoryTests() =

    let uri = Environment.GetEnvironmentVariable("MONGO_DB_URI")

    member private this._mongoClient = MongoClient(uri)

    [<Fact(DisplayName = "Should return an empty list")>]
    member this.GetAllAsyncShouldReturnEmptyList() =
        async {
            let serviceRepository = ServiceRepository(this._mongoClient) :> IServiceRepository
            let! services =
                serviceRepository
                    .GetAllAsync(Builders<Service>.Filter.Eq((fun s -> s.Name), "Hair Cut"), 0, 10)
            Assert.Empty(services)
        }

    [<Fact(DisplayName = "Should return null when document does not exist")>]
    member this.GetOneAsyncShouldReturnNull() =
        async {
            let serviceRepository = ServiceRepository(this._mongoClient) :> IServiceRepository
            let! service =
                serviceRepository.GetOneAsync(Builders<Service>.Filter.Eq((fun s -> s.Name), "Hair Cut"))
                |> Async.AwaitTask
            Assert.Null(service)
        }

    [<Fact(DisplayName = "Should return 0 documents")>]
    member this.CountAsyncShouldReturnZeroDocuments() =
        async {
            let serviceRepository = ServiceRepository(this._mongoClient) :> IServiceRepository
            let! counter =
                serviceRepository.CountAsync(Builders<Service>.Filter.Eq((fun s -> s.Name), "Hair Cut"))
                |> Async.AwaitTask
            Assert.Equal(int64(0), counter)
        }
