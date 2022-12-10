namespace ListZipperVsRbTree

module ListZipper =
    type T = {
        Focus: int list
        Path: int list
    }

    let empty = { Focus = []; Path = [] }

    let private next zipper =
        match zipper.Focus with
        | fHead::fList ->
            { Focus = fList; Path = fHead::zipper.Path }
        | [] -> zipper

    let private prev zipper =
        match zipper.Path with
        | pHead::pList ->
            { Focus = pHead::zipper.Focus; Path = pList }
        | [] -> zipper

    let rec insert insNum zipper =
        match zipper.Path, zipper.Focus with
        (* Insert number into focus when the zipper is empty. *)
        | [], [] 
            -> { Focus = [insNum]; Path = [] }
        (* When insNum is less than focus but greater than path, insert it at start of focus. *)
        | pHead::_, fHead::_ when fHead > insNum && pHead < insNum ->
            { zipper with Focus = insNum::zipper.Focus }
        (* Return unchanged zipper (zipped to specified number) when number already exists in zipper. *)
        | _, fHead::_ when fHead = insNum ->
            zipper
        (* When focus is greater than insNum but we are at start of zipper, insert insNum to start of focus. *)
        | [], fHead::_ when fHead > insNum ->
            {zipper with Focus = insNum::zipper.Focus}
        (* When focus is greater than insNum, recurse and try inserting at previous. *)
        | _, fHead::_ when fHead > insNum ->
            let difference = fHead - insNum
            let newFocus = zipper.Path[0..(difference - 1)] @ zipper.Focus
            let newPath = zipper.Path[difference..]
            { zipper with Focus = insNum::newFocus; Path = newPath }
        (* When focus is less than insNum but we are at end of zipper, insert insNum at end. *)
        | p, [f] when f < insNum -> 
            {Focus = [insNum]; Path = f::p;}
        (* When focus is less than insNum, recurse and try inserting at next. *)
        | _, fHead::_ when fHead < insNum ->
            insert insNum (next zipper)
        (* Above cases cover all logical possibilities. *)
        | _, _ -> failwith "Unexpected ListZipper.insert case"