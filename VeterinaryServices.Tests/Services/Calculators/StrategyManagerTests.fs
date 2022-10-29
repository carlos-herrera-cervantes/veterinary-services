namespace VeterinaryServices.Tests.Services.Calculators

open Xunit
open Moq
open VeterinaryServices.Services.Calculators

[<Collection("StrategyManager")>]
type StrateManagerTests() =

    [<Fact(DisplayName = "Should return the total using the default calculation strategy")>]
    member this.RunJobAsyncShouldReturnTotalUsingDefaultStrategy() =
        async {
            let mockTotalCalculator = Mock<ITotalCalculator>()

            mockTotalCalculator
                .Setup(fun x -> x.CalculateTotalAsync(It.IsAny<string array>()))
                .Returns(async { return 10M }) |> ignore

            let strategyManager = StrategyManager(mockTotalCalculator.Object) :> IStrategyManager
            let! total = strategyManager.RunJobAsync("bad strategy", [|"dummy-id"|])

            mockTotalCalculator
                .Verify((fun x -> x.CalculateTotalAsync(It.IsAny<string array>())), Times.Once)

            Assert.IsType<decimal>(total) |> ignore
            Assert.Equal(10M, total)
        }

    [<Fact(DisplayName = "Should return the total using the selected calculation strategy")>]
    member this.RunJobAsyncShouldReturnTotalUsingTheSelectedStrategy() =
        async {
            let mockTotalCalculator = Mock<ITotalCalculator>()

            mockTotalCalculator
                .Setup(fun x -> x.CalculateTotalAsync(It.IsAny<string array>()))
                .Returns(async { return 10M }) |> ignore

            let strategyManager = StrategyManager(mockTotalCalculator.Object) :> IStrategyManager
            let! total = strategyManager.RunJobAsync("classic", [|"dummy-id"|])

            mockTotalCalculator
                .Verify((fun x -> x.CalculateTotalAsync(It.IsAny<string array>())), Times.Once)

            Assert.IsType<decimal>(total) |> ignore
            Assert.Equal(10M, total)
        }
