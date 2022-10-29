namespace VeterinaryServices.Repository.Repositories

open System.Threading.Tasks
open MongoDB.Driver
open VeterinaryServices.Domain.Models

[<AllowNullLiteral>]
type ICustomerRepository =
    abstract member CountAsync: FilterDefinition<Customer> -> Task<int64>
