namespace VeterinaryServices.Tests.Services.Pagers

open System.Collections.Generic
open Xunit
open Moq
open VeterinaryServices.Services.Pagers
open VeterinaryServices.Services.Types

[<Collection("PagerStrategyManager")>]
type PagerStrategyManagerTests() =

    [<Fact(DisplayName = "Should return pager object using the default strategy")>]
    member this.GetPagerShouldReturnPagerUsingDefaultStrategy() =
        let mockPageable = Mock<IPageable<string>>()

        mockPageable
            .Setup(fun x ->
                x.GetPages(
                    It.IsAny<IEnumerable<string>>(),
                    It.IsAny<int64>(),
                    It.IsAny<int>(),
                    It.IsAny<int>()))
            .Returns(Pager<string>()) |> ignore

        let pagerStrategyManager = PagerStrategyManager(mockPageable.Object) :> IPagerStrategyManager<string>

        let strategy = "bad strategy"
        let documents = [| "test element" |]
        let totalDocs, page, pageSize = int64(1), 0, 10
        let pager = pagerStrategyManager.GetPager(strategy, documents, totalDocs, page, pageSize)

        mockPageable
            .Verify((fun x ->
                x.GetPages(
                    It.IsAny<IEnumerable<string>>(),
                    It.IsAny<int64>(),
                    It.IsAny<int>(),
                    It.IsAny<int>())), Times.Once)

        Assert.NotNull(pager)

    [<Fact(DisplayName = "Should return the pager object using the selected strategy")>]
    member this.GetPagerShouldReturnPagerUsingSelectedStrategy() =
        let mockPageable = Mock<IPageable<string>>()

        mockPageable
            .Setup(fun x ->
                x.GetPages(
                    It.IsAny<IEnumerable<string>>(),
                    It.IsAny<int64>(),
                    It.IsAny<int>(),
                    It.IsAny<int>()))
            .Returns(Pager<string>()) |> ignore

        let pagerStrategyManager = PagerStrategyManager(mockPageable.Object) :> IPagerStrategyManager<string>

        let strategy = "classic"
        let documents = [| "test element" |]
        let totalDocs, page, pageSize = int64(1), 0, 10
        let pager = pagerStrategyManager.GetPager(strategy, documents, totalDocs, page, pageSize)

        mockPageable
            .Verify((fun x ->
                x.GetPages(
                    It.IsAny<IEnumerable<string>>(),
                    It.IsAny<int64>(),
                    It.IsAny<int>(),
                    It.IsAny<int>())), Times.Once)

        Assert.NotNull(pager)
