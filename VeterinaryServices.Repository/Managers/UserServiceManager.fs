namespace VeterinaryServices.Repository.Managers

open System
open System.Threading.Tasks
open MongoDB.Driver
open VeterinaryServices.Domain.Models
open VeterinaryServices.Domain.Constants

type UserServiceManager(client: IMongoClient) =

    member private __.collection = client.GetDatabase(MongoConfig.DefaultDB).GetCollection<UserService>("user_services")

    interface IUserServiceManager with

        member __.CreateAsync(userService: UserService): Task = __.collection.InsertOneAsync userService

        member __.UpdateAsync(filter: FilterDefinition<UserService>, userService: UserService): Task =
            userService.UpdatedAt <- DateTime.UtcNow
            __.collection.ReplaceOneAsync(filter, userService) |> ignore
            Task.CompletedTask

        member __.DeleteManyAsync(filter: FilterDefinition<UserService>): Task =
            __.collection.DeleteManyAsync(filter) |> ignore
            Task.CompletedTask
