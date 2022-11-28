namespace ListZipperVsRbTree 

open System
open BenchmarkDotNet
open BenchmarkDotNet.Attributes

module InsertData = 
    (* Zipper data for IterationSetup *)
    let emptyZipper = ListZipper.empty
    let mutable zipper = emptyZipper

    (* Random number data for insertion tests *)
    let mutable rnd = System.Random()
    let mutable randomInsNum = 0
    let beforeInsNum = -100 (* This number is below all the numbers already in the structure. *)
    let afterInsNum = 99_999 (* This number is larger than all the numbers already in the structure. *)

[<HtmlExporter>]
type Benchmarks () =
    [<Params(10, 100, 1000, 10_000)>]
    member val public structureSize = 0 with get, set

    [<IterationSetup>]
    member this.createWithSize() =
        InsertData.zipper <- InsertData.emptyZipper
        InsertData.randomInsNum <- InsertData.rnd.Next()
        for i in [0..this.structureSize] do
            InsertData.zipper <- ListZipper.insert i InsertData.zipper

    [<Benchmark; IterationCount 10>]
    member this.``Random ListZipper.insert``() =
        ListZipper.insert InsertData.randomInsNum InsertData.zipper

    [<Benchmark; IterationCount 10>]
    member this.``ListZipper.insert at start``() =
        ListZipper.insert InsertData.beforeInsNum InsertData.zipper

    [<Benchmark; IterationCount 10>]
    member this.``ListZipper.insert at end``() =
        ListZipper.insert InsertData.afterInsNum InsertData.zipper