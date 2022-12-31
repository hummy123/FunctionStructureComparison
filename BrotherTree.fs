namespace ListZipperVsRbTree

(* RedBack tree copied from https://en.wikibooks.org/wiki/F_Sharp_Programming/Advanced_Data_Structures,
 * which is based on Chris Okasaki's well-known implementation. *)

module BrotherTree =
    type bro =
        | N0
        | N1 of bro
        | N2 of bro * int * bro
        | L2 of int
        | N3 of bro * int * bro * int * bro

    let empty = N0

    let rec n1 = function 
        | L2 a -> N2 (N0, a, N0)
        | N3 (t1, a1, t2, a2, t3) -> N2 (N2 (t1, a1, t2), a2, N1 t3)
        | N0 as node -> N1 node
        | N1 _ as node -> N1 (node)
        | N2 (_, _, _) as node -> N1 (node)

    let rec n2 x0 a2 t = 
        match x0, a2, t with 
        | L2 a1, a2, t -> N3 (N0, a1, N0, a2, t)
        | N3 (t1, a1, t2, a2, t3), a3, N1 t4 ->
            N2 (N2 (t1, a1, t2), a2, N2 (t3, a3, t4))
        | N3 (t1, a1, t2, a2, t3), a3, N0 -> N3 (N2 (t1, a1, t2), a2, N1 t3, a3, N0)
        | N3 (t1, a1, t2, a2, t3), a3, N2 (v, va, vb) ->
            N3 (N2 (t1, a1, t2), a2, N1 t3, a3, N2 (v, va, vb))
        | N3 (t1, a1, t2, a2, t3), a3, L2 v ->
            N3 (N2 (t1, a1, t2), a2, N1 t3, a3, L2 v)
        | N3 (t1, a1, t2, a2, t3), a3, N3 (v, va, vb, vc, vd) ->
            N3 (N2 (t1, a1, t2), a2, N1 t3, a3, N3 (v, va, vb, vc, vd))
        | N0, a1, L2 a2 -> N3 (N0, a1, N0, a2, N0)
        | N1 v, a1, L2 a2 -> N3 (N1 v, a1, N0, a2, N0)
        | N2 (v, va, vb), a1, L2 a2 -> N3 (N2 (v, va, vb), a1, N0, a2, N0)
        | N1 t1, a1, N3 (t2, a2, t3, a3, t4) ->
            N2 (N2 (t1, a1, t2), a2, N2 (t3, a3, t4))
        | N0, a1, N3 (t2, a2, t3, a3, t4) -> N3 (N0, a1, N1 t2, a2, N2 (t3, a3, t4))
        | N2 (v, va, vb), a1, N3 (t2, a2, t3, a3, t4) ->
            N3 (N2 (v, va, vb), a1, N1 t2, a2, N2 (t3, a3, t4))
        | N0, a, N0 -> N2 (N0, a, N0)
        | N0, a, N1 v -> N2 (N0, a, N1 v)
        | N0, a, N2 (v, va, vb) -> N2 (N0, a, N2 (v, va, vb))
        | N1 v, a, N0 -> N2 (N1 v, a, N0)
        | N1 v, a, N1 va -> N2 (N1 v, a, N1 va)
        | N1 v, a, N2 (va, vb, vc) -> N2 (N1 v, a, N2 (va, vb, vc))
        | N2 (v, va, vb), a, N0 -> N2 (N2 (v, va, vb), a, N0)
        | N2 (v, va, vb), a, N1 vc -> N2 (N2 (v, va, vb), a, N1 vc)
        | N2 (v, va, vb), a, N2 (vc, vd, ve) ->
            N2 (N2 (v, va, vb), a, N2 (vc, vd, ve))

    let rec ins (x: int) xal = 
        match x, xal with
        | x, N0 -> L2 x
        | x, N1 t -> n1 (ins x t)
        | x, N2 (l, a: int, r) ->
            if x < a 
            then n2 (ins x l) a r
            elif x = a 
            then n2 l a r
            else n2 l a (ins x r)
        | _ -> failwith "unexpected ins"

    let rec tree = function 
        | L2 a -> N2 (N0, a, N0)
        | N3 (t1, a1, t2, a2, t3) -> N2 (N2 (t1, a1, t2), a2, N1 t3)
        | t -> t

    let rec insert x t = tree (ins  x t)

    let rec findMax tree =
        match tree with
        | N0 -> N0
        | N1 t -> findMax t
        | N2(_, _, r) -> findMax r
        | N3(_, _, _, _, r) -> findMax r
        | L2 t -> findMax N0

    let rec split_min = function 
        | N0 -> None
        | N1 t ->
            match split_min t with
            | None -> None
            | Some(a, ta) -> Some(a, N1 ta)
        | N2 (t1, a, t2) ->
            match split_min t1 with
            | None -> Some(a, N1 t2)
            | Some(b, t1a) -> Some(b, n2 t1a a t2)
        | _ -> failwith "unexpected broTree.split_min case"

    let rec del (uu: int) x1 = 
        match uu, x1 with
        | _, N0 -> N0
        | x, N1 t -> N1 (del x t)
        | x, N2 (l, a, r) ->
            if uu < a
            then n2 (del x l) a r
            elif uu > a
            then n2 l a (del x r)
            else
                match split_min r with
                | None -> N1 l
                | Some(aa, b) -> n2 l aa b
        | _ -> failwith "unexpected brotree delete case"