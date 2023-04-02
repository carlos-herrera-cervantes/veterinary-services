namespace VeterinaryServices.Web.Controllers

open Microsoft.AspNetCore.Mvc
open MongoDB.Driver
open VeterinaryServices.Repository.Managers
open VeterinaryServices.Repository.Repositories
open VeterinaryServices.Domain.Models
open VeterinaryServices.Web.Types
open VeterinaryServices.Services.Pagers
open VeterinaryServices.Web.Config

[<Route(ApiConfig.BasePath + "/v1/catalog")>]
[<Produces("application/json")>]
[<Consumes("application/json")>]
[<ApiController>]
type ServiceCatalogController
    (
        serviceCatalogManager: IServiceCatalogManager,
        serviceCatalogRepository: IServiceCatalogRepository,
        pagerStrategyManager: IPagerStrategyManager<ServiceCatalog>
    ) =
    inherit ControllerBase()

    member private this._serviceCatalogManager = serviceCatalogManager

    member private this._serviceCatalogRepository = serviceCatalogRepository

    member private this._pagerStrategyManager = pagerStrategyManager

    [<HttpGet>]
    member this.GetAllAsync([<FromQuery>] pager: Pager) =
        async {
            let filter = Builders<ServiceCatalog>.Filter.Empty
            let! totalDocs = this._serviceCatalogRepository.CountAsync filter |> Async.AwaitTask
            let! docs = this._serviceCatalogRepository.GetAllAsync(filter, pager.Page, pager.PageSize)
            let pages = this._pagerStrategyManager.GetPager("classic", docs, totalDocs, pager.Page, pager.PageSize)

            return this.Ok pages :> IActionResult
        }

    [<HttpGet("{id}")>]
    member this.GetByIdAsync([<FromRoute>] id: string) =
        async {
            let filter = Builders<ServiceCatalog>.Filter.Eq((fun sc -> sc.Id), id)
            let! serviceCatalog = this._serviceCatalogRepository.GetOneAsync filter |> Async.AwaitTask

            match serviceCatalog with
            | null -> return NotFoundResult() :> IActionResult
            | _ -> return this.Ok serviceCatalog :> IActionResult
        }

    [<HttpPost>]
    member this.CreateAsync([<FromBody>] serviceCatalog: ServiceCatalog) =
        async {
            let! _ = this._serviceCatalogManager.CreateAsync serviceCatalog |> Async.AwaitTask
            return this.Created("", serviceCatalog) :> IActionResult
        }

    [<HttpPut("{id}")>]
    member this.UpdateByIdAsync([<FromRoute>] id: string, [<FromBody>] serviceCatalog: ServiceCatalog) =
        async {
            let filter = Builders<ServiceCatalog>.Filter.Eq((fun sc -> sc.Id), id)
            let! finded = this._serviceCatalogRepository.GetOneAsync filter |> Async.AwaitTask

            match finded with
            | null -> return NotFoundResult() :> IActionResult
            | _ ->
                let! _ = this._serviceCatalogManager.UpdateAsync(filter, serviceCatalog) |> Async.AwaitTask
                return this.Ok serviceCatalog :> IActionResult
        }

    [<HttpDelete("{id}")>]
    member this.DeleteByIdAsync([<FromRoute>] id: string) =
        async {
            let filter = Builders<ServiceCatalog>.Filter.Eq((fun sc -> sc.Id), id)
            let! finded = this._serviceCatalogRepository.GetOneAsync filter |> Async.AwaitTask

            match finded with
            | null -> return NotFoundResult() :> IActionResult
            | _ ->
                do! this._serviceCatalogManager.DeleteAsync filter |> Async.AwaitTask
                return this.NoContent() :> IActionResult
        }
