namespace VeterinaryServices.Web.Controllers

open Microsoft.AspNetCore.Mvc
open MongoDB.Driver
open VeterinaryServices.Domain.Models
open VeterinaryServices.Repository.Repositories
open VeterinaryServices.Repository.Managers
open VeterinaryServices.Services.Calculators
open VeterinaryServices.Services.Pagers
open VeterinaryServices.Web.Types
open VeterinaryServices.Web.Config

[<Route(ApiConfig.BasePath + "/v1/costs")>]
[<Produces("application/json")>]
[<Consumes("application/json")>]
[<ApiController>]
type ServiceController
    (
        serviceRepository: IServiceRepository,
        serviceManager: IServiceManager,
        calculatorService: ITotalCalculator,
        pageableService: IPageable<Service>
    ) =
    inherit ControllerBase()

    [<HttpGet>]
    member __.GetAllAsync([<FromQuery>] pager: Pager): Async<IActionResult> =
        async {
            let filter = Builders<Service>.Filter.Empty
            let! totalDocs = serviceRepository.CountAsync filter |> Async.AwaitTask
            let! docs = serviceRepository.GetAllAsync(filter, pager.Page, pager.PageSize)
            let pages = pageableService.GetPages(docs, totalDocs, pager.Page, pager.PageSize)

            return __.Ok pages :> IActionResult
        }

    [<HttpGet("{id}")>]
    member __.GetByIdAsync([<FromRoute>] id: string): Async<IActionResult> =
        async {
            let filter = Builders<Service>.Filter.Eq((fun s -> s.Id), id)
            let! service = serviceRepository.GetOneAsync filter |> Async.AwaitTask

            match service with
            | null ->
                let serviceNotFoundMessage = {| message = "Service not found" |}
                return NotFoundObjectResult(serviceNotFoundMessage) :> IActionResult
            | _ -> return __.Ok service :> IActionResult
        }

    [<HttpPost("total")>]
    member __.CalculateTotalAsync([<FromBody>] serviceTotal: ServiceTotal): Async<IActionResult> =
        async {
            let! total = calculatorService.CalculateTotalAsync(serviceTotal.Services)
            let serviceTotalMessage = {| total = total |}
            return __.Ok serviceTotalMessage :> IActionResult
        }

    [<HttpPost>]
    member __.CreateAsync([<FromBody>] service: Service): Async<IActionResult> =
        async {
            do! serviceManager.CreateAsync service |> Async.AwaitTask
            return __.Created("", service) :> IActionResult
        }

    [<HttpPut("{id}")>]
    member __.UpdateByIdAsync([<FromRoute>] id: string, [<FromBody>] service: Service): Async<IActionResult> =
        async {
            let filter = Builders<Service>.Filter.Eq((fun s -> s.Id), id)
            let! finded = serviceRepository.GetOneAsync filter |> Async.AwaitTask

            match finded with
            | null ->
                let serviceNotFoundMessage = {| message = "Service not found" |}
                return NotFoundObjectResult(serviceNotFoundMessage) :> IActionResult
            | _ ->
                do! serviceManager.UpdateAsync(filter, service) |> Async.AwaitTask
                return __.Ok service :> IActionResult
        }

    [<HttpDelete("{id}")>]
    member __.DeleteByIdAsync([<FromRoute>] id: string): Async<IActionResult> =
        async {
            let filter = Builders<Service>.Filter.Eq((fun s -> s.Id), id)
            let! service = serviceRepository.GetOneAsync filter |> Async.AwaitTask

            match service with
            | null ->
                let serviceNotFoundMessage = {| message = "Service not found" |}
                return NotFoundObjectResult(serviceNotFoundMessage) :> IActionResult
            | _ ->
                do! serviceManager.DeleteAsync filter |> Async.AwaitTask
                return __.NoContent() :> IActionResult
        }
