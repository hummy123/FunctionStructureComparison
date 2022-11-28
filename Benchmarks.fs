namespace ListZipperVsRbTree 

open System
open BenchmarkDotNet
open BenchmarkDotNet.Attributes
open BenchmarkDotNet.Running

module InsertData = 
    (* Zipper data for IterationSetup *)
    let emptyZipper = ListZipper.empty
    let mutable zipper = emptyZipper

    (* Random number data for insertion tests *)
    let mutable rnd = System.Random()
    let mutable randomInsNum = 0
    let beforeInsNum = -100 (* This number is below all the numbers already in the structure. *)
    let afterInsNum = 9000_000 (* This number is larger than all the numbers already in the structure. *)

[<MemoryDiagnoser; HtmlExporter>]
type Insert () =
    [<Params(100, 1000, 10_000, 100_000, 1000_000)>]
    member val public structureSize = 0 with get, set

    [<IterationSetup>]
    member this.createWithSize() =
        InsertData.zipper <- InsertData.emptyZipper
        InsertData.randomInsNum <- InsertData.rnd.Next(0, this.structureSize)
        for i in [0..this.structureSize] do
            InsertData.zipper <- ListZipper.insert i InsertData.zipper

    [<Benchmark(Description = "Random ListZipper.insert"); IterationCount 10>]
    member this.RandomListZipperInsert() =
        ListZipper.insert InsertData.randomInsNum InsertData.zipper

    [<Benchmark(Description = "ListZipper.insert at start"); IterationCount 10>]
    member this.ListZipperInsertAtStart() =
        ListZipper.insert InsertData.beforeInsNum InsertData.zipper

    [<Benchmark(Description = "ListZipper.insert at end"); IterationCount 10>]
    member this.ListZipperInsertAtEnd() =
        ListZipper.insert InsertData.afterInsNum InsertData.zipper

module Main = 
    [<EntryPoint>]
    let Main _ =
        BenchmarkRunner.Run<Insert>() |> ignore
        0