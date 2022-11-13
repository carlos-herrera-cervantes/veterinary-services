namespace VeterinaryServices.Tests.Repository.Managers

open System
open MongoDB.Driver
open Xunit
open VeterinaryServices.Repository.Repositories
open VeterinaryServices.Repository.Managers
open VeterinaryServices.Domain.Models

[<Collection(nameof(ServiceManager))>]
type ServiceManagerTests() =

    let uri = Environment.GetEnvironmentVariable("MONGO_DB_URI")

    member private this._mongoClient = MongoClient(uri)

    [<Fact(DisplayName = "Should create, update and delete a document")>]
    member this.CreateUpdateAndDeleteAsyncShouldCreateUpdateAndDeleteDocument() =
        async {
            let newService = Service()
            newService.Name <- "Health Care"
            newService.Description <- "This is a test description"
            newService.CategoryId <- "6371486d7359884e2d3b3bda"
            newService.MeasurementUnit <- "Night"

            let serviceManager = ServiceManager(this._mongoClient) :> IServiceManager
            do! serviceManager.CreateAsync(newService) |> Async.AwaitTask

            let serviceRepository = ServiceRepository(this._mongoClient) :> IServiceRepository
            let! service =
                serviceRepository.GetOneAsync(Builders<Service>.Filter.Eq((fun s -> s.Name), "Health Care"))
                |> Async.AwaitTask
            Assert.NotNull(service)

            service.Description <- "This is an update description"
            do! serviceManager.UpdateAsync(Builders<Service>.Filter.Eq((fun s -> s.Id), service.Id), service)
                |> Async.AwaitTask

            do! serviceManager.DeleteAsync(Builders<Service>.Filter.Eq((fun s -> s.Id), service.Id))
                |> Async.AwaitTask
        }
