﻿namespace VeterinaryServices.Tests.Repository.Repositories

open System
open MongoDB.Driver
open Xunit
open VeterinaryServices.Repository.Repositories
open VeterinaryServices.Domain.Models

[<Collection(nameof(EmployeeRepository))>]
type EmployeeRepositoryTests() =

    let uri = Environment.GetEnvironmentVariable("MONGO_DB_URI")

    member private __.mongoClient = MongoClient(uri)

    [<Fact(DisplayName = "Should return 0 documents")>]
    member __.CountAsyncShouldReturnZeroDocuments(): Async<unit> =
        async {
            let employeeRepository = EmployeeRepository(__.mongoClient) :> IEmployeeRepository
            let countFilter = Builders<Employee>.Filter.Eq((fun e -> e.EmployeeId), "6371726664c89c8f90fddd01")
            let! counter = employeeRepository.CountAsync(countFilter) |> Async.AwaitTask
            Assert.Equal(int64(0), counter)
        }
