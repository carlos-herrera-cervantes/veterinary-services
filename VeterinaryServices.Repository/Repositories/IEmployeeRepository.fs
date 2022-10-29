namespace VeterinaryServices.Repository.Repositories

open System.Threading.Tasks
open MongoDB.Driver
open VeterinaryServices.Domain.Models

[<AllowNullLiteral>]
type IEmployeeRepository =
    abstract member CountAsync: FilterDefinition<Employee> -> Task<int64>