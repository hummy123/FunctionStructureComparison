namespace ListZipperVsRbTree

(* RedBack tree copied from https://en.wikibooks.org/wiki/F_Sharp_Programming/Advanced_Data_Structures,
 * which is based on Chris Okasaki's well-known implementation. *)

module RedBlackTree =
    type color = R | B
    type tree =
        | E
        | T of color * tree * int * tree

    let empty = E

    let lbalance (a, b, c, d) =
        match a, b, c, d with
        | B, T(R, T(R, a, x, b), y, c), z, d            (* Left, left *)
        | B, T(R, a, x, T(R, b, y, c)), z, d            (* Left, right *)
            -> T(R, T(B, a, x, b), y, T(B, c, z, d))
        | a, b, c, d -> T(a, b, c, d) 

    let rbalance (a, b, c, d) =
        match a, b, c, d with
        | B, a, x, T(R, T(R, b, y, c), z, d)            (* Right, left *)
        | B, a, x, T(R, b, y, T(R, c, z, d))            (* Right, right *)
            -> T(R, T(B, a, x, b), y, T(B, c, z, d))
        | a, b, c, d -> T(a, b, c, d)
    
    let insert item tree =
        let rec ins = function
            | E -> T(R, E, item, E)
            | T(c, a, y, b) as node ->
                if item = y then node
                elif item < y then lbalance(c, ins a, y, b)
                else rbalance(c, a, y, ins b)

        (* Forcing root node to be black *)                
        match ins tree with
            | E -> failwith "Should never return empty from an insert"
            | T(_, l, x, r) -> T(B, l, x, r)

    let rec findMax tree =
        match tree with
        | E -> E
        | T(_, _, _, r) -> findMax r