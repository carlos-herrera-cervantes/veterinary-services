namespace VeterinaryServices.Services.Calculators

[<AllowNullLiteral>]
type ITotalCalculator =
    abstract member CalculateTotalAsync: array<string> -> Async<decimal>
