open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.DependencyInjection
open VeterinaryServices.Repository.Managers
open VeterinaryServices.Repository.Repositories
open VeterinaryServices.Services.Calculators
open VeterinaryServices.Services.Pagers
open VeterinaryServices.Web.Extensions.MongoDbExtensions
open VeterinaryServices.Web.Extensions.AutoMapperExtensions

[<EntryPoint>]
let main args =
    let builder = WebApplication.CreateBuilder(args)
    builder.Services.AddControllers().AddNewtonsoftJson() |> ignore
    builder.Services.AddMongoDbClient() |> ignore
    builder.Services.AddAutoMapperConfiguration() |> ignore
    builder.Services.AddSingleton<IUserServiceManager, UserServiceManager>() |> ignore
    builder.Services.AddSingleton<IUserServiceRepository, UserServiceRepository>() |> ignore
    builder.Services.AddSingleton<IServiceCatalogManager, ServiceCatalogManager>() |> ignore
    builder.Services.AddSingleton<IServiceCatalogRepository, ServiceCatalogRepository>() |> ignore
    builder.Services.AddSingleton<IServiceRepository, ServiceRepository>() |> ignore
    builder.Services.AddSingleton<IServiceManager, ServiceManager>() |> ignore
    builder.Services.AddSingleton<IPetRepository, PetRepository>() |> ignore
    builder.Services.AddSingleton<ICustomerRepository, CustomerRepository>() |> ignore
    builder.Services.AddSingleton<IEmployeeRepository, EmployeeRepository>() |> ignore
    builder.Services.AddSingleton<ITotalCalculator, ClassicCalculator>() |> ignore
    builder.Services.AddSingleton(typedefof<IPageable<_>>, typedefof<Pageable<_>>) |> ignore

    let app = builder.Build()

    app.UseHttpLogging() |> ignore
    app.MapControllers() |> ignore
    app.Run()

    0 // Exit code
