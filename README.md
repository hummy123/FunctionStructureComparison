# Introduction

This repository contains insertion implementations on a list zipper and a red-black tree. 

The list zipper is just a simple linked list that "saves our position" on the last modified node so that nodes close to it are quicker to access (accessing the next/previous node from the last modified one is a constant time operation), somewhat similarly to a splay tree which brings the last accessed node and other nodes close to it to the top.

My interest in the list zipper was due to these constant time operations on nodes close together, but my benchmarks below show that accessing any node in a red-black tree never takes more than 10 nanoseconds than the fastest zipper operation so it makes sense to use trees instead.

The reason the "insert at end" benchmarks for the list zipper below are so fast compared to the other directions is because the setup phase for each benchmark fills each structure with a number of nodes (up to 1_000_000) and this is done by always inserting into the end, meaning that our focus is there. If I inserted into the middle at the end of setup, then accessing nodes around the middle and start would be faster.

## Benchmarks


``` ini

BenchmarkDotNet=v0.13.2, OS=Windows 11 (10.0.22000.1219/21H2)
AMD Ryzen 3 5300U with Radeon Graphics, 1 CPU, 8 logical and 4 physical cores
.NET SDK=7.0.100
  [Host]     : .NET 6.0.11 (6.0.1122.52304), X64 RyuJIT AVX2 DEBUG
  Job-PZTXOX : .NET 6.0.11 (6.0.1122.52304), X64 RyuJIT AVX2

InvocationCount=1  IterationCount=10  UnrollFactor=1  

```
|                       Method | structureSize |           Mean |          Error |         StdDev |       Gen0 |      Gen1 |      Gen2 |  Allocated |
|----------------------------- |-------------- |---------------:|---------------:|---------------:|-----------:|----------:|----------:|-----------:|
 **'ListZipper.insert at start'** |           **100** |       **4,188.9 ns** |        **319.3 ns** |        **190.03 ns** |       **4,200.0 ns** |          **-** |         **-** |     **6944 B** |
     'RbTree.insert at start' |           100 |       2,612.5 ns |        462.0 ns |        241.65 ns |       2,550.0 ns |          - |         - |      864 B |
   'Random ListZipper.insert' |           100 |       3,300.0 ns |      2,324.7 ns |      1,537.68 ns |       3,100.0 ns |          - |         - |     5856 B |
       'Random RbTree.insert' |           100 |       4,055.6 ns |      2,385.6 ns |      1,419.60 ns |       3,800.0 ns |          - |         - |      768 B |
   'ListZipper.insert at end' |           100 |         925.0 ns |        135.2 ns |         70.71 ns |         900.0 ns |          - |         - |      576 B |
       'RbTree.insert at end' |           100 |       2,611.1 ns |        534.3 ns |        317.98 ns |       2,600.0 ns |          - |         - |     1008 B |
 **'ListZipper.insert at start'** |          **1000** |      **31,111.1 ns** |      **1,027.1 ns** |        **611.24 ns** |      **31,200.0 ns** |          **-** |         **-** |    **64544 B** |
     'RbTree.insert at start' |          1000 |       2,800.0 ns |        742.1 ns |        441.59 ns |       2,700.0 ns |          - |         - |     1008 B |
   'Random ListZipper.insert' |          1000 |      20,090.0 ns |     13,830.6 ns |      9,148.10 ns |      22,700.0 ns |          - |         - |    23840 B |
       'Random RbTree.insert' |          1000 |       2,794.4 ns |      1,748.6 ns |      1,040.57 ns |       2,450.0 ns |          - |         - |     1104 B |
   'ListZipper.insert at end' |          1000 |       1,216.7 ns |        314.4 ns |        187.08 ns |       1,150.0 ns |          - |         - |      576 B |
       'RbTree.insert at end' |          1000 |       3,200.0 ns |      1,432.5 ns |        947.51 ns |       2,650.0 ns |          - |         - |     1296 B |
 **'ListZipper.insert at start'** |         **10000** |     **197,566.7 ns** |     **22,794.5 ns** |     **13,564.66 ns** |     **190,500.0 ns** |          **-** |         **-** |   **640544 B** |
     'RbTree.insert at start' |         10000 |       5,080.0 ns |      3,398.4 ns |      2,247.86 ns |       4,200.0 ns |          - |         - |     1200 B |
   'Random ListZipper.insert' |         10000 |     129,900.0 ns |    130,629.1 ns |     86,403.03 ns |     150,800.0 ns |          - |         - |   567136 B |
       'Random RbTree.insert' |         10000 |       6,110.0 ns |      1,450.9 ns |        959.69 ns |       5,650.0 ns |          - |         - |      672 B |
   'ListZipper.insert at end' |         10000 |       3,010.0 ns |      2,072.3 ns |      1,370.69 ns |       2,550.0 ns |          - |         - |      576 B |
       'RbTree.insert at end' |         10000 |       3,500.0 ns |        617.4 ns |        367.42 ns |       3,400.0 ns |          - |         - |     1440 B |
 **'ListZipper.insert at start'** |        **100000** |   **8,238,905.6 ns** |    **215,090.4 ns** |    **127,996.82 ns** |   **8,256,850.0 ns** |  **1000.0000** |         **-** |  **6400544 B** |
     'RbTree.insert at start' |        100000 |      13,194.4 ns |      1,063.2 ns |        632.68 ns |      13,050.0 ns |          - |         - |     1344 B |
   'Random ListZipper.insert' |        100000 |     524,877.8 ns |    715,726.8 ns |    425,917.55 ns |     689,100.0 ns |  1000.0000 |         - |  4964064 B |
       'Random RbTree.insert' |        100000 |      10,460.0 ns |      1,835.7 ns |      1,214.22 ns |      10,250.0 ns |          - |         - |     1248 B |
   'ListZipper.insert at end' |        100000 |       6,355.6 ns |      1,005.1 ns |        598.15 ns |       6,400.0 ns |          - |         - |      576 B |
       'RbTree.insert at end' |        100000 |       9,077.8 ns |        979.1 ns |        582.62 ns |       8,900.0 ns |          - |         - |     1632 B |
 **'ListZipper.insert at start'** |       **1000000** | **193,421,877.8 ns** | **16,898,246.1 ns** | **10,055,875.18 ns** | **194,031,800.0 ns** | **10000.0000** | **2000.0000** | **64000544 B** |
     'RbTree.insert at start' |       1000000 |      19,810.0 ns |     10,594.6 ns |      7,007.69 ns |      15,550.0 ns |          - |         - |     1488 B |
   'Random ListZipper.insert' |       1000000 |  88,650,800.0 ns | 91,903,575.3 ns | 60,788,524.78 ns |  65,838,100.0 ns |  9000.0000 | 2000.0000 | 56712416 B |
       'Random RbTree.insert' |       1000000 |      17,970.0 ns |      8,117.6 ns |      5,369.26 ns |      17,600.0 ns |          - |         - |     1248 B |
   'ListZipper.insert at end' |       1000000 |       7,112.5 ns |      1,574.8 ns |        823.65 ns |       7,200.0 ns |          - |         - |      576 B |
       'RbTree.insert at end' |       1000000 |      13,280.0 ns |      4,472.3 ns |      2,958.15 ns |      12,200.0 ns |          - |         - |     1824 B |
