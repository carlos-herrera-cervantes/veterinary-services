namespace VeterinaryServices.Web.Attributes

open System.Threading.Tasks
open Microsoft.AspNetCore.Mvc
open Microsoft.AspNetCore.Mvc.Filters
open MongoDB.Driver
open VeterinaryServices.Repository.Repositories
open VeterinaryServices.Domain.Models

type ServiceExistFilter(serviceRepository: IServiceRepository) =

    member private this._serviceRepository = serviceRepository

    interface IAsyncActionFilter with

        member this.OnActionExecutionAsync(context: ActionExecutingContext, next: ActionExecutionDelegate) =
            async {
                let userService = context.ActionArguments.["userService"] :?> UserService
                let filter = Builders<Service>.Filter.In((fun e -> e.Id), userService.Services)
                let! counter = this._serviceRepository.CountAsync filter |> Async.AwaitTask

                if counter <> int64(userService.Services.Length) then
                    let serviceNotFound = {| message = "Service not found" |}
                    context.Result <- NotFoundObjectResult(serviceNotFound)
                else
                    do! next.Invoke() :> Task |> Async.AwaitTask
            } |> Async.StartAsTask :> Task

type ServiceExistAttribute() =
    inherit TypeFilterAttribute(typeof<ServiceExistFilter>)
