namespace VeterinaryServices.Repository.Managers

open System.Threading.Tasks
open MongoDB.Driver
open VeterinaryServices.Domain.Models

[<AllowNullLiteral>]
type IUserServiceManager =
    abstract member CreateAsync: UserService -> Task
    abstract member UpdateAsync: FilterDefinition<UserService> * UserService -> Task
    abstract member DeleteManyAsync: FilterDefinition<UserService> -> Task
