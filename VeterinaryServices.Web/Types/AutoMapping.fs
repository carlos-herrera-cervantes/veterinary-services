namespace VeterinaryServices.Web.Types

open AutoMapper
open VeterinaryServices.Domain.Models

type AutoMapping() as __ =
    inherit Profile()

    do
        __.CreateMap<UserService, UserServiceDetail>() |> ignore
        __.CreateMap<Service, ServiceDetail>() |> ignore
