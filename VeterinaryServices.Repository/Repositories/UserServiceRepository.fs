namespace VeterinaryServices.Repository.Repositories

open System
open System.Collections.Generic
open MongoDB.Driver
open VeterinaryServices.Domain.Models

[<AllowNullLiteral>]
type UserServiceRepository(client: IMongoClient) =

    let database = Environment.GetEnvironmentVariable("DEFAULT_DB")

    member private this._collection = client.GetDatabase(database).GetCollection<UserService>("user_services")

    interface IUserServiceRepository with

        member this.GetAllAsync(filter: FilterDefinition<UserService>, page: int, pageSize: int) =
            async {
                let! documents =
                    this._collection
                        .Find(filter)
                        .SortByDescending(fun us -> us.CreatedAt :> obj)
                        .Skip(Nullable(page * pageSize))
                        .Limit(Nullable pageSize)
                        .ToListAsync() |> Async.AwaitTask

                return documents :> IEnumerable<UserService>
            }

        member this.GetOneAsync(filter: FilterDefinition<UserService>) =
            this._collection.FindAsync(filter).Result.FirstOrDefaultAsync()

        member this.CountAsync(filter: FilterDefinition<UserService>) =
            this._collection.CountDocumentsAsync filter
