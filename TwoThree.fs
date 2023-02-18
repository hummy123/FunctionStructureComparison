namespace ListZipperVsRbTree

module TwoThree =
  type tree =
    | L
    | N2 of tree * int * tree
    | N3 of tree * int * tree * int * tree
  
  let empty = L

  type up = TI of tree | OF of tree * int * tree

  let treeI = function
    | TI t -> t
    | OF(l, a, r) -> N2(l, a, r)

  let rec ins x = function
    | L -> OF(L, x, L)
    | N2(l, a, r) as node ->
      if x < a then
        match ins x l with
        | TI l' -> TI(N2(l', a, r))
        | OF(l1, b, l2) -> TI(N3(l1, b, l2, a, r))
      elif x > a then
        match ins x r with
        | TI r' -> TI(N2(l, a, r'))
        | OF(r1, b, r2) -> TI(N3(l, a, r1, b, r2))
      else
        TI node
    | N3(l, a, m, b, r) as node ->
      if x < a then
        match ins x l with
        | TI l' -> TI(N3(l', a, m, b, r))
        | OF(l1, c, l2) -> OF(N2(l1, c, l2), a, N2(m, b, r))
      elif x > a then
        if x > b then
          match ins x r with
          | TI r' -> TI(N3(l, a, m, b, r'))
          | OF(r1, c, r2) -> OF(N2(l, a, m), b, N2(r1, c, r2))
        elif x < b then
          match ins x m with
          | TI m' -> TI(N3(l, a, m', b, r))
          | OF(m1, c, m2) -> OF(N2(l, a, m1), b, N2(m2, b, r))
        else
          TI(node)
      else
        TI(node)
  
  let insert x t = ins x t |> treeI
      
