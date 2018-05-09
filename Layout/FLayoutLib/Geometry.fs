module Geometry

    open System.Drawing
    open FLayoutLib
    
    
    let conciseSum = List.fold (+) 0 [1 .. 10]
    
    let GetUnion (iRects:seq<IRect>) = 
        let rects = iRects |> Seq.map (fun ir -> ir.Rect) |> Seq.choose id

        let merge r1 r2 = Rectangle.Union(r1, r2)   
        
        match Seq.length rects with
        | 0 -> None
        | _ -> let merged = rects |> Seq.fold merge (Seq.head rects)
               Some(merged)
        