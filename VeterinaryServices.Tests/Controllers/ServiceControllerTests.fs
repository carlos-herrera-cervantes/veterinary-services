namespace VeterinaryServices.Tests.Controllers

open Microsoft.AspNetCore.Mvc
open System.Collections.Generic
open System.Threading.Tasks
open Xunit
open Moq
open MongoDB.Driver
open VeterinaryServices.Repository.Managers
open VeterinaryServices.Repository.Repositories
open VeterinaryServices.Services.Calculators
open VeterinaryServices.Services.Pagers
open VeterinaryServices.Services.Types
open VeterinaryServices.Domain.Models
open VeterinaryServices.Web.Types
open VeterinaryServices.Web.Controllers

[<Collection("ServiceController")>]
type ServiceControllerTests() =

    [<Fact(DisplayName = "Should return 200 when services are listed successfully")>]
    member __.GetAllAsyncShouldReturn200(): Async<unit> =
        async {
            let mockServiceRepository = Mock<IServiceRepository>()
            let mockServiceManager = Mock<IServiceManager>()
            let mockTotalCalculatorStrategyManager = Mock<ITotalCalculator>()
            let mockPagerStrategyManager = Mock<IPageable<Service>>()

            mockServiceRepository
                .Setup(fun x -> x.CountAsync(It.IsAny<FilterDefinition<Service>>()))
                .ReturnsAsync(int64(1)) |> ignore
            mockServiceRepository
                .Setup(fun x -> x.GetAllAsync(It.IsAny<FilterDefinition<Service>>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(async {
                    let services = [| Service() |]
                    return services :> ICollection<Service>
                }) |> ignore
            mockPagerStrategyManager
                .Setup(fun x ->
                    x.GetPages(
                        It.IsAny<IEnumerable<Service>>(),
                        It.IsAny<int64>(),
                        It.IsAny<int>(),
                        It.IsAny<int>()))
                .Returns(Pager<Service>()) |> ignore

            let serviceController =
                ServiceController(
                    mockServiceRepository.Object,
                    mockServiceManager.Object,
                    mockTotalCalculatorStrategyManager.Object,
                    mockPagerStrategyManager.Object
                )

            let! response = serviceController.GetAllAsync(Pager())

            mockServiceRepository
                .Verify((fun x -> x.CountAsync(It.IsAny<FilterDefinition<Service>>())), Times.Once)
            mockServiceRepository
                .Verify((fun x ->
                    x.GetAllAsync(
                        It.IsAny<FilterDefinition<Service>>(),
                        It.IsAny<int>(),
                        It.IsAny<int>())), Times.Once)
            mockPagerStrategyManager
                .Verify((fun x ->
                    x.GetPages(
                        It.IsAny<IEnumerable<Service>>(),
                        It.IsAny<int64>(),
                        It.IsAny<int>(),
                        It.IsAny<int>())), Times.Once)

            Assert.IsType<OkObjectResult>(response) |> ignore
        }

    [<Fact(DisplayName = "Should return 404 when service does not exist")>]
    member __.GetByIdAsyncShouldReturn404(): Async<unit> =
        async {
            let mockServiceRepository = Mock<IServiceRepository>()
            let mockServiceManager = Mock<IServiceManager>()
            let mockTotalCalculatorStrategyManager = Mock<ITotalCalculator>()
            let mockPagerStrategyManager = Mock<IPageable<Service>>()

            mockServiceRepository
                .Setup(fun x -> x.GetOneAsync(It.IsAny<FilterDefinition<Service>>()))
                .ReturnsAsync(fun () -> null) |> ignore

            let serviceController =
                ServiceController(
                    mockServiceRepository.Object,
                    mockServiceManager.Object,
                    mockTotalCalculatorStrategyManager.Object,
                    mockPagerStrategyManager.Object
                )

            let! response = serviceController.GetByIdAsync "dummy-id"

            mockServiceRepository
                .Verify((fun x -> x.GetOneAsync(It.IsAny<FilterDefinition<Service>>())), Times.Once)

            Assert.IsType<NotFoundObjectResult>(response) |> ignore
        }

    [<Fact(DisplayName = "Should return 200 when service exist")>]
    member __.GetByIdAsyncShouldReturn200(): Async<unit> =
        async {
            let mockServiceRepository = Mock<IServiceRepository>()
            let mockServiceManager = Mock<IServiceManager>()
            let mockTotalCalculatorStrategyManager = Mock<ITotalCalculator>()
            let mockPagerStrategyManager = Mock<IPageable<Service>>()

            mockServiceRepository
                .Setup(fun x -> x.GetOneAsync(It.IsAny<FilterDefinition<Service>>()))
                .ReturnsAsync(Service()) |> ignore

            let serviceController =
                ServiceController(
                    mockServiceRepository.Object,
                    mockServiceManager.Object,
                    mockTotalCalculatorStrategyManager.Object,
                    mockPagerStrategyManager.Object
                )

            let! response = serviceController.GetByIdAsync "dummy-id"

            mockServiceRepository
                .Verify((fun x -> x.GetOneAsync(It.IsAny<FilterDefinition<Service>>())), Times.Once)

            Assert.IsType<OkObjectResult>(response) |> ignore
        }

    [<Fact(DisplayName = "Should return 200 when the total is calculated successfully")>]
    member __.CalculateTotalAsyncShouldReturn200(): Async<unit> =
        async {
            let mockServiceRepository = Mock<IServiceRepository>()
            let mockServiceManager = Mock<IServiceManager>()
            let mockTotalCalculatorStrategyManager = Mock<ITotalCalculator>()
            let mockPagerStrategyManager = Mock<IPageable<Service>>()

            mockTotalCalculatorStrategyManager
                .Setup(fun x -> x.CalculateTotalAsync(It.IsAny<string array>()))
                .Returns(async { return 100M }) |> ignore

            let serviceController =
                ServiceController(
                    mockServiceRepository.Object,
                    mockServiceManager.Object,
                    mockTotalCalculatorStrategyManager.Object,
                    mockPagerStrategyManager.Object
                )

            let! response = serviceController.CalculateTotalAsync(ServiceTotal())

            mockTotalCalculatorStrategyManager
                .Verify((fun x -> x.CalculateTotalAsync(It.IsAny<string array>())), Times.Once)

            Assert.IsType<OkObjectResult>(response) |> ignore
        }

    [<Fact(DisplayName = "Should return 404 when service does not exist")>]
    member __.UpdateByIdAsyncShouldReturn404(): Async<unit> =
        async {
            let mockServiceRepository = Mock<IServiceRepository>()
            let mockServiceManager = Mock<IServiceManager>()
            let mockTotalCalculatorStrategyManager = Mock<ITotalCalculator>()
            let mockPagerStrategyManager = Mock<IPageable<Service>>()

            mockServiceRepository
                .Setup(fun x -> x.GetOneAsync(It.IsAny<FilterDefinition<Service>>()))
                .ReturnsAsync(fun () -> null) |> ignore

            let serviceController =
                ServiceController(
                    mockServiceRepository.Object,
                    mockServiceManager.Object,
                    mockTotalCalculatorStrategyManager.Object,
                    mockPagerStrategyManager.Object
                )

            let! response = serviceController.UpdateByIdAsync("dummy-id", Service())

            mockServiceRepository
                .Verify((fun x -> x.GetOneAsync(It.IsAny<FilterDefinition<Service>>())), Times.Once)

            Assert.IsType<NotFoundObjectResult>(response) |> ignore
        }

    [<Fact(DisplayName = "Should return 200 when service is updated successfully")>]
    member __.UpdateByIdAsyncShouldReturn200(): Async<unit> =
        async {
            let mockServiceRepository = Mock<IServiceRepository>()
            let mockServiceManager = Mock<IServiceManager>()
            let mockTotalCalculatorStrategyManager = Mock<ITotalCalculator>()
            let mockPagerStrategyManager = Mock<IPageable<Service>>()

            mockServiceRepository
                .Setup(fun x -> x.GetOneAsync(It.IsAny<FilterDefinition<Service>>()))
                .ReturnsAsync(Service()) |> ignore

            let serviceController =
                ServiceController(
                    mockServiceRepository.Object,
                    mockServiceManager.Object,
                    mockTotalCalculatorStrategyManager.Object,
                    mockPagerStrategyManager.Object
                )

            let! response = serviceController.UpdateByIdAsync("dummy-id", Service())

            mockServiceRepository
                .Verify((fun x -> x.GetOneAsync(It.IsAny<FilterDefinition<Service>>())), Times.Once)

            Assert.IsType<OkObjectResult>(response) |> ignore
        }

    [<Fact(DisplayName = "Should return 404 when service does not exist")>]
    member __.DeleteByIdAsyncShouldReturn404(): Async<unit> =
        async {
            let mockServiceRepository = Mock<IServiceRepository>()
            let mockServiceManager = Mock<IServiceManager>()
            let mockTotalCalculatorStrategyManager = Mock<ITotalCalculator>()
            let mockPagerStrategyManager = Mock<IPageable<Service>>()

            mockServiceRepository
                .Setup(fun x -> x.GetOneAsync(It.IsAny<FilterDefinition<Service>>()))
                .ReturnsAsync(fun () -> null) |> ignore

            let serviceController =
                ServiceController(
                    mockServiceRepository.Object,
                    mockServiceManager.Object,
                    mockTotalCalculatorStrategyManager.Object,
                    mockPagerStrategyManager.Object
                )

            let! response = serviceController.DeleteByIdAsync "dummy-id"

            mockServiceRepository
                .Verify((fun x -> x.GetOneAsync(It.IsAny<FilterDefinition<Service>>())), Times.Once)

            Assert.IsType<NotFoundObjectResult>(response) |> ignore
        }

    [<Fact(DisplayName = "Should return 204 when service is deleted successfully")>]
    member __.DeleteByIdAsyncShouldReturn204(): Async<unit> =
        async {
            let mockServiceRepository = Mock<IServiceRepository>()
            let mockServiceManager = Mock<IServiceManager>()
            let mockTotalCalculatorStrategyManager = Mock<ITotalCalculator>()
            let mockPagerStrategyManager = Mock<IPageable<Service>>()

            mockServiceRepository
                .Setup(fun x -> x.GetOneAsync(It.IsAny<FilterDefinition<Service>>()))
                .ReturnsAsync(Service()) |> ignore
            mockServiceManager
                .Setup(fun x -> x.DeleteAsync(It.IsAny<FilterDefinition<Service>>()))
                .Returns(Task.CompletedTask) |> ignore

            let serviceController =
                ServiceController(
                    mockServiceRepository.Object,
                    mockServiceManager.Object,
                    mockTotalCalculatorStrategyManager.Object,
                    mockPagerStrategyManager.Object
                )

            let! response = serviceController.DeleteByIdAsync "dummy-id"

            mockServiceRepository
                .Verify((fun x -> x.GetOneAsync(It.IsAny<FilterDefinition<Service>>())), Times.Once)
            mockServiceManager
                .Verify((fun x -> x.DeleteAsync(It.IsAny<FilterDefinition<Service>>())), Times.Once)

            Assert.IsType<NoContentResult>(response) |> ignore
        }
