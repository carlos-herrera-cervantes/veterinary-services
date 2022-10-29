namespace VeterinaryServices.Repository.Managers

open System.Threading.Tasks
open MongoDB.Driver
open VeterinaryServices.Domain.Models

[<AllowNullLiteral>]
type IServiceCatalogManager =
    abstract member CreateAsync: ServiceCatalog -> Task
    abstract member UpdateAsync: FilterDefinition<ServiceCatalog> * ServiceCatalog -> Task
    abstract member DeleteAsync: FilterDefinition<ServiceCatalog> -> Task
