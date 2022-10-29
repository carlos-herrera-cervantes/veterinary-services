namespace VeterinaryServices.Services.Pagers

open System.Collections.Generic

type PagerStrategyManager<'a>(pageable: IPageable<'a>) =

    member private this._pageable = pageable

    member private this.makeStrategies() =
        let strategies = dict["classic", this._pageable]
        strategies

    interface IPagerStrategyManager<'a> with

        member this.GetPager(strategy: string, docs: IEnumerable<'a>, totalDocs: int64, page: int, pageSize: int) =
            let strategies = this.makeStrategies()
            let found, value = strategies.TryGetValue(strategy)

            match found with
            | false ->
                let pager = this._pageable.GetPages(docs, totalDocs, page, pageSize)
                pager
            | _ ->
                let pager = value.GetPages(docs, totalDocs, page, pageSize)
                pager
