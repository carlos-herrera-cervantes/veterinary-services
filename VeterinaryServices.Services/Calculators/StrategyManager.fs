namespace VeterinaryServices.Services.Calculators

type StrategyManager(totalCalculator: ITotalCalculator) =

    member private this._totalCalculator =  totalCalculator

    member private this.makeStrategies() =
        let strategies = dict["classic", this._totalCalculator]
        strategies

    interface IStrategyManager with

        member this.RunJobAsync(strategy: string, elements: string array) =
            async {
                let strategies = this.makeStrategies()
                let found, value = strategies.TryGetValue(strategy)

                match found with
                | false ->
                    let! totalCalculator = this._totalCalculator.CalculateTotalAsync elements
                    return totalCalculator
                | _ ->
                    let! totalCalculator = value.CalculateTotalAsync elements
                    return totalCalculator
            }
