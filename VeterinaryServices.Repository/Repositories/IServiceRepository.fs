namespace VeterinaryServices.Repository.Repositories

open System.Collections.Generic
open System.Threading.Tasks
open MongoDB.Driver
open VeterinaryServices.Domain.Models

[<AllowNullLiteral>]
type IServiceRepository =
    abstract member GetAllAsync: FilterDefinition<Service> * int * int -> Async<ICollection<Service>>
    abstract member GetOneAsync: FilterDefinition<Service> -> Task<Service>
    abstract member CountAsync: FilterDefinition<Service> -> Task<int64>
