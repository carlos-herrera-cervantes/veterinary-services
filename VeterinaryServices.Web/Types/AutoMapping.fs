namespace VeterinaryServices.Web.Types

open AutoMapper
open VeterinaryServices.Domain.Models

type AutoMapping() as this =
    inherit Profile()

    do
        this.CreateMap<UserService, UserServiceDetail>() |> ignore
        this.CreateMap<Service, ServiceDetail>() |> ignore
