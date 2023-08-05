namespace VeterinaryServices.Tests.Services.Calculators

open System.Collections.Generic
open Xunit
open Moq
open MongoDB.Driver
open VeterinaryServices.Services.Calculators
open VeterinaryServices.Repository.Repositories
open VeterinaryServices.Domain.Models

[<Collection("ClassicCalculator")>]
type ClassicCalculatorTests() =

    [<Fact(DisplayName = "Should return 0 when the services do not exist")>]
    member __.CalculateTotalAsyncShouldReturn0(): Async<unit> =
        async {
            let mockServiceRepository = Mock<IServiceRepository>()

            mockServiceRepository
                .Setup(fun x ->
                    x.GetAllAsync(
                        It.IsAny<FilterDefinition<Service>>(),
                        It.IsAny<int>(),
                        It.IsAny<int>()))
                .Returns(async {
                    let services = [||]
                    return services :> ICollection<Service>}) |> ignore

            let classicCalculator = ClassicCalculator(mockServiceRepository.Object) :> ITotalCalculator
            let! total = classicCalculator.CalculateTotalAsync([|"dummy-id"; "dummy-id-2"|])

            mockServiceRepository
                .Verify((fun x ->
                    x.GetAllAsync(
                        It.IsAny<FilterDefinition<Service>>(),
                        It.IsAny<int>(),
                        It.IsAny<int>())), Times.Once)

            Assert.IsType<decimal>(total) |> ignore
            Assert.Equal(0M, total)
        }

    [<Fact(DisplayName = "Should return the correct total")>]
    member __.CalculateTotalAsyncShouldReturnSuccessCalculation(): Async<unit> =
        async {
            let mockServiceRepository = Mock<IServiceRepository>()
            
            mockServiceRepository
                .Setup(fun x ->
                    x.GetAllAsync(
                        It.IsAny<FilterDefinition<Service>>(),
                        It.IsAny<int>(),
                        It.IsAny<int>()))
                .Returns(async {
                    let service1 = Service()
                    service1.Price <- 10M
                    let service2 = Service()
                    service2.Price <- 20M

                    let services = [|service1; service2|]
                    return services :> ICollection<Service>}) |> ignore

            let classicCalculator = ClassicCalculator(mockServiceRepository.Object) :> ITotalCalculator
            let! total = classicCalculator.CalculateTotalAsync([|"dummy-id"; "dummy-id-2"|])

            mockServiceRepository
                .Verify((fun x ->
                    x.GetAllAsync(
                        It.IsAny<FilterDefinition<Service>>(),
                        It.IsAny<int>(),
                        It.IsAny<int>())), Times.Once)

            Assert.IsType<decimal>(total) |> ignore
            Assert.Equal(30M, total)
        }
