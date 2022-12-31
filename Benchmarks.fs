namespace ListZipperVsRbTree 

open System
open BenchmarkDotNet
open BenchmarkDotNet.Attributes
open BenchmarkDotNet.Running

module InsertData = 
    (* Zipper data for IterationSetup *)
    let emptyZipper = ListZipper.empty
    let mutable zipper = emptyZipper

    (* RbTree data for IterationSetup *)
    let emptyTree = RedBlackTree.empty
    let mutable tree = emptyTree

    (* IntMap data for IterationSetup *)
    let emptyIntMap = IntMap.empty
    let mutable intMap = emptyIntMap

    (* AaTree data *)
    let emptyAaTree = AATree.emppty
    let mutable aaTree = emptyAaTree

    let mutable broTree = BrotherTree.empty

    (* Random number data for insertion tests *)
    let mutable rnd = System.Random()
    let mutable randomInsNum = 0
    let beforeInsNum = -100 (* This number is below all the numbers already in the structure. *)
    let afterInsNum = 9000_000 (* This number is larger than all the numbers already in the structure. *)

[<MemoryDiagnoser; HtmlExporter; MarkdownExporter>]
type ListZipperBenchmarks () =
    [<Params(100, 1000, 10_000, 100_000, 1000_000)>]
    member val public structureSize = 0 with get, set

    [<IterationSetup>]
    member this.createWithSize() =
        InsertData.zipper <- InsertData.emptyZipper
        InsertData.randomInsNum <- InsertData.rnd.Next(0, this.structureSize)
        for i in [0..this.structureSize] do
            InsertData.zipper <- ListZipper.insert i InsertData.zipper

    [<Benchmark(Description = "ListZipper.insert at start"); IterationCount 10>]
    member this.ListZipperInsertAtStart() =
        ListZipper.insert InsertData.beforeInsNum InsertData.zipper

    [<Benchmark(Description = "Random ListZipper.insert"); IterationCount 10>]
    member this.RandomListZipperInsert() =
        ListZipper.insert InsertData.randomInsNum InsertData.zipper

    [<Benchmark(Description = "ListZipper.insert at end"); IterationCount 10>]
    member this.ListZipperInsertAtEnd() =
        ListZipper.insert InsertData.afterInsNum InsertData.zipper

[<MemoryDiagnoser; HtmlExporter; MarkdownExporter>]
type AaTreeBenchmarks() =
    [<Params(100, 1000, 10_000, 100_000, 1000_000)>]
    member val public structureSize = 0 with get, set

    [<IterationSetup>]
    member this.createWithSize() =
        InsertData.aaTree <- InsertData.emptyAaTree
        InsertData.randomInsNum <- InsertData.rnd.Next(0, this.structureSize)
        for i in [0..this.structureSize] do
            InsertData.aaTree <- AATree.insert i InsertData.aaTree

    [<Benchmark(Description = "AaTree.insert at start"); IterationCount 10>]
    member this.RbTreeInsertAtStart() =
        AATree.insert InsertData.beforeInsNum InsertData.aaTree

    [<Benchmark(Description = "Random AaTree.insert"); IterationCount 10>]
    member this.RandomRbTreeInsert() =
        AATree.insert InsertData.randomInsNum InsertData.aaTree

    [<Benchmark(Description = "AaTree.insert at end"); IterationCount 10>]
    member this.RbTreeInsertAtEnd() =
        AATree.insert InsertData.afterInsNum InsertData.aaTree

    [<Benchmark(Description = "AaTree.findMax"); IterationCount 10>]
    member this.RbTreeFindMax() =
        RedBlackTree.findMax InsertData.tree


