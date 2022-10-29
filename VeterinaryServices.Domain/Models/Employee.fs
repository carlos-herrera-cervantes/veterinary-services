namespace VeterinaryServices.Domain.Models

open MongoDB.Bson
open MongoDB.Bson.Serialization.Attributes

[<AllowNullLiteral>]
type Employee() =
    inherit BaseSchema()

    [<BsonElement("employee_id")>]
    [<BsonRepresentation(BsonType.ObjectId)>]
    member val EmployeeId: string = null with get, set
