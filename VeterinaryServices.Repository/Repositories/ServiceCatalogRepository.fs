namespace VeterinaryServices.Repository.Repositories

open System
open System.Threading.Tasks
open MongoDB.Driver
open System.Collections.Generic
open VeterinaryServices.Domain.Models
open VeterinaryServices.Domain.Constants

type ServiceCatalogRepository(client: IMongoClient) =

    member private __.collection = client.GetDatabase(MongoConfig.DefaultDB).GetCollection<ServiceCatalog>("service_catalogs")

    interface IServiceCatalogRepository with

        member __.GetAllAsync(filter: FilterDefinition<ServiceCatalog>, page: int, pageSize: int): Async<IEnumerable<ServiceCatalog>> =
            async {
                let! documents =
                    __.collection
                        .Find(filter)
                        .SortByDescending(fun sc -> sc.CreatedAt :> obj)
                        .Skip(Nullable(page * pageSize))
                        .Limit(Nullable pageSize)
                        .ToListAsync() |> Async.AwaitTask

                return documents :> IEnumerable<ServiceCatalog>
            }

        member __.GetOneAsync(filter: FilterDefinition<ServiceCatalog>): Task<ServiceCatalog> = __.collection.FindAsync(filter).Result.FirstOrDefaultAsync()

        member __.CountAsync(filter: FilterDefinition<ServiceCatalog>): Task<int64> = __.collection.CountDocumentsAsync filter