[<MemoryDiagnoser; HtmlExporter; MarkdownExporter>]
type BroTreeBenchmarks() =
    [<Params(100, 1000, 10_000, 100_000, 1000_000)>]
    member val public structureSize = 0 with get, set

    [<IterationSetup>]
    member this.createWithSize() =
        InsertData.broTree <- BrotherTree.empty
        InsertData.randomInsNum <- InsertData.rnd.Next(0, this.structureSize)
        for i in [0..this.structureSize] do
            InsertData.broTree <- BrotherTree.insert i InsertData.broTree

    [<Benchmark(Description = "BroTree.insert at start"); IterationCount 10>]
    member this.RbTreeInsertAtStart() =
        BrotherTree.insert InsertData.beforeInsNum InsertData.broTree

    [<Benchmark(Description = "Random BroTree.insert"); IterationCount 10>]
    member this.RandomRbTreeInsert() =
        BrotherTree.insert InsertData.randomInsNum InsertData.broTree

    [<Benchmark(Description = "BroTree.insert at end"); IterationCount 10>]
    member this.RbTreeInsertAtEnd() =
        BrotherTree.insert InsertData.afterInsNum InsertData.broTree

    [<Benchmark(Description = "BroTree.findMax"); IterationCount 10>]
    member this.IntMapFindMax() =
        BrotherTree.findMax InsertData.broTree

[<MemoryDiagnoser; HtmlExporter; MarkdownExporter>]
type RbTreeBenchmarks() =
    [<Params(100, 1000, 10_000, 100_000, 1000_000)>]
    member val public structureSize = 0 with get, set

    [<IterationSetup>]
    member this.createWithSize() =
        InsertData.tree <- InsertData.emptyTree
        InsertData.randomInsNum <- InsertData.rnd.Next(0, this.structureSize)
        for i in [0..this.structureSize] do
            InsertData.tree <- RedBlackTree.insert i InsertData.tree

    [<Benchmark(Description = "RbTree.insert at start"); IterationCount 10>]
    member this.RbTreeInsertAtStart() =
        RedBlackTree.insert InsertData.beforeInsNum InsertData.tree

    [<Benchmark(Description = "Random RbTree.insert"); IterationCount 10>]
    member this.RandomRbTreeInsert() =
        RedBlackTree.insert InsertData.randomInsNum InsertData.tree

    [<Benchmark(Description = "RbTree.insert at end"); IterationCount 10>]
    member this.RbTreeInsertAtEnd() =
        RedBlackTree.insert InsertData.afterInsNum InsertData.tree

    [<Benchmark(Description = "RbTree.findMax"); IterationCount 10>]
    member this.RbTreeFindMax() =
        RedBlackTree.findMax InsertData.tree

[<MemoryDiagnoser; HtmlExporter; MarkdownExporter>]
type IntMapBenchmarks() =
    [<Params(100, 1000, 10_000, 100_000, 1000_000)>]
    member val public structureSize = 0 with get, set

    [<IterationSetup>]
    member this.createWithSize() =
        InsertData.intMap <- InsertData.emptyIntMap
        InsertData.randomInsNum <- InsertData.rnd.Next(0, this.structureSize)
        for i in [0..this.structureSize] do
            InsertData.intMap <- IntMap.insert i InsertData.intMap

    [<Benchmark(Description = "IntMap.insert at start"); IterationCount 10>]
    member this.IntMapInsertAtStart() =
        IntMap.insert InsertData.beforeInsNum InsertData.intMap

    [<Benchmark(Description = "Random IntMap.insert"); IterationCount 10>]
    member this.RandomIntMapInsert() =
        IntMap.insert InsertData.randomInsNum InsertData.intMap

    [<Benchmark(Description = "IntMap.insert at end"); IterationCount 10>]
    member this.IntMapInsertAtEnd() =
        IntMap.insert InsertData.afterInsNum InsertData.intMap

    [<Benchmark(Description = "IntMap.findMax"); IterationCount 10>]
    member this.IntMapFindMax() =
        IntMap.findMax InsertData.intMap

module Main = 
    [<EntryPoint>]
    let Main _ =
        BenchmarkRunner.Run<BroTreeBenchmarks>() |> ignore
        0