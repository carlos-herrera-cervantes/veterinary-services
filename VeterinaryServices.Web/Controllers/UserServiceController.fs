namespace VeterinaryServices.Web.Controllers

open Microsoft.AspNetCore.Mvc
open MongoDB.Driver
open AutoMapper
open VeterinaryServices.Repository.Managers
open VeterinaryServices.Repository.Repositories
open VeterinaryServices.Services.Calculators
open VeterinaryServices.Services.Pagers
open VeterinaryServices.Web.Types
open VeterinaryServices.Web.Attributes
open VeterinaryServices.Domain.Models
open VeterinaryServices.Domain.Constants
open VeterinaryServices.Web.Config

[<Route(ApiConfig.BasePath + "/v1/customers")>]
[<Produces("application/json")>]
[<Consumes("application/json")>]
[<ApiController>]
type UserServiceController
    (
        userServiceManager: IUserServiceManager,
        userServiceRepository: IUserServiceRepository,
        serviceRepository: IServiceRepository,
        calculatorService: ITotalCalculator,
        pageableService: IPageable<UserService>,
        mapper: IMapper
    ) =
    inherit ControllerBase()

    [<HttpGet>]
    member __.GetAllAsync([<FromQuery>] pager: Pager): Async<IActionResult> =
        async {
            let filter = Builders<UserService>.Filter.Empty
            let! totalDocs = userServiceRepository.CountAsync filter |> Async.AwaitTask
            let! docs = userServiceRepository.GetAllAsync(filter, pager.Page, pager.PageSize)
            let pages = pageableService.GetPages(docs, totalDocs, pager.Page, pager.PageSize)

            return __.Ok pages :> IActionResult
        }

    [<HttpGet("me")>]
    member __.GetAllMeAsync([<FromHeader(Name = "user-id")>] userId: string, [<FromQuery>] pager: Pager): Async<IActionResult> =
        async {
            let filter = Builders<UserService>.Filter.Eq((fun us -> us.CustomerId), userId)
            let! totalDocs = userServiceRepository.CountAsync filter |> Async.AwaitTask
            let! docs = userServiceRepository.GetAllAsync(filter, pager.Page, pager.PageSize)
            let pages = pageableService.GetPages(docs, totalDocs, pager.Page, pager.PageSize)

            return __.Ok pages :> IActionResult
        }

    [<HttpGet("{id}")>]
    member __.GetByIdAsync([<FromRoute>] id: string): Async<IActionResult> =
        async {
            let userServiceFilter = Builders<UserService>.Filter.Eq((fun us -> us.Id), id)
            let! userService = userServiceRepository.GetOneAsync userServiceFilter |> Async.AwaitTask

            match userService with
            | null ->
                let userServiceNotFoundMessage = {| message = "User service not found" |}
                return NotFoundObjectResult(userServiceNotFoundMessage) :> IActionResult
            | _ ->
                let finalService = mapper.Map<UserServiceDetail>(userService)

                let serviceFilter = Builders<Service>.Filter.In((fun s -> s.Id), userService.Services)
                let! services = serviceRepository.GetAllAsync(serviceFilter, 0, 10)

                if services.Count > 0 then finalService.ServiceDetails <- mapper.Map<ServiceDetail[]>(services)

                return __.Ok finalService :> IActionResult
        }

    [<HttpGet("me/{id}")>]
    member __.GetByIdMeAsync([<FromHeader(Name = "user-id")>] userId: string, [<FromRoute>] id: string): Async<IActionResult> =
        async {
            let serviceIdMatch = Builders<UserService>.Filter.Eq((fun us -> us.Id), id)
            let userIdMatch = Builders<UserService>.Filter.Eq((fun us -> us.CustomerId), userId)
            let userServiceFilter = Builders<UserService>.Filter.And(serviceIdMatch, userIdMatch)

            let! userService = userServiceRepository.GetOneAsync userServiceFilter |> Async.AwaitTask

            match userService with
            | null ->
                let userServiceNotFoundMessage = {| message = "User service not found" |}
                return NotFoundObjectResult(userServiceNotFoundMessage) :> IActionResult
            | _ ->
                let finalService = mapper.Map<UserServiceDetail>(userService)

                let serviceFilter = Builders<Service>.Filter.In((fun s -> s.Id), userService.Services)
                let! services = serviceRepository.GetAllAsync(serviceFilter, 0, 10)

                if services.Count > 0 then finalService.ServiceDetails <- mapper.Map<ServiceDetail[]>(services)

                return __.Ok finalService :> IActionResult
        }

    [<HttpPost>]
    [<CustomerExist>]
    [<PetExist>]
    [<EmployeeExist>]
    [<ServiceExist>]
    member __.CreateAsync([<FromBody>] userService: UserService): Async<IActionResult> =
        async {
            let! total = calculatorService.CalculateTotalAsync(userService.Services)
            userService.TotalCost <- total
            do! userServiceManager.CreateAsync userService |> Async.AwaitTask
            return __.Created("", userService) :> IActionResult
        }

    [<HttpPost("me")>]
    [<PetExist>]
    [<EmployeeExist>]
    [<ServiceExist>]
    member __.CreateMeAsync
        (
            [<FromHeader(Name = "user-id")>] userId: string,
            [<FromBody>] userService: UserService
        ): Async<IActionResult> =
        async {
            let! total = calculatorService.CalculateTotalAsync(userService.Services)
            userService.TotalCost <- total
            userService.CustomerId <- userId
            do! userServiceManager.CreateAsync userService |> Async.AwaitTask
            return __.Created("", userService) :> IActionResult
        }

    [<HttpPut("me/{id}")>]
    [<ServiceExistFromPatch>]
    member __.UpdateByIdAsync
        (
            [<FromHeader(Name = "user-id")>] userId: string,
            [<FromRoute>] id: string,
            [<FromBody>] patchUserService: PatchUserService
        ): Async<IActionResult> =
        async {
            let serviceIdMatch = Builders<UserService>.Filter.Eq((fun us -> us.Id), id)
            let userIdMatch = Builders<UserService>.Filter.Eq((fun us -> us.CustomerId), userId)
            let userServiceFilter = Builders<UserService>.Filter.And(serviceIdMatch, userIdMatch)

            let! userService = userServiceRepository.GetOneAsync userServiceFilter |> Async.AwaitTask

            match userService with
            | null ->
                let userServiceNotFoundMessage = {| message = "User service not found" |}
                return NotFoundObjectResult(userServiceNotFoundMessage) :> IActionResult
            | _ ->
                match userService.Status with
                | UserServiceStatus.Canceled ->
                    let serviceAlreadyCanceledMessage = {| message = "The user service is already canceled" |}
                    return ConflictObjectResult(serviceAlreadyCanceledMessage) :> IActionResult
                | UserServiceStatus.Charged ->
                    let serviceChargedMessage = {| message = "The user service is already charged" |}
                    return ConflictObjectResult(serviceChargedMessage) :> IActionResult
                | _ ->
                    userService.Services <- patchUserService.Services
                    do! userServiceManager.UpdateAsync(userServiceFilter, userService) |> Async.AwaitTask
                    return __.Ok userService :> IActionResult
        }

    [<HttpPut("{id}/employee")>]
    [<EmployeeExist>]
    member __.AssignEmployeeAsync([<FromRoute>] id: string, [<FromBody>] userService: UserService): Async<IActionResult> =
        async {
            let filter = Builders<UserService>.Filter.Eq((fun us -> us.Id), id)
            let! finded = userServiceRepository.GetOneAsync filter |> Async.AwaitTask

            match finded with
            | null ->
                let userServiceNotFoundMessage = {| message = "User service not found" |}
                return NotFoundObjectResult(userServiceNotFoundMessage) :> IActionResult
            | _ ->
                do! userServiceManager.UpdateAsync(filter, userService) |> Async.AwaitTask
                return __.Ok userService :> IActionResult
        }

    [<HttpPut("me/{id}/cancel")>]
    member __.CancelAsync([<FromHeader(Name = "user-id")>] userId: string, [<FromRoute>] id: string): Async<IActionResult> =
        async {
            let serviceIdMatch = Builders<UserService>.Filter.Eq((fun us -> us.Id), id)
            let userIdMatch = Builders<UserService>.Filter.Eq((fun us -> us.CustomerId), userId)
            let userServiceFilter = Builders<UserService>.Filter.And(serviceIdMatch, userIdMatch)
            let! userService = userServiceRepository.GetOneAsync userServiceFilter |> Async.AwaitTask

            match userService with
            | null ->
                let userServiceNotFoundMessage = {| message = "User service not found" |}
                return NotFoundObjectResult(userServiceNotFoundMessage) :> IActionResult
            | _ ->
                match userService.Status with
                | UserServiceStatus.Canceled ->
                    let serviceAlreadyCanceledMessage = {| message = "The user service is already canceled" |}
                    return ConflictObjectResult(serviceAlreadyCanceledMessage) :> IActionResult
                | UserServiceStatus.Charged ->
                    let serviceChargedMessage = {| message = "The user service is already charged" |}
                    return ConflictObjectResult(serviceChargedMessage) :> IActionResult
                | _ ->
                    userService.Status <- UserServiceStatus.Canceled
                    do! userServiceManager.UpdateAsync(userServiceFilter, userService) |> Async.AwaitTask
                    return __.Ok userService :> IActionResult
        }
