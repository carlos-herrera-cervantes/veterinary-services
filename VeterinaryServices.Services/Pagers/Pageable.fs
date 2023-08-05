namespace VeterinaryServices.Services.Pagers

open System.Collections.Generic
open VeterinaryServices.Services.Types

type Pageable<'a>() =

    interface IPageable<'a> with

        member __.GetPages(docs: IEnumerable<'a>, totalDocs: int64, page: int, pageSize: int): Pager<'a> =
            let pager = Pager()
            let skip = page * pageSize

            pager.HasNext <- int64(skip + pageSize) < totalDocs
            pager.HasPrevious <- not(page = 0)
            pager.TotalDocs <- totalDocs
            pager.Data <- docs
            pager.Page <- page
            pager.PageSize <- pageSize

            pager
