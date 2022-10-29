namespace VeterinaryServices.Web.Attributes

open System.Threading.Tasks
open Microsoft.AspNetCore.Mvc
open Microsoft.AspNetCore.Mvc.Filters
open MongoDB.Driver
open VeterinaryServices.Repository.Repositories
open VeterinaryServices.Domain.Models

type CustomerExistFilter(customerRepository: ICustomerRepository) =

    member private this._customerRepository = customerRepository

    interface IAsyncActionFilter with

        member this.OnActionExecutionAsync(context: ActionExecutingContext, next: ActionExecutionDelegate) =
            async {
                let userService = context.ActionArguments.["userService"] :?> UserService
                let filter = Builders<Customer>.Filter.Eq((fun c -> c.Id), userService.CustomerId)
                let! counter = this._customerRepository.CountAsync filter |> Async.AwaitTask

                if counter = int64(0) then
                    let customerNotFound = {| message = "Customer not found" |}
                    context.Result <- NotFoundObjectResult(customerNotFound)
                else
                    do! next.Invoke() :> Task |> Async.AwaitTask
            } |> Async.StartAsTask :> Task

type CustomerExistAttribute() =
    inherit TypeFilterAttribute(typeof<CustomerExistFilter>)
