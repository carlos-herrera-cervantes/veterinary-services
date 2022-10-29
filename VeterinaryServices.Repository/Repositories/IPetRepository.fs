namespace VeterinaryServices.Repository.Repositories

open System.Threading.Tasks
open MongoDB.Driver
open VeterinaryServices.Domain.Models

[<AllowNullLiteral>]
type IPetRepository =
    abstract member CountAsync: FilterDefinition<Pet> -> Task<int64>
