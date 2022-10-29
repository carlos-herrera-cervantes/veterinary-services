namespace VeterinaryServices.Tests.Controllers

open System.Threading.Tasks
open System.Collections.Generic
open Microsoft.AspNetCore.Mvc
open Moq
open Xunit
open MongoDB.Driver
open VeterinaryServices.Web.Controllers
open VeterinaryServices.Web.Types
open VeterinaryServices.Repository.Managers
open VeterinaryServices.Repository.Repositories
open VeterinaryServices.Services.Pagers
open VeterinaryServices.Services.Types
open VeterinaryServices.Domain.Models

[<Collection("ServiceCatalogController")>]
type ServiceCatalogControllerTests() =

    [<Fact(DisplayName = "Should return 200 status code listing catalogs")>]
    member this.GetAllAsyncShouldReturn200() =
        async {
            let mockServiceCatalogManager = Mock<IServiceCatalogManager>()
            let mockServiceCatalogRepository = Mock<IServiceCatalogRepository>()
            let mockPagerStrategyManager = Mock<IPagerStrategyManager<ServiceCatalog>>()

            mockServiceCatalogRepository
                .Setup(fun x -> x.CountAsync(It.IsAny<FilterDefinition<ServiceCatalog>>()))
                .ReturnsAsync(int64(1)) |> ignore
            mockServiceCatalogRepository
                .Setup(fun x ->
                    x.GetAllAsync(
                        It.IsAny<FilterDefinition<ServiceCatalog>>(),
                        It.IsAny<int>(),
                        It.IsAny<int>()))
                .Returns(async {
                    let catalogs = [| ServiceCatalog() |]
                    return catalogs :> IEnumerable<ServiceCatalog>
                }) |> ignore
            mockPagerStrategyManager
                .Setup(fun x ->
                    x.GetPager(
                        It.IsAny<string>(),
                        It.IsAny<IEnumerable<ServiceCatalog>>(),
                        It.IsAny<int64>(),
                        It.IsAny<int>(),
                        It.IsAny<int>()))
                .Returns(Pager<ServiceCatalog>()) |> ignore

            let serviceCatalogController =
                ServiceCatalogController(
                    mockServiceCatalogManager.Object,
                    mockServiceCatalogRepository.Object,
                    mockPagerStrategyManager.Object
                )
            let pager = Pager()
            let! response = serviceCatalogController.GetAllAsync pager

            mockServiceCatalogRepository
                .Verify((fun x -> x.CountAsync(It.IsAny<FilterDefinition<ServiceCatalog>>())), Times.Once)
            mockServiceCatalogRepository
                .Verify((fun x ->
                    x.GetAllAsync(
                        It.IsAny<FilterDefinition<ServiceCatalog>>(),
                        It.IsAny<int>(),
                        It.IsAny<int>())), Times.Once)
            mockPagerStrategyManager
                .Verify((fun x ->
                    x.GetPager(
                        It.IsAny<string>(),
                        It.IsAny<IEnumerable<ServiceCatalog>>(),
                        It.IsAny<int64>(),
                        It.IsAny<int>(),
                        It.IsAny<int>())), Times.Once)

            Assert.IsType<OkObjectResult>(response) |> ignore
        }

    [<Fact(DisplayName = "Should return 404 when catalog does not exist")>]
    member this.GetByIdAsyncShouldReturn404() =
        async {
            let mockServiceCatalogManager = Mock<IServiceCatalogManager>()
            let mockServiceCatalogRepository = Mock<IServiceCatalogRepository>()
            let mockPagerStrategyManager = Mock<IPagerStrategyManager<ServiceCatalog>>()

            mockServiceCatalogRepository
                .Setup(fun x -> x.GetOneAsync(It.IsAny<FilterDefinition<ServiceCatalog>>()))
                .ReturnsAsync(fun () -> null) |> ignore

            let serviceCatalogController =
                ServiceCatalogController(
                    mockServiceCatalogManager.Object,
                    mockServiceCatalogRepository.Object,
                    mockPagerStrategyManager.Object
                )
            let! response = serviceCatalogController.GetByIdAsync "dummy-id"

            mockServiceCatalogRepository
                .Verify((fun x -> x.GetOneAsync(It.IsAny<FilterDefinition<ServiceCatalog>>())), Times.Once)

            Assert.IsType<NotFoundResult>(response) |> ignore
        }

    [<Fact(DisplayName = "Should return 200 when catalog exists")>]
    member this.GetByIdAsyncShouldReturn200() =
        async {
            let mockServiceCatalogManager = Mock<IServiceCatalogManager>()
            let mockServiceCatalogRepository = Mock<IServiceCatalogRepository>()
            let mockPagerStrategyManager = Mock<IPagerStrategyManager<ServiceCatalog>>()

            mockServiceCatalogRepository
                .Setup(fun x -> x.GetOneAsync(It.IsAny<FilterDefinition<ServiceCatalog>>()))
                .ReturnsAsync(fun () -> ServiceCatalog()) |> ignore

            let serviceCatalogController =
                ServiceCatalogController(
                    mockServiceCatalogManager.Object,
                    mockServiceCatalogRepository.Object,
                    mockPagerStrategyManager.Object
                )
            let! response = serviceCatalogController.GetByIdAsync "dummy-id"

            mockServiceCatalogRepository
                .Verify((fun x -> x.GetOneAsync(It.IsAny<FilterDefinition<ServiceCatalog>>())), Times.Once)

            Assert.IsType<OkObjectResult>(response) |> ignore
        }

    [<Fact(DisplayName = "Should return 201 when catalog is created")>]
    member this.CreateAsyncShouldReturn201() =
        async {
            let mockServiceCatalogManager = Mock<IServiceCatalogManager>()
            let mockServiceCatalogRepository = Mock<IServiceCatalogRepository>()
            let mockPagerStrategyManager = Mock<IPagerStrategyManager<ServiceCatalog>>()

            mockServiceCatalogManager
                .Setup(fun x -> x.CreateAsync(It.IsAny<ServiceCatalog>()))
                .Returns(Task.FromResult(true)) |> ignore

            let serviceCatalogController =
                ServiceCatalogController(
                    mockServiceCatalogManager.Object,
                    mockServiceCatalogRepository.Object,
                    mockPagerStrategyManager.Object
                )
            let serviceCatalog = ServiceCatalog()
            serviceCatalog.Name <- "Test catalog"
            serviceCatalog.Description <- "This is a test catalog"

            let! response = serviceCatalogController.CreateAsync(serviceCatalog)

            mockServiceCatalogManager
                .Verify((fun x -> x.CreateAsync(It.IsAny<ServiceCatalog>())), Times.Once)

            Assert.IsType<CreatedResult>(response) |> ignore
        }

    [<Fact(DisplayName = "Should return 404 when catalog does not exist")>]
    member this.UpdateByIdAsyncShouldReturn404() =
        async {
            let mockServiceCatalogManager = Mock<IServiceCatalogManager>()
            let mockServiceCatalogRepository = Mock<IServiceCatalogRepository>()
            let mockPagerStrategyManager = Mock<IPagerStrategyManager<ServiceCatalog>>()

            mockServiceCatalogRepository
                .Setup(fun x -> x.GetOneAsync(It.IsAny<FilterDefinition<ServiceCatalog>>()))
                .ReturnsAsync(fun () -> null) |> ignore

            let serviceCatalogController =
                ServiceCatalogController(
                    mockServiceCatalogManager.Object,
                    mockServiceCatalogRepository.Object,
                    mockPagerStrategyManager.Object
                )
            let serviceCatalog = ServiceCatalog()
            serviceCatalog.Name <- "Test catalog"
            serviceCatalog.Description <- "This is a test catalog"

            let! response = serviceCatalogController.UpdateByIdAsync("dummy-id", serviceCatalog)

            mockServiceCatalogRepository
                .Verify((fun x -> x.GetOneAsync(It.IsAny<FilterDefinition<ServiceCatalog>>())), Times.Once)
            mockServiceCatalogManager
                .Verify((fun x ->
                    x.UpdateAsync(
                        It.IsAny<FilterDefinition<ServiceCatalog>>(),
                        It.IsAny<ServiceCatalog>())), Times.Never)

            Assert.IsType<NotFoundResult>(response) |> ignore
        }

    [<Fact(DisplayName = "Should return 200 when catalog is updated")>]
    member this.UpdateByIdAsyncShouldReturn200() =
        async {
            let mockServiceCatalogManager = Mock<IServiceCatalogManager>()
            let mockServiceCatalogRepository = Mock<IServiceCatalogRepository>()
            let mockPagerStrategyManager = Mock<IPagerStrategyManager<ServiceCatalog>>()

            mockServiceCatalogRepository
                .Setup(fun x -> x.GetOneAsync(It.IsAny<FilterDefinition<ServiceCatalog>>()))
                .ReturnsAsync(fun () -> ServiceCatalog()) |> ignore
            mockServiceCatalogManager
                .Setup(fun x -> x.UpdateAsync(It.IsAny<FilterDefinition<ServiceCatalog>>(), It.IsAny<ServiceCatalog>()))
                .Returns(Task.FromResult(true)) |> ignore

            let serviceCatalogController =
                ServiceCatalogController(
                    mockServiceCatalogManager.Object,
                    mockServiceCatalogRepository.Object,
                    mockPagerStrategyManager.Object
                )
            let serviceCatalog = ServiceCatalog()
            serviceCatalog.Name <- "Test catalog"
            serviceCatalog.Description <- "This is a test catalog"

            let! response = serviceCatalogController.UpdateByIdAsync("dummy-id", serviceCatalog)

            mockServiceCatalogRepository
                .Verify((fun x -> x.GetOneAsync(It.IsAny<FilterDefinition<ServiceCatalog>>())), Times.Once)
            mockServiceCatalogManager
                .Verify((fun x ->
                    x.UpdateAsync(
                        It.IsAny<FilterDefinition<ServiceCatalog>>(),
                        It.IsAny<ServiceCatalog>())), Times.Once)

            Assert.IsType<OkObjectResult>(response) |> ignore
        }

    [<Fact(DisplayName = "Should return 404 when catalog does not exist")>]
    member this.DeleteByIdAsyncShouldReturn404() =
        async {
            let mockServiceCatalogManager = Mock<IServiceCatalogManager>()
            let mockServiceCatalogRepository = Mock<IServiceCatalogRepository>()
            let mockPagerStrategyManager = Mock<IPagerStrategyManager<ServiceCatalog>>()

            mockServiceCatalogRepository
                .Setup(fun x -> x.GetOneAsync(It.IsAny<FilterDefinition<ServiceCatalog>>()))
                .ReturnsAsync(fun () -> null) |> ignore

            let serviceCatalogController =
                ServiceCatalogController(
                    mockServiceCatalogManager.Object,
                    mockServiceCatalogRepository.Object,
                    mockPagerStrategyManager.Object
                )
            let! response = serviceCatalogController.DeleteByIdAsync "dummy-id"

            mockServiceCatalogRepository
                .Verify((fun x -> x.GetOneAsync(It.IsAny<FilterDefinition<ServiceCatalog>>())), Times.Once)
            mockServiceCatalogManager
                .Verify((fun x -> x.DeleteAsync(It.IsAny<FilterDefinition<ServiceCatalog>>())), Times.Never)

            Assert.IsType<NotFoundResult>(response) |> ignore
        }

    [<Fact(DisplayName = "Should return 204 when catalog is eliminated")>]
    member this.DeleteByIdAsyncShouldReturn204() =
        async {
            let mockServiceCatalogManager = Mock<IServiceCatalogManager>()
            let mockServiceCatalogRepository = Mock<IServiceCatalogRepository>()
            let mockPagerStrategyManager = Mock<IPagerStrategyManager<ServiceCatalog>>()

            mockServiceCatalogRepository
                .Setup(fun x -> x.GetOneAsync(It.IsAny<FilterDefinition<ServiceCatalog>>()))
                .ReturnsAsync(fun () -> ServiceCatalog()) |> ignore
            mockServiceCatalogManager
                .Setup(fun x -> x.DeleteAsync(It.IsAny<FilterDefinition<ServiceCatalog>>()))
                .Returns(Task.FromResult(true)) |> ignore

            let serviceCatalogController =
                ServiceCatalogController(
                    mockServiceCatalogManager.Object,
                    mockServiceCatalogRepository.Object,
                    mockPagerStrategyManager.Object
                )
            let! response = serviceCatalogController.DeleteByIdAsync "dummy-id"

            mockServiceCatalogRepository
                .Verify((fun x -> x.GetOneAsync(It.IsAny<FilterDefinition<ServiceCatalog>>())), Times.Once)
            mockServiceCatalogManager
                .Verify((fun x -> x.DeleteAsync(It.IsAny<FilterDefinition<ServiceCatalog>>())), Times.Once)

            Assert.IsType<NoContentResult>(response) |> ignore
        }
