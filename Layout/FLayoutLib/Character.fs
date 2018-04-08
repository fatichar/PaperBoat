namespace FLayoutLib

open System.Drawing
open System.Collections.Generic


type Character(chars: IReadOnlyList<char>, offset:int, rect:Rectangle) =
    let Offset = offset    
    member public x.Value 
                        with get() : char = chars.[Offset]

    interface IRect with
        member this.Rect : Rectangle = rect



