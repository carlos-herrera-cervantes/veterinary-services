namespace VeterinaryServices.Web.Controllers

open Microsoft.AspNetCore.Mvc
open MongoDB.Driver
open VeterinaryServices.Domain.Models
open VeterinaryServices.Repository.Repositories
open VeterinaryServices.Repository.Managers
open VeterinaryServices.Services.Calculators
open VeterinaryServices.Services.Pagers
open VeterinaryServices.Web.Types

[<Route("api/services/v1/costs")>]
[<Produces("application/json")>]
[<Consumes("application/json")>]
[<ApiController>]
type ServiceController
    (
        serviceRepository: IServiceRepository,
        serviceManager: IServiceManager,
        totalCalulatorStrategyManager: IStrategyManager,
        pagerStrategyManager: IPagerStrategyManager<Service>
    ) =
    inherit ControllerBase()

    member private this._serviceRepository = serviceRepository

    member private this._serviceManager = serviceManager

    member private this._totalCalculatorStrategyManager = totalCalulatorStrategyManager

    member private this._pagerStrategyManager = pagerStrategyManager

    [<HttpGet>]
    member this.GetAllAsync([<FromQuery>] pager: Pager) =
        async {
            let filter = Builders<Service>.Filter.Empty
            let! totalDocs = this._serviceRepository.CountAsync filter |> Async.AwaitTask
            let! docs = this._serviceRepository.GetAllAsync(filter, pager.Page, pager.PageSize)
            let pages = this._pagerStrategyManager.GetPager("classic", docs, totalDocs, pager.Page, pager.PageSize)

            return this.Ok pages :> IActionResult
        }

    [<HttpGet("{id}")>]
    member this.GetByIdAsync([<FromRoute>] id: string) =
        async {
            let filter = Builders<Service>.Filter.Eq((fun s -> s.Id), id)
            let! service = this._serviceRepository.GetOneAsync filter |> Async.AwaitTask

            match service with
            | null ->
                let serviceNotFoundMessage = {| message = "Service not found" |}
                return NotFoundObjectResult(serviceNotFoundMessage) :> IActionResult
            | _ -> return this.Ok service :> IActionResult
        }

    [<HttpPost("total")>]
    member this.CalculateTotalAsync([<FromBody>] serviceTotal: ServiceTotal) =
        async {
            let! total = this._totalCalculatorStrategyManager.RunJobAsync("classic", serviceTotal.Services)
            let serviceTotalMessage = {| total = total |}
            return this.Ok serviceTotalMessage :> IActionResult
        }

    [<HttpPost>]
    member this.CreateAsync([<FromBody>] service: Service) =
        async {
            do! this._serviceManager.CreateAsync service |> Async.AwaitTask
            return this.Created("", service) :> IActionResult
        }

    [<HttpPut("{id}")>]
    member this.UpdateByIdAsync([<FromRoute>] id: string, [<FromBody>] service: Service) =
        async {
            let filter = Builders<Service>.Filter.Eq((fun s -> s.Id), id)
            let! finded = this._serviceRepository.GetOneAsync filter |> Async.AwaitTask

            match finded with
            | null ->
                let serviceNotFoundMessage = {| message = "Service not found" |}
                return NotFoundObjectResult(serviceNotFoundMessage) :> IActionResult
            | _ ->
                let! _ = this._serviceManager.UpdateAsync(filter, service) |> Async.AwaitTask
                return this.Ok service :> IActionResult
        }

    [<HttpDelete("{id}")>]
    member this.DeleteByIdAsync([<FromRoute>] id: string) =
        async {
            let filter = Builders<Service>.Filter.Eq((fun s -> s.Id), id)
            let! service = this._serviceRepository.GetOneAsync filter |> Async.AwaitTask

            match service with
            | null ->
                let serviceNotFoundMessage = {| message = "Service not found" |}
                return NotFoundObjectResult(serviceNotFoundMessage) :> IActionResult
            | _ ->
                do! this._serviceManager.DeleteAsync filter |> Async.AwaitTask
                return this.NoContent() :> IActionResult
        }
