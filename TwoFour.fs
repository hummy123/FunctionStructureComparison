namespace ListZipperVsRbTree

module TwoFour =
  type tree =
    | L
    | N2 of tree * int * tree
    | N3 of tree * int * tree * int * tree
    | N4 of tree * int * tree * int * tree * int * tree
  
  let empty = L

  type up = 
    | TI of tree
    | Upi of tree * int * tree

  let treeI = function
    | TI t -> t
    | Upi(l, a, r) -> N2(l, a, r)

  let rec ins x = function
    | L -> 
      Upi(L, x, L)
    | N2(l, a, r) as node ->
      if x < a then
        match ins x l with
        | TI l' -> TI(N2(l', a, r))
        | Upi(l1, b, l2) -> TI(N3(l1, b, l2, a, r))
      elif x = a then
        TI node
      else
        match ins x r with
        | TI r' -> TI(N2(l, a, r'))
        | Upi(r1, b, r2) -> TI(N3(l, a, r1, b, r2))
    | N3(l, a, m, b, r) as node ->
      if x < a then
        match ins x l with
        | TI l' -> TI(N3(l', a, m, b, r))
        | Upi(l1, c, l2) -> Upi(N2(l1, c, l2), a, N2(m, b, r))
      elif x < b then
        match ins x m with
        | TI m' -> TI(N3(l, a, m', b, r))
        | Upi(m1, c, m2) -> Upi(N2(l, a, m1), c, N2(m2, b, r))
      elif x > b then
        match ins x r with
        | TI r' -> TI(N3(l, a, m, b, r'))
        | Upi(r1, c, r2) -> Upi(N2(l, a, m), b, N2(r1, c, r2))
      else
        TI node
    | N4(t1, a, t2, b, t3, c, t4) as node ->
      if x < b then
        if x < a then
          match ins x t1 with
          | TI t -> TI(N4(t, a, t2, b, t3, c, t4))
          | Upi(l, y, r) -> Upi(N2(l, y, r), a, N3(t2, b, t3, c, t4))
        elif x > a then
          match ins x t2 with
          | TI t -> TI(N4(t1, a, t, b, t3, c, t4))
          | Upi(l, y, r) -> Upi(N2(t1, a, l), y, N3(r, b, t3, c, t4))
        else
          TI node
      elif x > b then
        if x < c then
          match ins x t3 with
          | TI t -> TI(N4(t1, a, t, b, t3, c, t4))
          | Upi(l, y, r) -> Upi(N2(t1, a, l), y, N3(r, b, t3, c, t4))
        elif x > c then
          match ins x t4 with
          | TI t -> TI(N4(t1, a, t2, b, t3, c, t))
          | Upi(l, y, r) -> Upi(N2(t1, a, t2), b, N3(t3, c, l, y, r))
        else
          TI node
      else
        TI node

  let insert x tree = ins x tree |> treeI

