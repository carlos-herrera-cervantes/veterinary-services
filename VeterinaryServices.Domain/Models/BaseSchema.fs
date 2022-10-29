namespace VeterinaryServices.Domain.Models

open System
open MongoDB.Bson
open MongoDB.Bson.Serialization.Attributes
open Newtonsoft.Json

[<AllowNullLiteral>]
type BaseSchema() =

    [<JsonProperty("id")>]
    [<BsonElement("_id")>]
    [<BsonId>]
    [<BsonRepresentation(BsonType.ObjectId)>]
    member val Id: string = null with get, set

    [<JsonProperty("created_at")>]
    [<BsonElement("created_at")>]
    [<BsonRepresentation(BsonType.DateTime)>]
    member val CreatedAt: DateTime = DateTime.UtcNow with get, set

    [<JsonProperty("updated_at")>]
    [<BsonElement("updated_at")>]
    [<BsonRepresentation(BsonType.DateTime)>]
    member val UpdatedAt: DateTime = DateTime.UtcNow with get, set
