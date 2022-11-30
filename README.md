# Introduction

This repository contains insertion implementations on a list zipper and a red-black tree. 

The list zipper is just a simple linked list that "saves our position" on the last modified node so that nodes close to it are quicker to access (accessing the next/previous node from the last modified one is a constant time operation), somewhat similarly to a splay tree which brings the last accessed node and other nodes close to it to the top.

My interest in the list zipper was due to these constant time operations on nodes close together, but my benchmarks below show that accessing any node in a red-black tree never takes more than 10 nanoseconds than the fastest zipper operation so it makes sense to use trees instead.

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
|   **&#39;Random ListZipper.insert&#39;** |           **100** |       **3.070 μs** |      **1.3636 μs** |      **0.9019 μs** |          **-** |         **-** |         **-** |     **4448 B** |
| &#39;ListZipper.insert at start&#39; |           100 |       4.810 μs |      1.2778 μs |      0.8452 μs |          - |         - |         - |     6944 B |
|   &#39;ListZipper.insert at end&#39; |           100 |       1.090 μs |      0.1664 μs |      0.1101 μs |          - |         - |         - |      576 B |
|   **&#39;Random ListZipper.insert&#39;** |          **1000** |      **23.360 μs** |     **17.1649 μs** |     **11.3535 μs** |          **-** |         **-** |         **-** |    **58720 B** |
| &#39;ListZipper.insert at start&#39; |          1000 |      30.100 μs |      0.7850 μs |      0.4106 μs |          - |         - |         - |    64544 B |
|   &#39;ListZipper.insert at end&#39; |          1000 |       1.440 μs |      0.9216 μs |      0.6096 μs |          - |         - |         - |      576 B |
|   **&#39;Random ListZipper.insert&#39;** |         **10000** |     **175.840 μs** |    **130.2639 μs** |     **86.1615 μs** |          **-** |         **-** |         **-** |   **107040 B** |
| &#39;ListZipper.insert at start&#39; |         10000 |     316.210 μs |     36.6729 μs |     24.2568 μs |          - |         - |         - |   640544 B |
|   &#39;ListZipper.insert at end&#39; |         10000 |       1.656 μs |      0.3861 μs |      0.2297 μs |          - |         - |         - |      576 B |
|   **&#39;Random ListZipper.insert&#39;** |        **100000** |     **929.770 μs** |    **769.0881 μs** |    **508.7042 μs** |          **-** |         **-** |         **-** |  **2976480 B** |
| &#39;ListZipper.insert at start&#39; |        100000 |   9,919.120 μs |    417.9989 μs |    276.4804 μs |  1000.0000 |         - |         - |  6400544 B |
|   &#39;ListZipper.insert at end&#39; |        100000 |       5.533 μs |      1.6443 μs |      0.9785 μs |          - |         - |         - |      616 B |
|   **&#39;Random ListZipper.insert&#39;** |       **1000000** |  **47,275.589 μs** | **40,129.8165 μs** | **23,880.6101 μs** |  **2000.0000** |         **-** |         **-** | **17461792 B** |
| &#39;ListZipper.insert at start&#39; |       1000000 | 167,280.010 μs | 10,376.7286 μs |  6,863.5635 μs | 11000.0000 | 3000.0000 | 1000.0000 | 64000904 B |
|   &#39;ListZipper.insert at end&#39; |       1000000 |       6.213 μs |      0.2593 μs |      0.1356 μs |          - |         - |         - |      576 B |
