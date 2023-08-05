namespace VeterinaryServices.Web.Attributes

open System.Linq
open System.Threading.Tasks
open Microsoft.AspNetCore.Mvc
open Microsoft.AspNetCore.Mvc.Filters
open MongoDB.Driver
open VeterinaryServices.Repository.Repositories
open VeterinaryServices.Domain.Models

type EmployeeExistFilter(employeeRepository: IEmployeeRepository) =

    interface IAsyncActionFilter with

        member __.OnActionExecutionAsync(context: ActionExecutingContext, next: ActionExecutionDelegate): Task =
            async {
                let userService = context.ActionArguments.["userService"] :?> UserService

                match userService.EmployeeDetails.Length with
                | 0 -> do! next.Invoke() :> Task |> Async.AwaitTask
                | _ ->
                    let employeeIds = userService.EmployeeDetails.Select(fun ed -> ed.EmployeeId)
                    let filter = Builders<Employee>.Filter.In((fun e -> e.EmployeeId), employeeIds)
                    let! counter = employeeRepository.CountAsync filter |> Async.AwaitTask

                    if counter <> int64(userService.EmployeeDetails.Length) then
                        let employeeNotFound = {| message = "Some of the employees do not exist" |}
                        context.Result <- NotFoundObjectResult(employeeNotFound)
                    else
                        do! next.Invoke() :> Task |> Async.AwaitTask
            } |> Async.StartAsTask :> Task

type EmployeeExistAttribute() =
    inherit TypeFilterAttribute(typeof<EmployeeExistFilter>)
