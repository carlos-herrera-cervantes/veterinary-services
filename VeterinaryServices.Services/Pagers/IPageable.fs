namespace VeterinaryServices.Services.Pagers

open System.Collections.Generic
open VeterinaryServices.Services.Types

[<AllowNullLiteral>]
type IPageable<'a> =
    abstract member GetPages: IEnumerable<'a> * int64 * int * int -> Pager<'a>
