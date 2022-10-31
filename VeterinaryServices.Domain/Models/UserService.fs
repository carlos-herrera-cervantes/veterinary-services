namespace VeterinaryServices.Domain.Models

open System.ComponentModel.DataAnnotations
open MongoDB.Bson.Serialization.Attributes
open MongoDB.Bson
open Newtonsoft.Json
open VeterinaryServices.Domain.Constants

[<AllowNullLiteral>]
type EmployeeDetail() =

    [<JsonProperty("employee_id")>]
    [<BsonElement("employee_id")>]
    [<BsonRepresentation(BsonType.ObjectId)>]
    member val EmployeeId: string = null with get, set

    [<JsonProperty("employee_name")>]
    [<BsonElement("employee_name")>]
    member val EmployeeName: string = null with get, set

[<AllowNullLiteral>]
type UserService() =
    inherit BaseSchema()

    [<JsonProperty("customer_id")>]
    [<BsonElement("customer_id")>]
    [<BsonRepresentation(BsonType.ObjectId)>]
    member val CustomerId: string = null with get, set

    [<JsonProperty("pet_id")>]
    [<BsonElement("pet_id")>]
    [<BsonRepresentation(BsonType.ObjectId)>]
    [<Required(ErrorMessage = "Pet ID is required")>]
    member val PetId: string = null with get, set

    [<JsonProperty("pet_name")>]
    [<BsonElement("pet_name")>]
    [<Required(ErrorMessage = "Pet name is required")>]
    member val PetName: string = null with get, set

    [<JsonProperty("employee_details")>]
    [<BsonElement("employee_details")>]
    member val EmployeeDetails: EmployeeDetail array = [||] with get, set

    [<JsonProperty("services")>]
    [<BsonElement("services")>]
    [<BsonRepresentation(BsonType.ObjectId)>]
    [<Required(ErrorMessage = "Services is required")>]
    [<MinLength(1, ErrorMessage = "At least 1 servie should be defined")>]
    member val Services: string array = [||] with get, set

    [<JsonProperty("total_cost")>]
    [<BsonElement("total_cost")>]
    [<Required(ErrorMessage = "Total cost is required")>]
    [<Range(1, 1000000, ErrorMessage = "Total cost should be greater than 0")>]
    member val TotalCost: decimal = 0M with get, set

    [<JsonProperty("status")>]
    [<BsonElement("status")>]
    member val Status: UserServiceStatus = UserServiceStatus.Created with get, set

type ServiceDetail() =

    [<JsonProperty("name")>]
    member val Name: string = null with get, set

    [<JsonProperty("description")>]
    member val Description: string = null with get, set

    [<JsonProperty("price")>]
    member val Price: decimal = 0M with get, set

[<AllowNullLiteral>]
type UserServiceDetail() =
    inherit BaseSchema()

    [<JsonProperty("customer_id")>]
    member val CustomerId: string = null with get, set

    [<JsonProperty("pet_id")>]
    member val PetId: string = null with get, set

    [<JsonProperty("pet_name")>]
    member val PetName: string = null with get, set

    [<JsonProperty("employee_details")>]
    member val EmployeeDetails: EmployeeDetail array = [||] with get, set

    [<JsonProperty("service_details")>]
    member val ServiceDetails: ServiceDetail array = [||] with get, set

    [<JsonProperty("total_cost")>]
    member val TotalCost: decimal = 0M with get, set

    [<JsonProperty("status")>]
    member val Status: UserServiceStatus = UserServiceStatus.Created with get, set

[<AllowNullLiteral>]
type PatchUserService() =

    [<JsonProperty("services")>]
    [<Required(ErrorMessage = "Services is required")>]
    [<MinLength(1, ErrorMessage = "At least 1 servie should be defined")>]
    member val Services: string array = [||] with get, set
