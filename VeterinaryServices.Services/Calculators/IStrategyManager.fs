namespace VeterinaryServices.Services.Calculators

[<AllowNullLiteral>]
type IStrategyManager =
    abstract member RunJobAsync: string * string array -> Async<decimal>
