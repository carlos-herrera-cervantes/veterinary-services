namespace VeterinaryServices.Web.Attributes

open System.Threading.Tasks
open Microsoft.AspNetCore.Mvc
open Microsoft.AspNetCore.Mvc.Filters
open MongoDB.Driver
open VeterinaryServices.Repository.Repositories
open VeterinaryServices.Domain.Models

type ServiceExistFilter(serviceRepository: IServiceRepository) =

    interface IAsyncActionFilter with

        member __.OnActionExecutionAsync(context: ActionExecutingContext, next: ActionExecutionDelegate): Task =
            async {
                let userService = context.ActionArguments.["userService"] :?> UserService
                let filter = Builders<Service>.Filter.In((fun e -> e.Id), userService.Services)
                let! counter = serviceRepository.CountAsync filter |> Async.AwaitTask

                if counter <> int64(userService.Services.Length) then
                    let serviceNotFound = {| message = "Service not found" |}
                    context.Result <- NotFoundObjectResult(serviceNotFound)
                else
                    do! next.Invoke() :> Task |> Async.AwaitTask
            } |> Async.StartAsTask :> Task

type ServiceExistAttribute() =
    inherit TypeFilterAttribute(typeof<ServiceExistFilter>)


type ServiceExistFromPatchFilter(serviceRepository: IServiceRepository) =

    interface IAsyncActionFilter with

        member __.OnActionExecutionAsync(context: ActionExecutingContext, next: ActionExecutionDelegate): Task =
            async {
                let patchUserService = context.ActionArguments.["patchUserService"] :?> PatchUserService
                let filter = Builders<Service>.Filter.In((fun e -> e.Id), patchUserService.Services)
                let! counter = serviceRepository.CountAsync filter |> Async.AwaitTask

                if counter <> int64(patchUserService.Services.Length) then
                    let serviceNotFound = {| message = "Service not found" |}
                    context.Result <- NotFoundObjectResult(serviceNotFound)
                else
                    do! next.Invoke() :> Task |> Async.AwaitTask
            } |> Async.StartAsTask :> Task

type ServiceExistFromPatchAttribute() =
    inherit TypeFilterAttribute(typeof<ServiceExistFromPatchFilter>)
