﻿namespace VeterinaryServices.Tests.Repository.Repositories

open System
open MongoDB.Driver
open Xunit
open VeterinaryServices.Repository.Repositories
open VeterinaryServices.Domain.Models

[<Collection(nameof(UserServiceRepository))>]
type UserServiceRepositoryTests() =

    let uri = Environment.GetEnvironmentVariable("MONGO_DB_URI")

    member private this._mongoClient = MongoClient(uri)

    [<Fact(DisplayName = "Should return an empty list")>]
    member this.GetAllAsyncShouldReturnEmptyList() =
        async {
            let userServiceRepository = UserServiceRepository(this._mongoClient) :> IUserServiceRepository
            let! services =
                userServiceRepository
                    .GetAllAsync(
                        Builders<UserService>.Filter.Eq((fun us -> us.CustomerId), "63708b537d3736e56f1ebaaf"),
                        0, 10
                    )
            Assert.Empty(services)
        }

    [<Fact(DisplayName = "Should return null when document does not exist")>]
    member this.GetOneAsyncShouldReturnNull() =
        async {
            let userServiceRepository = UserServiceRepository(this._mongoClient) :> IUserServiceRepository
            let! service =
                userServiceRepository
                    .GetOneAsync(Builders<UserService>.Filter.Eq((fun us -> us.CustomerId), "63708b537d3736e56f1ebaaf"))
                |> Async.AwaitTask
            Assert.Null(service)
        }

    [<Fact(DisplayName = "Should return 0 or more documents")>]
    member this.CountAsyncShouldReturnDocumentCounter() =
        async {
            let userServiceRepository = UserServiceRepository(this._mongoClient) :> IUserServiceRepository
            let! counter = userServiceRepository.CountAsync(Builders<UserService>.Filter.Empty) |> Async.AwaitTask
            Assert.NotNull(counter)
        }
