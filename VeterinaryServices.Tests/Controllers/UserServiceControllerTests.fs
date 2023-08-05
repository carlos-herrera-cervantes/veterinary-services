namespace VeterinaryServices.Tests.Controllers

open Microsoft.AspNetCore.Mvc
open System.Collections.Generic
open System.Threading.Tasks
open Moq
open Xunit
open MongoDB.Driver
open AutoMapper
open VeterinaryServices.Web.Controllers
open VeterinaryServices.Web.Types
open VeterinaryServices.Repository.Managers
open VeterinaryServices.Repository.Repositories
open VeterinaryServices.Services.Calculators
open VeterinaryServices.Services.Pagers
open VeterinaryServices.Services.Types
open VeterinaryServices.Domain.Models
open VeterinaryServices.Domain.Constants

[<Collection("UserServiceController")>]
type UserServiceControllerTests() =

    [<Fact(DisplayName = "Should return 200 when list user services success")>]
    member __.GetAllAsyncShouldReturn200(): Async<unit> =
        async {
            let mockUserServiceManager = Mock<IUserServiceManager>()
            let mockUserServiceRepository = Mock<IUserServiceRepository>()
            let mockServiceRepository = Mock<IServiceRepository>()
            let mockTotalCalulatorStrategyManager = Mock<ITotalCalculator>()
            let mockPagerStrategyManager = Mock<IPageable<UserService>>()
            let mockMapper = Mock<IMapper>()

            mockUserServiceRepository
                .Setup(fun x -> x.CountAsync(It.IsAny<FilterDefinition<UserService>>()))
                .ReturnsAsync(int64(1)) |> ignore
            mockUserServiceRepository
                .Setup(fun x -> x.GetAllAsync(It.IsAny<FilterDefinition<UserService>>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(async {
                    let userServices = [| UserService() |]
                    return userServices :> IEnumerable<UserService>
                }) |> ignore
            mockPagerStrategyManager
                .Setup(fun x ->
                    x.GetPages(
                        It.IsAny<IEnumerable<UserService>>(),
                        It.IsAny<int64>(),
                        It.IsAny<int>(),
                        It.IsAny<int>()))
                .Returns(Pager<UserService>()) |> ignore

            let userServiceController =
                UserServiceController(
                    mockUserServiceManager.Object,
                    mockUserServiceRepository.Object,
                    mockServiceRepository.Object,
                    mockTotalCalulatorStrategyManager.Object,
                    mockPagerStrategyManager.Object,
                    mockMapper.Object
                )

            let! response = userServiceController.GetAllAsync(Pager())

            mockUserServiceRepository
                .Verify((fun x -> x.CountAsync(It.IsAny<FilterDefinition<UserService>>())), Times.Once)
            mockUserServiceRepository
                .Verify((fun x ->
                    x.GetAllAsync(
                        It.IsAny<FilterDefinition<UserService>>(),
                        It.IsAny<int>(),
                        It.IsAny<int>())), Times.Once)
            mockPagerStrategyManager
                .Verify((fun x ->
                    x.GetPages(
                        It.IsAny<IEnumerable<UserService>>(),
                        It.IsAny<int64>(),
                        It.IsAny<int>(),
                        It.IsAny<int>())), Times.Once)

            Assert.IsType<OkObjectResult>(response) |> ignore
        }

    [<Fact(DisplayName = "Should return 200 when user services are successfully listed for a particular user")>]
    member __.GetAllMeAsync(): Async<unit> =
        async {
            let mockUserServiceManager = Mock<IUserServiceManager>()
            let mockUserServiceRepository = Mock<IUserServiceRepository>()
            let mockServiceRepository = Mock<IServiceRepository>()
            let mockTotalCalulatorStrategyManager = Mock<ITotalCalculator>()
            let mockPagerStrategyManager = Mock<IPageable<UserService>>()
            let mockMapper = Mock<IMapper>()

            mockUserServiceRepository
                .Setup(fun x -> x.CountAsync(It.IsAny<FilterDefinition<UserService>>()))
                .ReturnsAsync(int64(1)) |> ignore
            mockUserServiceRepository
                .Setup(fun x -> x.GetAllAsync(It.IsAny<FilterDefinition<UserService>>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(async {
                    let userServices = [| UserService() |]
                    return userServices :> IEnumerable<UserService>
                }) |> ignore
            mockPagerStrategyManager
                .Setup(fun x ->
                    x.GetPages(
                        It.IsAny<IEnumerable<UserService>>(),
                        It.IsAny<int64>(),
                        It.IsAny<int>(),
                        It.IsAny<int>()))
                .Returns(Pager<UserService>()) |> ignore

            let userServiceController =
                UserServiceController(
                    mockUserServiceManager.Object,
                    mockUserServiceRepository.Object,
                    mockServiceRepository.Object,
                    mockTotalCalulatorStrategyManager.Object,
                    mockPagerStrategyManager.Object,
                    mockMapper.Object
                )

            let! response = userServiceController.GetAllMeAsync("dummy-id", Pager())

            mockUserServiceRepository
                .Verify((fun x -> x.CountAsync(It.IsAny<FilterDefinition<UserService>>())), Times.Once)
            mockUserServiceRepository
                .Verify((fun x ->
                    x.GetAllAsync(
                        It.IsAny<FilterDefinition<UserService>>(),
                        It.IsAny<int>(),
                        It.IsAny<int>())), Times.Once)
            mockPagerStrategyManager
                .Verify((fun x ->
                    x.GetPages(
                        It.IsAny<IEnumerable<UserService>>(),
                        It.IsAny<int64>(),
                        It.IsAny<int>(),
                        It.IsAny<int>())), Times.Once)

            Assert.IsType<OkObjectResult>(response) |> ignore
        }

    [<Fact(DisplayName = "Should return 404 when user service is not found")>]
    member __.GetByIdAsyncShouldReturn404(): Async<unit> =
        async {
            let mockUserServiceManager = Mock<IUserServiceManager>()
            let mockUserServiceRepository = Mock<IUserServiceRepository>()
            let mockServiceRepository = Mock<IServiceRepository>()
            let mockTotalCalulatorStrategyManager = Mock<ITotalCalculator>()
            let mockPagerStrategyManager = Mock<IPageable<UserService>>()
            let mockMapper = Mock<IMapper>()

            mockUserServiceRepository
                .Setup(fun x -> x.GetOneAsync(It.IsAny<FilterDefinition<UserService>>()))
                .ReturnsAsync(fun () -> null) |> ignore

            let userServiceController =
                UserServiceController(
                    mockUserServiceManager.Object,
                    mockUserServiceRepository.Object,
                    mockServiceRepository.Object,
                    mockTotalCalulatorStrategyManager.Object,
                    mockPagerStrategyManager.Object,
                    mockMapper.Object
                )

            let! response = userServiceController.GetByIdAsync "dummy-id"

            mockUserServiceRepository
                .Verify((fun x -> x.GetOneAsync(It.IsAny<FilterDefinition<UserService>>())), Times.Once)

            Assert.IsType<NotFoundObjectResult>(response) |> ignore
        }

    [<Fact(DisplayName = "Should return 200 when user service exist")>]
    member __.GetByIdAsyncShouldReturn200(): Async<unit> =
        async {
            let mockUserServiceManager = Mock<IUserServiceManager>()
            let mockUserServiceRepository = Mock<IUserServiceRepository>()
            let mockServiceRepository = Mock<IServiceRepository>()
            let mockTotalCalulatorStrategyManager = Mock<ITotalCalculator>()
            let mockPagerStrategyManager = Mock<IPageable<UserService>>()
            let mockMapper = Mock<IMapper>()

            mockUserServiceRepository
                .Setup(fun x -> x.GetOneAsync(It.IsAny<FilterDefinition<UserService>>()))
                .ReturnsAsync(UserService()) |> ignore
            mockMapper
                .Setup(fun x -> x.Map<UserServiceDetail>(It.IsAny<UserService>()))
                .Returns(UserServiceDetail()) |> ignore
            mockMapper
                .Setup(fun x -> x.Map<ServiceDetail[]>(It.IsAny<ICollection<Service>>()))
                .Returns([| ServiceDetail() |]) |> ignore
            mockServiceRepository
                .Setup(fun x -> x.GetAllAsync(It.IsAny<FilterDefinition<Service>>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(async {
                    let services = [| Service() |]
                    return services :> ICollection<Service>
                }) |> ignore

            let userServiceController =
                UserServiceController(
                    mockUserServiceManager.Object,
                    mockUserServiceRepository.Object,
                    mockServiceRepository.Object,
                    mockTotalCalulatorStrategyManager.Object,
                    mockPagerStrategyManager.Object,
                    mockMapper.Object
                )

            let! response = userServiceController.GetByIdAsync "dummy-id"

            mockUserServiceRepository
                .Verify((fun x -> x.GetOneAsync(It.IsAny<FilterDefinition<UserService>>())), Times.Once)
            mockMapper
                .Verify((fun x -> x.Map<UserServiceDetail>(It.IsAny<UserService>())), Times.Once)
            mockMapper
                .Verify((fun x -> x.Map<ServiceDetail[]>(It.IsAny<ICollection<Service>>())), Times.Once)
            mockServiceRepository
                .Verify((fun x ->
                    x.GetAllAsync(
                        It.IsAny<FilterDefinition<Service>>(),
                        It.IsAny<int>(),
                        It.IsAny<int>())), Times.Once)

            Assert.IsType<OkObjectResult>(response) |> ignore
        }

    [<Fact(DisplayName = "Should return 404 when user service does not exist for specific user")>]
    member __.GetByIdMeAsyncShouldReturn404(): Async<unit> =
        async {
            let mockUserServiceManager = Mock<IUserServiceManager>()
            let mockUserServiceRepository = Mock<IUserServiceRepository>()
            let mockServiceRepository = Mock<IServiceRepository>()
            let mockTotalCalulatorStrategyManager = Mock<ITotalCalculator>()
            let mockPagerStrategyManager = Mock<IPageable<UserService>>()
            let mockMapper = Mock<IMapper>()

            mockUserServiceRepository
                .Setup(fun x -> x.GetOneAsync(It.IsAny<FilterDefinition<UserService>>()))
                .ReturnsAsync(fun () -> null) |> ignore

            let userServiceController =
                UserServiceController(
                    mockUserServiceManager.Object,
                    mockUserServiceRepository.Object,
                    mockServiceRepository.Object,
                    mockTotalCalulatorStrategyManager.Object,
                    mockPagerStrategyManager.Object,
                    mockMapper.Object
                )

            let! response = userServiceController.GetByIdMeAsync("dummy-id", "dummy-id-2")

            mockUserServiceRepository
                .Verify((fun x -> x.GetOneAsync(It.IsAny<FilterDefinition<UserService>>())), Times.Once)

            Assert.IsType<NotFoundObjectResult>(response) |> ignore
        }

    [<Fact(DisplayName = "Should return 200 when user service is successfully found for a particular user")>]
    member __.GetByIdMeAsyncShouldReturn200(): Async<unit> =
        async {
            let mockUserServiceManager = Mock<IUserServiceManager>()
            let mockUserServiceRepository = Mock<IUserServiceRepository>()
            let mockServiceRepository = Mock<IServiceRepository>()
            let mockTotalCalulatorStrategyManager = Mock<ITotalCalculator>()
            let mockPagerStrategyManager = Mock<IPageable<UserService>>()
            let mockMapper = Mock<IMapper>()

            mockUserServiceRepository
                .Setup(fun x -> x.GetOneAsync(It.IsAny<FilterDefinition<UserService>>()))
                .ReturnsAsync(UserService()) |> ignore
            mockMapper
                .Setup(fun x -> x.Map<UserServiceDetail>(It.IsAny<UserService>()))
                .Returns(UserServiceDetail()) |> ignore
            mockMapper
                .Setup(fun x -> x.Map<ServiceDetail[]>(It.IsAny<ICollection<Service>>()))
                .Returns([| ServiceDetail() |]) |> ignore
            mockServiceRepository
                .Setup(fun x -> x.GetAllAsync(It.IsAny<FilterDefinition<Service>>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(async {
                    let services = [| Service() |]
                    return services :> ICollection<Service>
                }) |> ignore

            let userServiceController =
                UserServiceController(
                    mockUserServiceManager.Object,
                    mockUserServiceRepository.Object,
                    mockServiceRepository.Object,
                    mockTotalCalulatorStrategyManager.Object,
                    mockPagerStrategyManager.Object,
                    mockMapper.Object
                )

            let! response = userServiceController.GetByIdMeAsync("dummy-id", "dummy-id-2")

            mockUserServiceRepository
                .Verify((fun x -> x.GetOneAsync(It.IsAny<FilterDefinition<UserService>>())), Times.Once)
            mockMapper
                .Verify((fun x -> x.Map<UserServiceDetail>(It.IsAny<UserService>())), Times.Once)
            mockMapper
                .Verify((fun x -> x.Map<ServiceDetail[]>(It.IsAny<ICollection<Service>>())), Times.Once)
            mockServiceRepository
                .Verify((fun x ->
                    x.GetAllAsync(
                        It.IsAny<FilterDefinition<Service>>(),
                        It.IsAny<int>(),
                        It.IsAny<int>())), Times.Once)

            Assert.IsType<OkObjectResult>(response) |> ignore
        }

    [<Fact(DisplayName = "Should return 201 when user service is created successfully")>]
    member __.CreateAsyncShouldReturn201(): Async<unit> =
        async {
            let mockUserServiceManager = Mock<IUserServiceManager>()
            let mockUserServiceRepository = Mock<IUserServiceRepository>()
            let mockServiceRepository = Mock<IServiceRepository>()
            let mockTotalCalulatorStrategyManager = Mock<ITotalCalculator>()
            let mockPagerStrategyManager = Mock<IPageable<UserService>>()
            let mockMapper = Mock<IMapper>()

            mockTotalCalulatorStrategyManager
                .Setup(fun x -> x.CalculateTotalAsync(It.IsAny<string array>()))
                .Returns(async { return 10M }) |> ignore
            mockUserServiceManager
                .Setup(fun x -> x.CreateAsync(It.IsAny<UserService>()))
                .Returns(Task.FromResult(true)) |> ignore

            let userServiceController =
                UserServiceController(
                    mockUserServiceManager.Object,
                    mockUserServiceRepository.Object,
                    mockServiceRepository.Object,
                    mockTotalCalulatorStrategyManager.Object,
                    mockPagerStrategyManager.Object,
                    mockMapper.Object
                )

            let! response = userServiceController.CreateAsync(UserService())

            mockTotalCalulatorStrategyManager
                .Verify((fun x -> x.CalculateTotalAsync(It.IsAny<string array>())), Times.Once)
            mockUserServiceManager
                .Verify((fun x -> x.CreateAsync(It.IsAny<UserService>())), Times.Once)

            Assert.IsType<CreatedResult>(response) |> ignore
        }

    [<Fact(DisplayName = "Should return 201 when a user service is created successfully for specific user")>]
    member __.CreateMeAsyncShouldReturn201(): Async<unit> =
        async {
            let mockUserServiceManager = Mock<IUserServiceManager>()
            let mockUserServiceRepository = Mock<IUserServiceRepository>()
            let mockServiceRepository = Mock<IServiceRepository>()
            let mockTotalCalulatorStrategyManager = Mock<ITotalCalculator>()
            let mockPagerStrategyManager = Mock<IPageable<UserService>>()
            let mockMapper = Mock<IMapper>()

            mockTotalCalulatorStrategyManager
                .Setup(fun x -> x.CalculateTotalAsync(It.IsAny<string array>()))
                .Returns(async { return 10M }) |> ignore
            mockUserServiceManager
                .Setup(fun x -> x.CreateAsync(It.IsAny<UserService>()))
                .Returns(Task.FromResult(true)) |> ignore

            let userServiceController =
                UserServiceController(
                    mockUserServiceManager.Object,
                    mockUserServiceRepository.Object,
                    mockServiceRepository.Object,
                    mockTotalCalulatorStrategyManager.Object,
                    mockPagerStrategyManager.Object,
                    mockMapper.Object
                )

            let! response = userServiceController.CreateMeAsync("dummy-id", UserService())

            mockTotalCalulatorStrategyManager
                .Verify((fun x -> x.CalculateTotalAsync(It.IsAny<string array>())), Times.Once)
            mockUserServiceManager
                .Verify((fun x -> x.CreateAsync(It.IsAny<UserService>())), Times.Once)

            Assert.IsType<CreatedResult>(response) |> ignore
        }

    [<Fact(DisplayName = "Should return 404 when user service does not exist")>]
    member __.AssignEmployeeAsyncShouldReturn404(): Async<unit> =
        async {
            let mockUserServiceManager = Mock<IUserServiceManager>()
            let mockUserServiceRepository = Mock<IUserServiceRepository>()
            let mockServiceRepository = Mock<IServiceRepository>()
            let mockTotalCalulatorStrategyManager = Mock<ITotalCalculator>()
            let mockPagerStrategyManager = Mock<IPageable<UserService>>()
            let mockMapper = Mock<IMapper>()

            mockUserServiceRepository
                .Setup(fun x -> x.GetOneAsync(It.IsAny<FilterDefinition<UserService>>()))
                .ReturnsAsync(fun () -> null) |> ignore

            let userServiceController =
                UserServiceController(
                    mockUserServiceManager.Object,
                    mockUserServiceRepository.Object,
                    mockServiceRepository.Object,
                    mockTotalCalulatorStrategyManager.Object,
                    mockPagerStrategyManager.Object,
                    mockMapper.Object
                )

            let! response = userServiceController.AssignEmployeeAsync("dummy-id", UserService())

            mockUserServiceRepository
                .Verify((fun x -> x.GetOneAsync(It.IsAny<FilterDefinition<UserService>>())), Times.Once)

            Assert.IsType<NotFoundObjectResult>(response) |> ignore
        }

    [<Fact(DisplayName = "Should return 200 an employee is assigned successfully to a user service")>]
    member __.AssignEmployeeAsyncShouldReturn200(): Async<unit> =
        async {
            let mockUserServiceManager = Mock<IUserServiceManager>()
            let mockUserServiceRepository = Mock<IUserServiceRepository>()
            let mockServiceRepository = Mock<IServiceRepository>()
            let mockTotalCalulatorStrategyManager = Mock<ITotalCalculator>()
            let mockPagerStrategyManager = Mock<IPageable<UserService>>()
            let mockMapper = Mock<IMapper>()

            mockUserServiceRepository
                .Setup(fun x -> x.GetOneAsync(It.IsAny<FilterDefinition<UserService>>()))
                .ReturnsAsync(UserService()) |> ignore

            let userServiceController =
                UserServiceController(
                    mockUserServiceManager.Object,
                    mockUserServiceRepository.Object,
                    mockServiceRepository.Object,
                    mockTotalCalulatorStrategyManager.Object,
                    mockPagerStrategyManager.Object,
                    mockMapper.Object
                )

            let! response = userServiceController.AssignEmployeeAsync("dummy-id", UserService())

            mockUserServiceRepository
                .Verify((fun x -> x.GetOneAsync(It.IsAny<FilterDefinition<UserService>>())), Times.Once)

            Assert.IsType<OkObjectResult>(response) |> ignore
        }

    [<Fact(DisplayName = "Should return 404 when user service does not exist")>]
    member __.UpdateByIdAsyncShouldReturn404(): Async<unit> =
        async {
            let mockUserServiceManager = Mock<IUserServiceManager>()
            let mockUserServiceRepository = Mock<IUserServiceRepository>()
            let mockServiceRepository = Mock<IServiceRepository>()
            let mockTotalCalulatorStrategyManager = Mock<ITotalCalculator>()
            let mockPagerStrategyManager = Mock<IPageable<UserService>>()
            let mockMapper = Mock<IMapper>()

            mockUserServiceRepository
                .Setup(fun x -> x.GetOneAsync(It.IsAny<FilterDefinition<UserService>>()))
                .ReturnsAsync(fun () -> null) |> ignore

            let userServiceController =
                UserServiceController(
                    mockUserServiceManager.Object,
                    mockUserServiceRepository.Object,
                    mockServiceRepository.Object,
                    mockTotalCalulatorStrategyManager.Object,
                    mockPagerStrategyManager.Object,
                    mockMapper.Object
                )

            let! response =
                userServiceController.UpdateByIdAsync(
                    "dummy-user-id",
                    "dummy-service-id",PatchUserService()
                )

            mockUserServiceRepository
                .Verify((fun x -> x.GetOneAsync(It.IsAny<FilterDefinition<UserService>>())), Times.Once)

            Assert.IsType<NotFoundObjectResult>(response) |> ignore
        }

    [<Fact(DisplayName = "Should return 409 when user service has the canceled status")>]
    member __.UpdateByIdAsyncShouldReturn409ByCanceledStatus(): Async<unit> =
        async {
            let mockUserServiceManager = Mock<IUserServiceManager>()
            let mockUserServiceRepository = Mock<IUserServiceRepository>()
            let mockServiceRepository = Mock<IServiceRepository>()
            let mockTotalCalulatorStrategyManager = Mock<ITotalCalculator>()
            let mockPagerStrategyManager = Mock<IPageable<UserService>>()
            let mockMapper = Mock<IMapper>()

            let dummyUserService = UserService()
            dummyUserService.Status <- UserServiceStatus.Canceled

            mockUserServiceRepository
                .Setup(fun x -> x.GetOneAsync(It.IsAny<FilterDefinition<UserService>>()))
                .ReturnsAsync(dummyUserService) |> ignore

            let userServiceController =
                UserServiceController(
                    mockUserServiceManager.Object,
                    mockUserServiceRepository.Object,
                    mockServiceRepository.Object,
                    mockTotalCalulatorStrategyManager.Object,
                    mockPagerStrategyManager.Object,
                    mockMapper.Object
                )

            let! response =
                userServiceController.UpdateByIdAsync(
                    "dummy-user-id",
                    "dummy-service-id",PatchUserService()
                )

            mockUserServiceRepository
                .Verify((fun x -> x.GetOneAsync(It.IsAny<FilterDefinition<UserService>>())), Times.Once)

            Assert.IsType<ConflictObjectResult>(response) |> ignore
        }

    [<Fact(DisplayName = "Should return 409 when user service has the charged status")>]
    member __.UpdateByIdAsyncShouldReturn409ByChargedStatus(): Async<unit> =
        async {
            let mockUserServiceManager = Mock<IUserServiceManager>()
            let mockUserServiceRepository = Mock<IUserServiceRepository>()
            let mockServiceRepository = Mock<IServiceRepository>()
            let mockTotalCalulatorStrategyManager = Mock<ITotalCalculator>()
            let mockPagerStrategyManager = Mock<IPageable<UserService>>()
            let mockMapper = Mock<IMapper>()

            let dummyUserService = UserService()
            dummyUserService.Status <- UserServiceStatus.Charged

            mockUserServiceRepository
                .Setup(fun x -> x.GetOneAsync(It.IsAny<FilterDefinition<UserService>>()))
                .ReturnsAsync(dummyUserService) |> ignore

            let userServiceController =
                UserServiceController(
                    mockUserServiceManager.Object,
                    mockUserServiceRepository.Object,
                    mockServiceRepository.Object,
                    mockTotalCalulatorStrategyManager.Object,
                    mockPagerStrategyManager.Object,
                    mockMapper.Object
                )

            let! response =
                userServiceController.UpdateByIdAsync(
                    "dummy-user-id",
                    "dummy-service-id",PatchUserService()
                )

            mockUserServiceRepository
                .Verify((fun x -> x.GetOneAsync(It.IsAny<FilterDefinition<UserService>>())), Times.Once)

            Assert.IsType<ConflictObjectResult>(response) |> ignore
        }

    [<Fact(DisplayName = "Should return 200 when user service is updated successfully")>]
    member __.UpdateByIdAsyncShouldRetur200(): Async<unit> =
        async {
            let mockUserServiceManager = Mock<IUserServiceManager>()
            let mockUserServiceRepository = Mock<IUserServiceRepository>()
            let mockServiceRepository = Mock<IServiceRepository>()
            let mockTotalCalulatorStrategyManager = Mock<ITotalCalculator>()
            let mockPagerStrategyManager = Mock<IPageable<UserService>>()
            let mockMapper = Mock<IMapper>()

            let dummyUserService = UserService()
            dummyUserService.Status <- UserServiceStatus.Created

            mockUserServiceRepository
                .Setup(fun x -> x.GetOneAsync(It.IsAny<FilterDefinition<UserService>>()))
                .ReturnsAsync(dummyUserService) |> ignore
            mockUserServiceManager
                .Setup(fun x -> x.UpdateAsync(It.IsAny<FilterDefinition<UserService>>(), It.IsAny<UserService>()))
                .Returns(Task.CompletedTask) |> ignore

            let userServiceController =
                UserServiceController(
                    mockUserServiceManager.Object,
                    mockUserServiceRepository.Object,
                    mockServiceRepository.Object,
                    mockTotalCalulatorStrategyManager.Object,
                    mockPagerStrategyManager.Object,
                    mockMapper.Object
                )

            let! response =
                userServiceController.UpdateByIdAsync(
                    "dummy-user-id",
                    "dummy-service-id",PatchUserService()
                )

            mockUserServiceRepository
                .Verify((fun x -> x.GetOneAsync(It.IsAny<FilterDefinition<UserService>>())), Times.Once)
            mockUserServiceManager
                .Verify((fun x ->
                    x.UpdateAsync(
                        It.IsAny<FilterDefinition<UserService>>(),
                        It.IsAny<UserService>())), Times.Once)

            Assert.IsType<OkObjectResult>(response) |> ignore
        }


    [<Fact(DisplayName = "Should return 404 when user service does not exist")>]
    member __.CancelAsyncShouldReturn404(): Async<unit> =
        async {
            let mockUserServiceManager = Mock<IUserServiceManager>()
            let mockUserServiceRepository = Mock<IUserServiceRepository>()
            let mockServiceRepository = Mock<IServiceRepository>()
            let mockTotalCalulatorStrategyManager = Mock<ITotalCalculator>()
            let mockPagerStrategyManager = Mock<IPageable<UserService>>()
            let mockMapper = Mock<IMapper>()

            mockUserServiceRepository
                .Setup(fun x -> x.GetOneAsync(It.IsAny<FilterDefinition<UserService>>()))
                .ReturnsAsync(fun () -> null) |> ignore

            let userServiceController =
                UserServiceController(
                    mockUserServiceManager.Object,
                    mockUserServiceRepository.Object,
                    mockServiceRepository.Object,
                    mockTotalCalulatorStrategyManager.Object,
                    mockPagerStrategyManager.Object,
                    mockMapper.Object
                )

            let! response = userServiceController.CancelAsync("dummy-user-id", "dummy-service-id")

            mockUserServiceRepository
                .Verify((fun x -> x.GetOneAsync(It.IsAny<FilterDefinition<UserService>>())), Times.Once)

            Assert.IsType<NotFoundObjectResult>(response) |> ignore
        }

    [<Fact(DisplayName = "Should return 409 when user service has the canceled status")>]
    member __.CancelAsyncShouldReturn409ByCanceledStatus(): Async<unit> =
        async {
            let mockUserServiceManager = Mock<IUserServiceManager>()
            let mockUserServiceRepository = Mock<IUserServiceRepository>()
            let mockServiceRepository = Mock<IServiceRepository>()
            let mockTotalCalulatorStrategyManager = Mock<ITotalCalculator>()
            let mockPagerStrategyManager = Mock<IPageable<UserService>>()
            let mockMapper = Mock<IMapper>()

            let dummyUserService = UserService()
            dummyUserService.Status <- UserServiceStatus.Canceled

            mockUserServiceRepository
                .Setup(fun x -> x.GetOneAsync(It.IsAny<FilterDefinition<UserService>>()))
                .ReturnsAsync(dummyUserService) |> ignore

            let userServiceController =
                UserServiceController(
                    mockUserServiceManager.Object,
                    mockUserServiceRepository.Object,
                    mockServiceRepository.Object,
                    mockTotalCalulatorStrategyManager.Object,
                    mockPagerStrategyManager.Object,
                    mockMapper.Object
                )

            let! response = userServiceController.CancelAsync("dummy-user-id", "dummy-service-id")

            mockUserServiceRepository
                .Verify((fun x -> x.GetOneAsync(It.IsAny<FilterDefinition<UserService>>())), Times.Once)

            Assert.IsType<ConflictObjectResult>(response) |> ignore
        }

    [<Fact(DisplayName = "Should return 409 when user service has the charged status")>]
    member __.CancelAsyncShouldReturn409ByChargedStatus(): Async<unit> =
        async {
            let mockUserServiceManager = Mock<IUserServiceManager>()
            let mockUserServiceRepository = Mock<IUserServiceRepository>()
            let mockServiceRepository = Mock<IServiceRepository>()
            let mockTotalCalulatorStrategyManager = Mock<ITotalCalculator>()
            let mockPagerStrategyManager = Mock<IPageable<UserService>>()
            let mockMapper = Mock<IMapper>()

            let dummyUserService = UserService()
            dummyUserService.Status <- UserServiceStatus.Charged

            mockUserServiceRepository
                .Setup(fun x -> x.GetOneAsync(It.IsAny<FilterDefinition<UserService>>()))
                .ReturnsAsync(dummyUserService) |> ignore

            let userServiceController =
                UserServiceController(
                    mockUserServiceManager.Object,
                    mockUserServiceRepository.Object,
                    mockServiceRepository.Object,
                    mockTotalCalulatorStrategyManager.Object,
                    mockPagerStrategyManager.Object,
                    mockMapper.Object
                )

            let! response = userServiceController.CancelAsync("dummy-user-id", "dummy-service-id")

            mockUserServiceRepository
                .Verify((fun x -> x.GetOneAsync(It.IsAny<FilterDefinition<UserService>>())), Times.Once)

            Assert.IsType<ConflictObjectResult>(response) |> ignore
        }

    [<Fact(DisplayName = "Should return 200 when user service is canceled successfully")>]
    member __.CancelAsyncShouldReturn200(): Async<unit> =
        async {
            let mockUserServiceManager = Mock<IUserServiceManager>()
            let mockUserServiceRepository = Mock<IUserServiceRepository>()
            let mockServiceRepository = Mock<IServiceRepository>()
            let mockTotalCalulatorStrategyManager = Mock<ITotalCalculator>()
            let mockPagerStrategyManager = Mock<IPageable<UserService>>()
            let mockMapper = Mock<IMapper>()

            let dummyUserService = UserService()
            dummyUserService.Status <- UserServiceStatus.Created

            mockUserServiceRepository
                .Setup(fun x -> x.GetOneAsync(It.IsAny<FilterDefinition<UserService>>()))
                .ReturnsAsync(dummyUserService) |> ignore
            mockUserServiceManager
                .Setup(fun x -> x.UpdateAsync(It.IsAny<FilterDefinition<UserService>>(), It.IsAny<UserService>()))
                .Returns(Task.CompletedTask) |> ignore

            let userServiceController =
                UserServiceController(
                    mockUserServiceManager.Object,
                    mockUserServiceRepository.Object,
                    mockServiceRepository.Object,
                    mockTotalCalulatorStrategyManager.Object,
                    mockPagerStrategyManager.Object,
                    mockMapper.Object
                )

            let! response = userServiceController.CancelAsync("dummy-user-id", "dummy-service-id")

            mockUserServiceRepository
                .Verify((fun x -> x.GetOneAsync(It.IsAny<FilterDefinition<UserService>>())), Times.Once)
            mockUserServiceManager
                .Verify((fun x ->
                    x.UpdateAsync(
                        It.IsAny<FilterDefinition<UserService>>(),
                        It.IsAny<UserService>())), Times.Once)

            Assert.IsType<OkObjectResult>(response) |> ignore
        }
