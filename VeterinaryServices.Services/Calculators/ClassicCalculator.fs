namespace VeterinaryServices.Services.Calculators

open System.Linq
open MongoDB.Driver
open VeterinaryServices.Repository.Repositories
open VeterinaryServices.Domain.Models

type ClassicCalculator(serviceRepository: IServiceRepository) =

    member private this._serviceRepository = serviceRepository

    interface ITotalCalculator with

        member this.CalculateTotalAsync(serviceIds: string array) =
            async {
                let filter = Builders<Service>.Filter.In((fun s -> s.Id), serviceIds)
                let! services = this._serviceRepository.GetAllAsync(filter, 0, 100)

                if services.Count = 0 then
                    return 0M
                else
                    let total = services.Select(fun s -> s.Price).Sum()
                    return total
            }
