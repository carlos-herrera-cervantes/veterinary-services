namespace VeterinaryServices.Repository.Repositories

open MongoDB.Driver
open System.Collections.Generic
open System.Threading.Tasks
open VeterinaryServices.Domain.Models

[<AllowNullLiteral>]
type IServiceCatalogRepository =
    abstract member GetAllAsync: FilterDefinition<ServiceCatalog> * int * int -> Async<IEnumerable<ServiceCatalog>>
    abstract member GetOneAsync: FilterDefinition<ServiceCatalog> -> Task<ServiceCatalog>
    abstract member CountAsync: FilterDefinition<ServiceCatalog> -> Task<int64>
