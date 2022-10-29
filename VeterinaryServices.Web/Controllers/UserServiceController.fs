namespace VeterinaryServices.Web.Controllers

open Microsoft.AspNetCore.Mvc
open MongoDB.Driver
open AutoMapper
open VeterinaryServices.Repository.Managers
open VeterinaryServices.Repository.Repositories
open VeterinaryServices.Services.Calculators
open VeterinaryServices.Services.Pagers
open VeterinaryServices.Web.Types
open VeterinaryServices.Domain.Models
open VeterinaryServices.Web.Attributes

[<Route("api/services/v1/customers")>]
[<Produces("application/json")>]
[<Consumes("application/json")>]
[<ApiController>]
type UserServiceController
    (
        userServiceManager: IUserServiceManager,
        userServiceRepository: IUserServiceRepository,
        serviceCatalogRepository: IServiceCatalogRepository,
        serviceRepository: IServiceRepository,
        totalCalulatorStrategyManager: IStrategyManager,
        pagerStrategyManager: IPagerStrategyManager<UserService>,
        mapper: IMapper
    ) =
    inherit ControllerBase()

    member private this._userServiceManager = userServiceManager

    member private this._userServiceRepository = userServiceRepository

    member private this._serviceCatalogRepository = serviceCatalogRepository

    member private this._serviceRepository = serviceRepository

    member private this._totalCalculatorStrategyManager = totalCalulatorStrategyManager

    member private this._pagerStrategyManager = pagerStrategyManager

    member private this._mapper = mapper

    [<HttpGet>]
    member this.GetAllAsync([<FromQuery>] pager: Pager) =
        async {
            let filter = Builders<UserService>.Filter.Empty
            let! totalDocs = this._userServiceRepository.CountAsync filter |> Async.AwaitTask
            let! docs = this._userServiceRepository.GetAllAsync(filter, pager.Page, pager.PageSize)
            let pages = this._pagerStrategyManager.GetPager("classic", docs, totalDocs, pager.Page, pager.PageSize)

            return this.Ok pages :> IActionResult
        }

    [<HttpGet("me")>]
    member this.GetAllMeAsync([<FromHeader(Name = "user-id")>] userId: string, [<FromQuery>] pager: Pager) =
        async {
            let filter = Builders<UserService>.Filter.Eq((fun us -> us.CustomerId), userId)
            let! totalDocs = this._userServiceRepository.CountAsync filter |> Async.AwaitTask
            let! docs = this._userServiceRepository.GetAllAsync(filter, pager.Page, pager.PageSize)
            let pages = this._pagerStrategyManager.GetPager("classic", docs, totalDocs, pager.Page, pager.PageSize)

            return this.Ok pages :> IActionResult
        }

    [<HttpGet("{id}")>]
    member this.GetByIdAsync([<FromRoute>] id: string) =
        async {
            let userServiceFilter = Builders<UserService>.Filter.Eq((fun us -> us.Id), id)
            let! userService = this._userServiceRepository.GetOneAsync userServiceFilter |> Async.AwaitTask

            match userService with
            | null ->
                let userServiceNotFoundMessage = {| message = "User service not found" |}
                return NotFoundObjectResult(userServiceNotFoundMessage) :> IActionResult
            | _ ->
                let finalService = this._mapper.Map<UserServiceDetail>(userService)

                let serviceFilter = Builders<Service>.Filter.In((fun s -> s.Id), userService.Services)
                let! services = this._serviceRepository.GetAllAsync(serviceFilter, 0, 10)

                if services.Count > 0 then finalService.ServiceDetails <- this._mapper.Map<ServiceDetail[]>(services)

                return this.Ok finalService :> IActionResult
        }

    [<HttpGet("me/{id}")>]
    member this.GetByIdMeAsync([<FromHeader(Name = "user-id")>] userId: string, [<FromRoute>] id: string) =
        async {
            let serviceIdMatch = Builders<UserService>.Filter.Eq((fun us -> us.Id), id)
            let userIdMatch = Builders<UserService>.Filter.Eq((fun us -> us.CustomerId), userId)
            let userServiceFilter = Builders<UserService>.Filter.And(serviceIdMatch, userIdMatch)

            let! userService = this._userServiceRepository.GetOneAsync userServiceFilter |> Async.AwaitTask

            match userService with
            | null ->
                let userServiceNotFoundMessage = {| message = "User service not found" |}
                return NotFoundObjectResult(userServiceNotFoundMessage) :> IActionResult
            | _ ->
                let finalService = this._mapper.Map<UserServiceDetail>(userService)

                let serviceFilter = Builders<Service>.Filter.In((fun s -> s.Id), userService.Services)
                let! services = this._serviceRepository.GetAllAsync(serviceFilter, 0, 10)

                if services.Count > 0 then finalService.ServiceDetails <- this._mapper.Map<ServiceDetail[]>(services)

                return this.Ok finalService :> IActionResult
        }

    [<HttpPost>]
    [<CustomerExist>]
    [<PetExist>]
    [<EmployeeExist>]
    [<ServiceExist>]
    member this.CreateAsync([<FromBody>] userService: UserService) =
        async {
            let! total = this._totalCalculatorStrategyManager.RunJobAsync("classic", userService.Services)
            userService.TotalCost <- total
            do! this._userServiceManager.CreateAsync userService |> Async.AwaitTask
            return this.Created("", userService) :> IActionResult
        }

    [<HttpPost("me")>]
    [<PetExist>]
    [<EmployeeExist>]
    [<ServiceExist>]
    member this.CreateMeAsync
        (
            [<FromHeader(Name = "user-id")>] userId: string,
            [<FromBody>] userService: UserService
        ) =
        async {
            let! total = this._totalCalculatorStrategyManager.RunJobAsync("classic", userService.Services)
            userService.TotalCost <- total
            userService.CustomerId <- userId
            do! this._userServiceManager.CreateAsync userService |> Async.AwaitTask
            return this.Created("", userService) :> IActionResult
        }

    [<HttpPut("{id}/employee")>]
    [<EmployeeExist>]
    member this.AssignEmployeeAsync([<FromRoute>] id: string, [<FromBody>] userService: UserService) =
        async {
            let filter = Builders<UserService>.Filter.Eq((fun us -> us.Id), id)
            let! finded = this._userServiceRepository.GetOneAsync filter |> Async.AwaitTask

            match finded with
            | null ->
                let userServiceNotFoundMessage = {| message = "User service not found" |}
                return NotFoundObjectResult(userServiceNotFoundMessage) :> IActionResult
            | _ ->
                do! this._userServiceManager.UpdateAsync(filter, userService) |> Async.AwaitTask
                return this.Ok userService :> IActionResult
        }
