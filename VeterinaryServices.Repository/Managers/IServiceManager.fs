namespace VeterinaryServices.Repository.Managers

open System.Threading.Tasks
open MongoDB.Driver
open VeterinaryServices.Domain.Models

[<AllowNullLiteral>]
type IServiceManager =
    abstract member CreateAsync: Service -> Task
    abstract member UpdateAsync: FilterDefinition<Service> * Service -> Task
    abstract member DeleteAsync: FilterDefinition<Service> -> Task
