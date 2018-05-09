namespace FLayoutLib

open System
open System.Drawing
open System.Collections.Immutable

type Character(chars: ImmutableArray<char>, offset:int, rect:Option<Rectangle>) =
    let _offset = offset
    
    member public this.Value = chars.[_offset]
    
    interface IRect with
        member this.Rect : Option<Rectangle> = rect

#if Debug
    member public this.ToString = this.Value.ToString
#endif
