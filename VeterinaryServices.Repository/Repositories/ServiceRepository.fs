namespace VeterinaryServices.Repository.Repositories

open System
open System.Threading.Tasks
open System.Collections.Generic
open MongoDB.Driver
open VeterinaryServices.Domain.Models
open VeterinaryServices.Domain.Constants

type ServiceRepository(client: IMongoClient) =

    member private __.collection = client.GetDatabase(MongoConfig.DefaultDB).GetCollection<Service>("services")

    interface IServiceRepository with

        member __.GetAllAsync(filter: FilterDefinition<Service>, page: int, pageSize: int): Async<ICollection<Service>> =
            async {
                let! documents =
                    __.collection
                        .Find(filter)
                        .SortByDescending(fun s -> s.CreatedAt :> obj)
                        .Skip(Nullable(page * pageSize))
                        .Limit(Nullable pageSize)
                        .ToListAsync() |> Async.AwaitTask

                return documents :> ICollection<Service>
            }

        member __.GetOneAsync(filter: FilterDefinition<Service>): Task<Service> = __.collection.FindAsync(filter).Result.FirstOrDefaultAsync()

        member __.CountAsync(filter: FilterDefinition<Service>): Task<int64> = __.collection.CountDocumentsAsync filter
