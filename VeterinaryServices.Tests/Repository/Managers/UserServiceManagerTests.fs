namespace VeterinaryServices.Tests.Repository.Managers

open System
open MongoDB.Driver
open Xunit
open VeterinaryServices.Repository.Repositories
open VeterinaryServices.Repository.Managers
open VeterinaryServices.Domain.Models

[<Collection(nameof(UserServiceManager))>]
type UserServiceManagerTests() =

    let uri = Environment.GetEnvironmentVariable("MONGO_DB_URI")

    member private this._mongoClient = MongoClient(uri)

    [<Fact(DisplayName = "Should create and update document")>]
    member this.CreateAndUpdateAsyncShouldCreateAndUpdateDocument() =
        async {
            let userServiceManager = UserServiceManager(this._mongoClient) :> IUserServiceManager
            let newUserService = UserService()
            newUserService.CustomerId <- "63707d2ba64a2dc5cf24bd73"
            newUserService.PetId <- "63707d4839e9155fb8460044"
            newUserService.PetName <- "Miguel"

            do! userServiceManager.CreateAsync(newUserService) |> Async.AwaitTask

            let userServiceRepository = UserServiceRepository(this._mongoClient) :> IUserServiceRepository
            let! userService =
                userServiceRepository
                    .GetOneAsync(Builders<UserService>.Filter.Eq((fun us -> us.CustomerId), "63707d2ba64a2dc5cf24bd73"))
                    |> Async.AwaitTask
            Assert.NotNull(userService)

            userService.PetName <- "Antonio"
            do! userServiceManager
                    .UpdateAsync(
                        Builders<UserService>.Filter.Eq((fun us -> us.CustomerId), "63707d2ba64a2dc5cf24bd73"),
                        userService
                    )
                |> Async.AwaitTask

            let! userServiceAfterUpdate =
                userServiceRepository
                    .GetOneAsync(Builders<UserService>.Filter.Eq((fun us -> us.CustomerId), "63707d2ba64a2dc5cf24bd73"))
                    |> Async.AwaitTask
            Assert.Equal("Antonio", userServiceAfterUpdate.PetName)

            do! userServiceManager.DeleteManyAsync(Builders<UserService>.Filter.Empty) |> Async.AwaitTask
        }
