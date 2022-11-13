namespace VeterinaryServices.Repository.Managers

open System
open System.Threading.Tasks
open MongoDB.Driver
open VeterinaryServices.Domain.Models

type UserServiceManager(client: IMongoClient) =

    let database = Environment.GetEnvironmentVariable("DEFAULT_DB")

    member private this._collection = client.GetDatabase(database).GetCollection<UserService>("user_services")

    interface IUserServiceManager with

        member this.CreateAsync(userService: UserService) = this._collection.InsertOneAsync userService

        member this.UpdateAsync(filter: FilterDefinition<UserService>, userService: UserService) =
            userService.UpdatedAt <- DateTime.UtcNow
            this._collection.ReplaceOneAsync(filter, userService) |> ignore
            Task.CompletedTask

        member this.DeleteManyAsync(filter: FilterDefinition<UserService>) =
            this._collection.DeleteManyAsync(filter) |> ignore
            Task.CompletedTask
