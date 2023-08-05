namespace VeterinaryServices.Tests.Repository.Repositories

open System
open MongoDB.Driver
open Xunit
open VeterinaryServices.Repository.Repositories
open VeterinaryServices.Domain.Models

[<Collection(nameof(PetRepository))>]
type PetRepositoryTests() =

    let uri = Environment.GetEnvironmentVariable("MONGO_DB_URI")

    member private __.mongoClient = MongoClient(uri)

    [<Fact(DisplayName = "Should return 0 documents")>]
    member __.CounAsyncShouldReturnZeroDocuments(): Async<unit> =
        async {
            let petRepository = PetRepository(__.mongoClient) :> IPetRepository
            let countFilter = Builders<Pet>.Filter.Eq((fun p -> p.Id), "63716ae486a27e32f5f89efd")
            let! counter = petRepository.CountAsync(countFilter) |> Async.AwaitTask
            Assert.Equal(int64(0), counter)
        }
