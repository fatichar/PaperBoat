namespace FLayoutLib

open System
open System.Drawing
open System.Collections
open System.Collections.Immutable

type TextLine(words: Word array, offset:int, wordCount:int, rect:Rectangle) =
    do
        if offset < 0 || offset >= words.Length
            then invalidArg "offset" ""
        if wordCount < 1 || wordCount > words.Length - offset
            then invalidArg "charCount" ""

    //private props
    let _offset = offset
    let _words  = words
    
    //public props
    member public this.WordCount = wordCount
    member public this.Words = words.[offset .. offset + wordCount];
    
    member public this.Item
        with get(i) = match i with
                        | n when n >= offset && n < offset + wordCount -> 
                            words.[offset + i]
                        | _ -> 
                            invalidArg "i" ""
     
    interface IRect with
        member this.Rect:Rectangle = rect

#if Debug
    member public this.ToString = this.Value.ToString
#endif
