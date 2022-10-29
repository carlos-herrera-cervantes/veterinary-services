namespace VeterinaryServices.Repository.Repositories

open System
open MongoDB.Driver
open System.Collections.Generic
open VeterinaryServices.Domain.Models

type ServiceCatalogRepository(client: IMongoClient) =

    let database = Environment.GetEnvironmentVariable("DEFAULT_DB")

    member private this._collection = client.GetDatabase(database).GetCollection<ServiceCatalog>("service_catalogs")

    interface IServiceCatalogRepository with

        member this.GetAllAsync(filter: FilterDefinition<ServiceCatalog>, page: int, pageSize: int) =
            async {
                let! documents =
                    this._collection
                        .Find(filter)
                        .SortByDescending(fun sc -> sc.CreatedAt :> obj)
                        .Skip(Nullable(page * pageSize))
                        .Limit(Nullable pageSize)
                        .ToListAsync() |> Async.AwaitTask

                return documents :> IEnumerable<ServiceCatalog>
            }

        member this.GetOneAsync(filter: FilterDefinition<ServiceCatalog>) =
            this._collection.FindAsync(filter).Result.FirstOrDefaultAsync()

        member this.CountAsync(filter: FilterDefinition<ServiceCatalog>) =
            this._collection.CountDocumentsAsync filter
