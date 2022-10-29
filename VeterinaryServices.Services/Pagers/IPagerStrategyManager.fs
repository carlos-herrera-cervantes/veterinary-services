﻿namespace VeterinaryServices.Services.Pagers

open System.Collections.Generic
open VeterinaryServices.Services.Types

[<AllowNullLiteral>]
type IPagerStrategyManager<'a> =
    abstract member GetPager: string * IEnumerable<'a> * int64 * int * int -> Pager<'a>
