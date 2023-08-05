namespace VeterinaryServices.Repository.Repositories

open System
open System.Threading.Tasks
open System.Collections.Generic
open MongoDB.Driver
open VeterinaryServices.Domain.Models
open VeterinaryServices.Domain.Constants

[<AllowNullLiteral>]
type UserServiceRepository(client: IMongoClient) =

    member private __.collection = client.GetDatabase(MongoConfig.DefaultDB).GetCollection<UserService>("user_services")

    interface IUserServiceRepository with

        member __.GetAllAsync(filter: FilterDefinition<UserService>, page: int, pageSize: int): Async<IEnumerable<UserService>> =
            async {
                let! documents =
                    __.collection
                        .Find(filter)
                        .SortByDescending(fun us -> us.CreatedAt :> obj)
                        .Skip(Nullable(page * pageSize))
                        .Limit(Nullable pageSize)
                        .ToListAsync() |> Async.AwaitTask

                return documents :> IEnumerable<UserService>
            }

        member __.GetOneAsync(filter: FilterDefinition<UserService>): Task<UserService> = __.collection.FindAsync(filter).Result.FirstOrDefaultAsync()

        member __.CountAsync(filter: FilterDefinition<UserService>): Task<int64> = __.collection.CountDocumentsAsync filter
