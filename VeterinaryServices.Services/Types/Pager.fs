namespace VeterinaryServices.Services.Types

open System.Collections.Generic
open Newtonsoft.Json

type Pager<'a>() =

    [<JsonProperty("total_docs")>]
    member val TotalDocs: int64 = int64(0) with get, set

    [<JsonProperty("data")>]
    member val Data: IEnumerable<'a> = null with get, set

    [<JsonProperty("page")>]
    member val Page: int = 0 with get, set

    [<JsonProperty("page_size")>]
    member val PageSize: int = 0 with get, set

    [<JsonProperty("has_next")>]
    member val HasNext: bool = false with get, set

    [<JsonProperty("has_previous")>]
    member val HasPrevious: bool = false with get, set
