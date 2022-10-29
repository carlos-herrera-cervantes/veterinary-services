namespace VeterinaryServices.Repository.Repositories

open System
open System.Collections.Generic
open MongoDB.Driver
open VeterinaryServices.Domain.Models

type ServiceRepository(client: IMongoClient) =

    let database = Environment.GetEnvironmentVariable("DEFAULT_DB")

    member private this._collection = client.GetDatabase(database).GetCollection<Service>("services")

    interface IServiceRepository with

        member this.GetAllAsync(filter: FilterDefinition<Service>, page: int, pageSize: int) =
            async {
                let! documents =
                    this._collection
                        .Find(filter)
                        .SortByDescending(fun s -> s.CreatedAt :> obj)
                        .Skip(Nullable(page * pageSize))
                        .Limit(Nullable pageSize)
                        .ToListAsync() |> Async.AwaitTask

                return documents :> ICollection<Service>
            }

        member this.GetOneAsync(filter: FilterDefinition<Service>) =
            this._collection.FindAsync(filter).Result.FirstOrDefaultAsync()

        member this.CountAsync(filter: FilterDefinition<Service>) =
            this._collection.CountDocumentsAsync filter
