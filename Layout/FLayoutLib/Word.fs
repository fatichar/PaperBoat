namespace FLayoutLib

open System
open System.Drawing

type Word(chars: List<Character>, offset:int, charCount:int, rect:Option<Rectangle>) =
    do
        if offset < 0 || offset >= chars.Length
            then invalidArg "offset" ""
        if charCount < 1 || charCount > chars.Length - offset
            then invalidArg "charCount" ""

    //private props
    let _offset = offset
    let _chars  = chars
    
    //public props
    member public this.CharCount = charCount
    
    member public this.Item
        with get(i) = match i with
                        | n when n >= offset && n < offset + charCount -> 
                            chars.[offset + i]
                        | _ -> 
                            invalidArg "i" ""
     
    interface IRect with
        member this.Rect : Option<Rectangle> = rect

#if Debug
    member public this.ToString = this.Value.ToString
#endif
