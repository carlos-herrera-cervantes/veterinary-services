namespace VeterinaryServices.Domain.Models

open System.ComponentModel.DataAnnotations
open MongoDB.Bson.Serialization.Attributes
open Newtonsoft.Json

[<AllowNullLiteral>]
type ServiceCatalog() =
    inherit BaseSchema()

    [<JsonProperty("name")>]
    [<BsonElement("name")>]
    [<Required(ErrorMessage = "Name is required")>]
    member val Name: string = null with get, set

    [<JsonProperty("description")>]
    [<BsonElement("description")>]
    [<Required(ErrorMessage = "Description is required")>]
    member val Description: string = null with get, set

    [<JsonProperty("active")>]
    [<BsonElement("active")>]
    member val Active: bool = true with get, set
