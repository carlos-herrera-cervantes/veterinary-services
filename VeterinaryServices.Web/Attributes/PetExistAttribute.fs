namespace VeterinaryServices.Web.Attributes

open System.Threading.Tasks
open Microsoft.AspNetCore.Mvc
open Microsoft.AspNetCore.Mvc.Filters
open MongoDB.Driver
open VeterinaryServices.Repository.Repositories
open VeterinaryServices.Domain.Models

type PetExistFilter(petRepository: IPetRepository) =

    member private this._petRepository = petRepository

    interface IAsyncActionFilter with

        member this.OnActionExecutionAsync(context: ActionExecutingContext, next: ActionExecutionDelegate) =
            async {
                let userService = context.ActionArguments.["userService"] :?> UserService
                let filter = Builders<Pet>.Filter.Eq((fun p -> p.Id), userService.PetId)
                let! counter = this._petRepository.CountAsync filter |> Async.AwaitTask

                if counter = int64(0) then
                    let petNotFound = {| message = "Pet not found" |}
                    context.Result <- NotFoundObjectResult(petNotFound)
                else
                    do! next.Invoke() :> Task |> Async.AwaitTask
            } |> Async.StartAsTask :> Task

type PetExistAttribute() =
    inherit TypeFilterAttribute(typeof<PetExistFilter>)
