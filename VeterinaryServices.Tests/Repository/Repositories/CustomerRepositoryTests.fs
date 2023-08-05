﻿namespace VeterinaryServices.Tests.Repository.Repositories

open System
open MongoDB.Driver
open Xunit
open VeterinaryServices.Repository.Repositories
open VeterinaryServices.Domain.Models

type CustomerRepositoryTests() =

    let uri = Environment.GetEnvironmentVariable("MONGO_DB_URI")

    member private __.mongoClient = MongoClient(uri)

    [<Fact(DisplayName = "Should return 0 documents")>]
    member __.CountAsyncShouldReturnZeroDocuments(): Async<unit> =
        async {
            let customerRepository = CustomerRepository(__.mongoClient) :> ICustomerRepository
            let countFilter = Builders<Customer>.Filter.Eq((fun c -> c.Id), "63716e62f44992c87feefd3e")
            let! counter = customerRepository.CountAsync(countFilter) |> Async.AwaitTask
            Assert.Equal(int64(0), counter)
        }
