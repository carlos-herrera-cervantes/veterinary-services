namespace VeterinaryServices.Services.Calculators

[<AllowNullLiteral>]
type ITotalCalculator =
    abstract member CalculateTotalAsync: string array -> Async<decimal>
