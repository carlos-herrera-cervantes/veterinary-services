namespace VeterinaryServices.Repository.Repositories

open System
open System.Linq.Expressions
open System.Collections.Generic
open System.Threading.Tasks
open VeterinaryServices.Domain.Models
open MongoDB.Driver

[<AllowNullLiteral>]
type IUserServiceRepository =
    abstract member GetAllAsync: FilterDefinition<UserService> * int * int -> Async<IEnumerable<UserService>>
    abstract member GetOneAsync: FilterDefinition<UserService> -> Task<UserService>
    abstract member CountAsync: FilterDefinition<UserService> -> Task<int64>
