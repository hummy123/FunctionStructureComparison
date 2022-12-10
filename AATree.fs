namespace ListZipperVsRbTree

(* Implementation guided by following paper: https://arxiv.org/pdf/1412.4882.pdf *)

module AATree =
    type AaTree = 
        | E
        | T of int * AaTree * int * AaTree

    let emppty = E

    let skew = function
        | T(lvx, T(lvy, a, ky, b), kx, c) when lvx = lvy ->
            T(lvx, a, ky, T(lvx, b, kx, c))
        | t -> t

    let split = function
        | T(lvx, a, kx, T(lvy, b, ky, T(lvz, c, kz, d))) when lvx = lvy && lvy = lvz ->
            T(lvx + 1, T(lvx, a, kx, b), ky, T(lvx, c, kz, d))
        | t -> t

    let rec insert item = function
        | E -> T(1, E, item, E)
        | T(h, l, v, r) as node ->
            if item < v
            then split <| (skew <| T(h, insert item l, v, r))
            elif item > v
            then split <| (skew <| T(h, l, v, insert item r))
            else node
