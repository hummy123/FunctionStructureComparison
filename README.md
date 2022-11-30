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

|Method                      |structureSize|Mean          |Error         |StdDev        |Median        |Gen0      |Gen1     |Allocated |
|----------------------------|-------------|--------------|--------------|--------------|--------------|----------|---------|----------|
|'ListZipper.insert at start'|100          |7.220 μs      |3.9795 μs     |2.6322 μs     |6.700 μs      |-         |-        |6944 B    |
|'RbTree.insert at start'    |100          |8.330 μs      |3.2261 μs     |2.1339 μs     |8.450 μs      |-         |-        |864 B     |
|'Random ListZipper.insert'  |100          |6.660 μs      |4.1522 μs     |2.7464 μs     |6.750 μs      |-         |-        |2400 B    |
|'Random RbTree.insert'      |100          |5.940 μs      |2.8133 μs     |1.8608 μs     |5.650 μs      |-         |-        |864 B     |
|'ListZipper.insert at end'  |100          |2.511 μs      |1.8493 μs     |1.1005 μs     |2.300 μs      |-         |-        |576 B     |
|'RbTree.insert at end'      |100          |6.240 μs      |2.0915 μs     |1.3834 μs     |6.400 μs      |-         |-        |1008 B    |
|'ListZipper.insert at start'|1000         |33.000 μs     |3.1031 μs     |1.8466 μs     |32.500 μs     |-         |-        |64544 B   |
|'RbTree.insert at start'    |1000         |5.620 μs      |3.8273 μs     |2.5315 μs     |5.450 μs      |-         |-        |1008 B    |
|'Random ListZipper.insert'  |1000         |16.420 μs     |8.0237 μs     |5.3072 μs     |15.500 μs     |-         |-        |61408 B   |
|'Random RbTree.insert'      |1000         |4.620 μs      |3.4731 μs     |2.2972 μs     |3.550 μs      |-         |-        |960 B     |
|'ListZipper.insert at end'  |1000         |1.744 μs      |1.3525 μs     |0.8048 μs     |1.500 μs      |-         |-        |576 B     |
|'RbTree.insert at end'      |1000         |3.956 μs      |2.8133 μs     |1.6741 μs     |3.200 μs      |-         |-        |1296 B    |
|'ListZipper.insert at start'|10000        |164.988 μs    |5.3773 μs     |2.8124 μs     |164.350 μs    |-         |-        |640544 B  |
|'RbTree.insert at start'    |10000        |4.500 μs      |2.2246 μs     |1.3238 μs     |4.400 μs      |-         |-        |1200 B    |
|'Random ListZipper.insert'  |10000        |95.300 μs     |74.1976 μs    |49.0772 μs    |113.200 μs    |-         |-        |138656 B  |
|'Random RbTree.insert'      |10000        |3.275 μs      |1.2590 μs     |0.6585 μs     |3.150 μs      |-         |-        |1104 B    |
|'ListZipper.insert at end'  |10000        |3.640 μs      |3.0356 μs     |2.0079 μs     |3.100 μs      |-         |-        |576 B     |
|'RbTree.insert at end'      |10000        |4.138 μs      |2.1191 μs     |1.1083 μs     |4.050 μs      |-         |-        |1440 B    |
|'ListZipper.insert at start'|100000       |6,918.300 μs  |412.1613 μs   |272.6192 μs   |6,876.650 μs  |1000.0000 |-        |6400544 B |
|'RbTree.insert at start'    |100000       |12.580 μs     |0.7702 μs     |0.5095 μs     |12.600 μs     |-         |-        |1008 B    |
|'Random ListZipper.insert'  |100000       |2,290.460 μs  |4,040.8718 μs |2,672.7865 μs |953.400 μs    |1000.0000 |-        |5893344 B |
|'Random RbTree.insert'      |100000       |10.350 μs     |1.5124 μs     |0.9000 μs     |10.550 μs     |-         |-        |1296 B    |
|'ListZipper.insert at end'  |100000       |6.189 μs      |0.3981 μs     |0.2369 μs     |6.200 μs      |-         |-        |240 B     |
|'RbTree.insert at end'      |100000       |8.800 μs      |0.5040 μs     |0.3333 μs     |8.750 μs      |-         |-        |1632 B    |
|'ListZipper.insert at start'|1000000      |159,831.840 μs|9,968.4396 μs |6,593.5056 μs |157,693.100 μs|10000.0000|2000.0000|64000544 B|
|'RbTree.insert at start'    |1000000      |15.161 μs     |0.8587 μs     |0.5110 μs     |15.250 μs     |-         |-        |1488 B    |
|'Random ListZipper.insert'  |1000000      |93,591.920 μs |70,213.1355 μs|46,441.6418 μs|97,688.550 μs |9000.0000 |2000.0000|53304096 B|
|'Random RbTree.insert'      |1000000      |12.561 μs     |1.3560 μs     |0.8069 μs     |12.350 μs     |-         |-        |1344 B    |
|'ListZipper.insert at end'  |1000000      |6.340 μs      |0.9049 μs     |0.5985 μs     |6.350 μs      |-         |-        |576 B     |
|'RbTree.insert at end'      |1000000      |13.590 μs     |6.5006 μs     |4.2997 μs     |11.600 μs     |-         |-        |1824 B    |
