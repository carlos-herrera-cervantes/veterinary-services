namespace VeterinaryServices.Web.Controllers

open Microsoft.AspNetCore.Mvc

[<Route("api/v1/catalog")>]
[<Produces("application/json")>]
[<Consumes("application/json")>]
[<ApiController>]
type CatalogController() =
    inherit ControllerBase()

    [<HttpGet>]
    member this.GetAllAsync() =
        "This is a dummy catalog"
