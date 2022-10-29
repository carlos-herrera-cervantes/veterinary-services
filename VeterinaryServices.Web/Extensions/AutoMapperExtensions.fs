namespace VeterinaryServices.Web.Extensions

open Microsoft.Extensions.DependencyInjection
open AutoMapper
open VeterinaryServices.Web.Types

module AutoMapperExtensions =

    type IServiceCollection with

        member this.AddAutoMapperConfiguration() =
            let mapperConfiguration = MapperConfiguration(fun mc -> mc.AddProfile(AutoMapping()))
            let mapper = mapperConfiguration.CreateMapper()
            this.AddSingleton(mapper) |> ignore
            this
