namespace VeterinaryServices.Web.Types

open Newtonsoft.Json

type Pager() =

    [<JsonProperty("page")>]
    member val Page: int = 0 with get, set

    [<JsonProperty("page_size")>]
    member val PageSize: int = 10 with get, set
