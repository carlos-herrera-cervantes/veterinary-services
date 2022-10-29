namespace VeterinaryServices.Web.Types

open Newtonsoft.Json

type ServiceTotal() =

    [<JsonProperty("services")>]
    member val Services: string array = [||] with get, set
