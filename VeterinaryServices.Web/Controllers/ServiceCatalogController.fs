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
        pageableService: IPageable<ServiceCatalog>
    ) =
    inherit ControllerBase()

    [<HttpGet>]
    member __.GetAllAsync([<FromQuery>] pager: Pager): Async<IActionResult> =
        async {
            let filter = Builders<ServiceCatalog>.Filter.Empty
            let! totalDocs = serviceCatalogRepository.CountAsync filter |> Async.AwaitTask
            let! docs = serviceCatalogRepository.GetAllAsync(filter, pager.Page, pager.PageSize)
            let pages = pageableService.GetPages(docs, totalDocs, pager.Page, pager.PageSize)

            return __.Ok pages :> IActionResult
        }

    [<HttpGet("{id}")>]
    member __.GetByIdAsync([<FromRoute>] id: string): Async<IActionResult> =
        async {
            let filter = Builders<ServiceCatalog>.Filter.Eq((fun sc -> sc.Id), id)
            let! serviceCatalog = serviceCatalogRepository.GetOneAsync filter |> Async.AwaitTask

            match serviceCatalog with
            | null -> return NotFoundResult() :> IActionResult
            | _ -> return __.Ok serviceCatalog :> IActionResult
        }

    [<HttpPost>]
    member __.CreateAsync([<FromBody>] serviceCatalog: ServiceCatalog): Async<IActionResult> =
        async {
            let! _ = serviceCatalogManager.CreateAsync serviceCatalog |> Async.AwaitTask
            return __.Created("", serviceCatalog) :> IActionResult
        }

    [<HttpPut("{id}")>]
    member __.UpdateByIdAsync([<FromRoute>] id: string, [<FromBody>] serviceCatalog: ServiceCatalog): Async<IActionResult> =
        async {
            let filter = Builders<ServiceCatalog>.Filter.Eq((fun sc -> sc.Id), id)
            let! finded = serviceCatalogRepository.GetOneAsync filter |> Async.AwaitTask

            match finded with
            | null -> return NotFoundResult() :> IActionResult
            | _ ->
                let! _ = serviceCatalogManager.UpdateAsync(filter, serviceCatalog) |> Async.AwaitTask
                return __.Ok serviceCatalog :> IActionResult
        }

    [<HttpDelete("{id}")>]
    member __.DeleteByIdAsync([<FromRoute>] id: string): Async<IActionResult> =
        async {
            let filter = Builders<ServiceCatalog>.Filter.Eq((fun sc -> sc.Id), id)
            let! finded = serviceCatalogRepository.GetOneAsync filter |> Async.AwaitTask

            match finded with
            | null -> return NotFoundResult() :> IActionResult
            | _ ->
                do! serviceCatalogManager.DeleteAsync filter |> Async.AwaitTask
                return __.NoContent() :> IActionResult
        }
