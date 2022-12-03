namespace ListZipperVsRbTree

(* Copied from: https://github.com/fsprojects/FSharpx.Collections/blob/master/src/FSharpx.Collections.Experimental/IntMap.fs *)

module IntMap =
    type IntMap =
        | Nil
        | Tip of int 
        | Bin of int * int * IntMap * IntMap

    ///O(1). The empty map. Credit: Haskell.org
    let empty = Nil
        
    let inline private maskW i m =
        int(i &&& (~~~(m - 1ul) ^^^ m))

    let inline private mask i m =
        maskW (uint32 i) (uint32 m)

    let inline private nomatch i p m =
        mask i m <> p

    let inline private zero i m =
        (uint32 i) &&& (uint32 m) = 0ul

    let inline private highestBitMask x0 =
        let x1 = x0 ||| (x0 >>> 1)
        let x2 = x1 ||| (x1 >>> 2)
        let x3 = x2 ||| (x2 >>> 4)
        let x4 = x3 ||| (x3 >>> 8)
        let x5 = x4 ||| (x4 >>> 16)
        let x6 = x5 ||| (x5 >>> 32) // for 64 bit platforms
        x6 ^^^ (x6 >>> 1)

    let inline private branchMask p1 p2 =
        int(highestBitMask(uint32 p1 ^^^ uint32 p2))

    let inline private join p1 t1 p2 t2 =
        let m = branchMask p1 p2
        let p = mask p1 m
        if zero p1 m then Bin(p, m, t1, t2) else Bin(p, m, t2, t1)

    ///O(min(n,W)). Insert a new key/value pair in the map. If the key is already present in the map, the associated value is replaced with the supplied value, i.e. insert is equivalent to insertWith const. Credit: Haskell.org
    let rec insert k t =
        match t with
        | Bin(p, m, l, r) when nomatch k p m -> join k (Tip(k)) p t
        | Bin(p, m, l, r) when zero k m -> Bin(p, m, insert k l, r)
        | Bin(p, m, l, r) -> Bin(p, m, l, insert k r)
        | Tip(ky) when k = ky -> Tip(k)
        | Tip(ky) -> join k (Tip(k)) ky t
        | Nil -> Tip(k)