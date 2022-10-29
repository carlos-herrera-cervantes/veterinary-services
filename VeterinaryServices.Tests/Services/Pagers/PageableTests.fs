namespace VeterinaryServices.Tests.Services.Pagers

open Xunit
open VeterinaryServices.Services.Pagers

[<Collection("Pageable")>]
type PageableTests() =

    [<Fact(DisplayName = "Should return the correct pages")>]
    member this.GetPagesShouldReturnPages() =
        let docs = [|"carlos"; "isela"|]
        let totalDocs = int64(2)
        let page = 0
        let pageSize = 10

        let pageable = Pageable<string>() :> IPageable<string>
        let pages = pageable.GetPages(docs, totalDocs, page, pageSize)

        Assert.False(pages.HasNext)
        Assert.False(pages.HasPrevious)
        Assert.Equal(int64(2), pages.TotalDocs)
        Assert.Equal(0, pages.Page)
        Assert.Equal(10, pages.PageSize)
