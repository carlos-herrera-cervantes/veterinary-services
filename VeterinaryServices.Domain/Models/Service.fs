namespace VeterinaryServices.Domain.Models

open System.ComponentModel.DataAnnotations
open MongoDB.Bson.Serialization.Attributes
open MongoDB.Bson
open Newtonsoft.Json

[<AllowNullLiteral>]
type Service() =
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

    [<JsonProperty("category_id")>]
    [<BsonElement("category_id")>]
    [<BsonRepresentation(BsonType.ObjectId)>]
    [<Required(ErrorMessage = "Category ID is required")>]
    member val CategoryId: string = null with get, set

    [<JsonProperty("measurement_unit")>]
    [<BsonElement("measurement_unit")>]
    [<Required(ErrorMessage = "Measurement unit is required")>]
    member val MeasurementUnit: string = null with get, set

    [<JsonProperty("price")>]
    [<BsonElement("price")>]
    [<Range(1, 1000000, ErrorMessage = "Total cost should be greater than 0")>]
    member val Price: decimal = 0M with get, set
