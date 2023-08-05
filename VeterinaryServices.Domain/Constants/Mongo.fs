namespace VeterinaryServices.Domain.Constants

open System

module MongoConfig =

    let DefaultDB = Environment.GetEnvironmentVariable("DEFAULT_DB")

    let CustomerDB = Environment.GetEnvironmentVariable("CUSTOMER_DB")

    let EmployeeDB = Environment.GetEnvironmentVariable("EMPLOYEE_DB")

    let PetDB = Environment.GetEnvironmentVariable("PET_DB");
