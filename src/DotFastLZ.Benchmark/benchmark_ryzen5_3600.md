CPU : AMD Ryzen 5 3600
RAM : 64GB

### Benchmark : compression canterbury/alice29.txt

| Name                                     | MB/s    | Rate    |
|------------------------------------------|---------|---------|
| memcpy                                   | 4015.60 | 100.00  |
| DotFastLZ.Compression L2                 | 35.98   | 55.68   |
| K4os.Compression.LZ4 L00                 | 35.46   | 58.32   |
| DotFastLZ.Compression L1                 | 34.50   | 56.19   |
| System.IO.Compression.ZipArchive Fastest | 28.11   | 40.86   |
| System.IO.Compression.ZipArchive Optimal | 10.27   | 36.18   |
| K4os.Compression.LZ4 L03_HC              | 6.03    | 44.64   |
| K4os.Compression.LZ4 L09_HC              | 3.13    | 41.87   |
| K4os.Compression.LZ4 L12                 | 2.15    | 41.40   |

### Benchmark : decompression canterbury/alice29.txt

| Name                                     | MB/s     | Rate     |
|------------------------------------------|----------|----------|
| memcpy                                   | 11849.95 | 100.00   |
| K4os.Compression.LZ4 L03_HC              | 717.03   | 44.64    |
| K4os.Compression.LZ4 L12                 | 672.96   | 41.40    |
| K4os.Compression.LZ4 L09_HC              | 493.88   | 41.87    |
| System.IO.Compression.ZipArchive Optimal | 120.26   | 36.18    |
| K4os.Compression.LZ4 L00                 | 114.99   | 58.32    |
| System.IO.Compression.ZipArchive Fastest | 105.93   | 40.86    |
| DotFastLZ.Compression L2                 | 76.73    | 55.68    |
| DotFastLZ.Compression L1                 | 74.02    | 56.19    |

### Benchmark : compression canterbury/asyoulik.txt

| Name                                     | MB/s     | Rate     |
|------------------------------------------|----------|----------|
| memcpy                                   | 11750.00 | 100.00   |
| K4os.Compression.LZ4 L00                 | 138.46   | 63.64    |
| DotFastLZ.Compression L1                 | 46.14    | 59.54    |
| DotFastLZ.Compression L2                 | 45.10    | 58.91    |
| System.IO.Compression.ZipArchive Fastest | 31.84    | 43.37    |
| System.IO.Compression.ZipArchive Optimal | 10.76    | 39.76    |
| K4os.Compression.LZ4 L03_HC              | 8.58     | 49.55    |
| K4os.Compression.LZ4 L09_HC              | 6.71     | 47.06    |
| K4os.Compression.LZ4 L12                 | 2.71     | 46.58    |

### Benchmark : decompression canterbury/asyoulik.txt

| Name                                     | MB/s     | Rate     |
|------------------------------------------|----------|----------|
| memcpy                                   | 51017.09 | 100.00   |
| K4os.Compression.LZ4 L00                 | 1041.80  | 63.64    |
| K4os.Compression.LZ4 L09_HC              | 830.88   | 47.06    |
| K4os.Compression.LZ4 L12                 | 558.25   | 46.58    |
| K4os.Compression.LZ4 L03_HC              | 416.54   | 49.55    |
| System.IO.Compression.ZipArchive Optimal | 119.56   | 39.76    |
| System.IO.Compression.ZipArchive Fastest | 117.01   | 43.37    |
| DotFastLZ.Compression L1                 | 93.67    | 59.54    |
| DotFastLZ.Compression L2                 | 90.88    | 58.91    |

### Benchmark : compression canterbury/cp.html

| Name                                     | MB/s     | Rate     |
|------------------------------------------|----------|----------|
| memcpy                                   | 41898.66 | 100.00   |
| K4os.Compression.LZ4 L00                 | 98.75    | 48.40    |
| DotFastLZ.Compression L2                 | 42.65    | 47.77    |
| DotFastLZ.Compression L1                 | 39.38    | 49.32    |
| System.IO.Compression.ZipArchive Fastest | 28.75    | 35.82    |
| K4os.Compression.LZ4 L09_HC              | 21.90    | 42.03    |
| System.IO.Compression.ZipArchive Optimal | 16.06    | 33.04    |
| K4os.Compression.LZ4 L03_HC              | 11.82    | 42.47    |
| K4os.Compression.LZ4 L12                 | 9.63     | 41.83    |

### Benchmark : decompression canterbury/cp.html

| Name                                     | MB/s     | Rate     |
|------------------------------------------|----------|----------|
| memcpy                                   | 43450.46 | 100.00   |
| K4os.Compression.LZ4 L00                 | 581.78   | 48.40    |
| K4os.Compression.LZ4 L12                 | 555.11   | 41.83    |
| K4os.Compression.LZ4 L03_HC              | 550.50   | 42.47    |
| K4os.Compression.LZ4 L09_HC              | 213.55   | 42.03    |
| System.IO.Compression.ZipArchive Fastest | 109.95   | 35.82    |
| System.IO.Compression.ZipArchive Optimal | 104.30   | 33.04    |
| DotFastLZ.Compression L1                 | 92.68    | 49.32    |
| DotFastLZ.Compression L2                 | 87.07    | 47.77    |

### Benchmark : compression canterbury/fields.c

| Name                                     | MB/s     | Rate     |
|------------------------------------------|----------|----------|
| memcpy                                   | 44306.12 | 100.00   |
| K4os.Compression.LZ4 L00                 | 108.23   | 46.80    |
| DotFastLZ.Compression L1                 | 36.28    | 42.46    |
| DotFastLZ.Compression L2                 | 34.79    | 42.38    |
| K4os.Compression.LZ4 L03_HC              | 30.12    | 38.27    |
| System.IO.Compression.ZipArchive Fastest | 27.76    | 33.19    |
| K4os.Compression.LZ4 L09_HC              | 23.77    | 37.95    |
| System.IO.Compression.ZipArchive Optimal | 14.76    | 28.98    |
| K4os.Compression.LZ4 L12                 | 8.59     | 37.71    |

### Benchmark : decompression canterbury/fields.c

| Name                                     | MB/s     | Rate     |
|------------------------------------------|----------|----------|
| memcpy                                   | 48333.95 | 100.00   |
| K4os.Compression.LZ4 L00                 | 378.14   | 46.80    |
| K4os.Compression.LZ4 L09_HC              | 377.81   | 37.95    |
| K4os.Compression.LZ4 L03_HC              | 376.79   | 38.27    |
| K4os.Compression.LZ4 L12                 | 205.02   | 37.71    |
| System.IO.Compression.ZipArchive Fastest | 105.87   | 33.19    |
| System.IO.Compression.ZipArchive Optimal | 105.45   | 28.98    |
| DotFastLZ.Compression L1                 | 80.08    | 42.46    |
| DotFastLZ.Compression L2                 | 78.42    | 42.38    |

### Benchmark : compression canterbury/grammar.lsp

| Name                                     | MB/s     | Rate     |
|------------------------------------------|----------|----------|
| memcpy                                   | 29571.85 | 100.00   |
| K4os.Compression.LZ4 L03_HC              | 38.80    | 46.63    |
| K4os.Compression.LZ4 L09_HC              | 34.18    | 46.41    |
| DotFastLZ.Compression L1                 | 30.62    | 47.89    |
| DotFastLZ.Compression L2                 | 29.14    | 47.89    |
| System.IO.Compression.ZipArchive Fastest | 22.66    | 37.62    |
| System.IO.Compression.ZipArchive Optimal | 16.65    | 35.82    |
| K4os.Compression.LZ4 L12                 | 11.64    | 46.25    |
| K4os.Compression.LZ4 L00                 | 2.25     | 51.46    |

### Benchmark : decompression canterbury/grammar.lsp

| Name                                     | MB/s     | Rate     |
|------------------------------------------|----------|----------|
| memcpy                                   | 44357.78 | 100.00   |
| K4os.Compression.LZ4 L09_HC              | 467.90   | 46.41    |
| K4os.Compression.LZ4 L03_HC              | 462.19   | 46.63    |
| K4os.Compression.LZ4 L12                 | 446.00   | 46.25    |
| K4os.Compression.LZ4 L00                 | 398.75   | 51.46    |
| DotFastLZ.Compression L1                 | 93.48    | 47.89    |
| DotFastLZ.Compression L2                 | 91.47    | 47.89    |
| System.IO.Compression.ZipArchive Optimal | 86.24    | 35.82    |
| System.IO.Compression.ZipArchive Fastest | 77.00    | 37.62    |

### Benchmark : compression canterbury/kennedy.xls

| Name                                     | MB/s     | Rate     |
|------------------------------------------|----------|----------|
| memcpy                                   | 14484.37 | 100.00   |
| K4os.Compression.LZ4 L00                 | 149.88   | 36.39    |
| DotFastLZ.Compression L1                 | 57.72    | 39.37    |
| DotFastLZ.Compression L2                 | 55.85    | 40.08    |
| System.IO.Compression.ZipArchive Fastest | 39.85    | 23.17    |
| K4os.Compression.LZ4 L03_HC              | 28.64    | 31.96    |
| System.IO.Compression.ZipArchive Optimal | 16.41    | 21.38    |
| K4os.Compression.LZ4 L09_HC              | 3.43     | 31.48    |
| K4os.Compression.LZ4 L12                 | 0.27     | 31.48    |

### Benchmark : decompression canterbury/kennedy.xls

| Name                                     | MB/s     | Rate     |
|------------------------------------------|----------|----------|
| memcpy                                   | 43568.78 | 100.00   |
| K4os.Compression.LZ4 L00                 | 943.82   | 36.39    |
| K4os.Compression.LZ4 L03_HC              | 869.62   | 31.96    |
| K4os.Compression.LZ4 L12                 | 691.61   | 31.48    |
| K4os.Compression.LZ4 L09_HC              | 607.04   | 31.48    |
| System.IO.Compression.ZipArchive Optimal | 162.71   | 21.38    |
| System.IO.Compression.ZipArchive Fastest | 151.38   | 23.17    |
| DotFastLZ.Compression L2                 | 96.89    | 40.08    |
| DotFastLZ.Compression L1                 | 89.80    | 39.37    |

### Benchmark : compression canterbury/lcet10.txt

| Name                                     | MB/s     | Rate     |
|------------------------------------------|----------|----------|
| memcpy                                   | 12514.89 | 100.00   |
| K4os.Compression.LZ4 L00                 | 90.24    | 54.65    |
| DotFastLZ.Compression L1                 | 65.43    | 54.67    |
| DotFastLZ.Compression L2                 | 62.94    | 53.70    |
| System.IO.Compression.ZipArchive Fastest | 32.91    | 38.97    |
| K4os.Compression.LZ4 L03_HC              | 24.23    | 41.21    |
| System.IO.Compression.ZipArchive Optimal | 9.92     | 34.27    |
| K4os.Compression.LZ4 L09_HC              | 8.76     | 38.85    |
| K4os.Compression.LZ4 L12                 | 4.46     | 38.46    |

### Benchmark : decompression canterbury/lcet10.txt

| Name                                     | MB/s     | Rate     |
|------------------------------------------|----------|----------|
| memcpy                                   | 44921.01 | 100.00   |
| K4os.Compression.LZ4 L00                 | 955.72   | 54.65    |
| K4os.Compression.LZ4 L12                 | 748.06   | 38.46    |
| K4os.Compression.LZ4 L03_HC              | 708.95   | 41.21    |
| K4os.Compression.LZ4 L09_HC              | 669.29   | 38.85    |
| DotFastLZ.Compression L1                 | 150.98   | 54.67    |
| DotFastLZ.Compression L2                 | 140.22   | 53.70    |
| System.IO.Compression.ZipArchive Optimal | 119.38   | 34.27    |
| System.IO.Compression.ZipArchive Fastest | 115.31   | 38.97    |

### Benchmark : compression canterbury/plrabn12.txt

| Name                                     | MB/s     | Rate     |
|------------------------------------------|----------|----------|
| memcpy                                   | 39276.79 | 100.00   |
| K4os.Compression.LZ4 L00                 | 148.78   | 67.57    |
| DotFastLZ.Compression L1                 | 68.81    | 62.37    |
| DotFastLZ.Compression L2                 | 67.01    | 61.85    |
| System.IO.Compression.ZipArchive Fastest | 33.20    | 44.95    |
| K4os.Compression.LZ4 L03_HC              | 24.79    | 50.64    |
| System.IO.Compression.ZipArchive Optimal | 9.99     | 41.15    |
| K4os.Compression.LZ4 L09_HC              | 7.95     | 47.20    |
| K4os.Compression.LZ4 L12                 | 4.86     | 46.62    |

### Benchmark : decompression canterbury/plrabn12.txt

| Name                                     | MB/s     | Rate     |
|------------------------------------------|----------|----------|
| memcpy                                   | 45230.16 | 100.00   |
| K4os.Compression.LZ4 L00                 | 1564.92  | 67.57    |
| K4os.Compression.LZ4 L12                 | 774.99   | 46.62    |
| K4os.Compression.LZ4 L03_HC              | 724.32   | 50.64    |
| K4os.Compression.LZ4 L09_HC              | 715.50   | 47.20    |
| DotFastLZ.Compression L2                 | 158.32   | 61.85    |
| DotFastLZ.Compression L1                 | 156.43   | 62.37    |
| System.IO.Compression.ZipArchive Optimal | 129.30   | 41.15    |
| System.IO.Compression.ZipArchive Fastest | 124.58   | 44.95    |

### Benchmark : compression canterbury/ptt5

| Name                                     | MB/s     | Rate     |
|------------------------------------------|----------|----------|
| memcpy                                   | 12948.17 | 100.00   |
| K4os.Compression.LZ4 L00                 | 129.49   | 16.93    |
| DotFastLZ.Compression L2                 | 52.62    | 15.77    |
| DotFastLZ.Compression L1                 | 50.63    | 15.84    |
| System.IO.Compression.ZipArchive Fastest | 31.20    | 12.57    |
| K4os.Compression.LZ4 L03_HC              | 18.76    | 13.55    |
| System.IO.Compression.ZipArchive Optimal | 8.87     | 10.66    |
| K4os.Compression.LZ4 L09_HC              | 3.08     | 13.01    |
| K4os.Compression.LZ4 L12                 | 0.73     | 12.89    |

### Benchmark : decompression canterbury/ptt5

| Name                                     | MB/s     | Rate     |
|------------------------------------------|----------|----------|
| memcpy                                   | 45742.14 | 100.00   |
| K4os.Compression.LZ4 L03_HC              | 517.62   | 13.55    |
| K4os.Compression.LZ4 L12                 | 496.73   | 12.89    |
| K4os.Compression.LZ4 L00                 | 484.19   | 16.93    |
| K4os.Compression.LZ4 L09_HC              | 440.90   | 13.01    |
| System.IO.Compression.ZipArchive Fastest | 105.05   | 12.57    |
| DotFastLZ.Compression L1                 | 89.28    | 15.84    |
| DotFastLZ.Compression L2                 | 83.42    | 15.77    |
| System.IO.Compression.ZipArchive Optimal | 68.97    | 10.66    |

### Benchmark : compression canterbury/sum

| Name                                     | MB/s     | Rate     |
|------------------------------------------|----------|----------|
| memcpy                                   | 52097.87 | 100.00   |
| K4os.Compression.LZ4 L00                 | 141.51   | 49.20    |
| DotFastLZ.Compression L2                 | 68.30    | 49.40    |
| DotFastLZ.Compression L1                 | 64.29    | 53.66    |
| K4os.Compression.LZ4 L03_HC              | 34.73    | 43.44    |
| System.IO.Compression.ZipArchive Fastest | 30.27    | 38.01    |
| System.IO.Compression.ZipArchive Optimal | 16.10    | 34.17    |
| K4os.Compression.LZ4 L09_HC              | 12.85    | 42.72    |
| K4os.Compression.LZ4 L12                 | 3.69     | 42.60    |

### Benchmark : decompression canterbury/sum

| Name                                     | MB/s     | Rate     |
|------------------------------------------|----------|----------|
| memcpy                                   | 55255.31 | 100.00   |
| K4os.Compression.LZ4 L09_HC              | 1015.59  | 42.72    |
| K4os.Compression.LZ4 L03_HC              | 1011.53  | 43.44    |
| K4os.Compression.LZ4 L00                 | 979.44   | 49.20    |
| K4os.Compression.LZ4 L12                 | 977.19   | 42.60    |
| DotFastLZ.Compression L1                 | 170.67   | 53.66    |
| DotFastLZ.Compression L2                 | 163.58   | 49.40    |
| System.IO.Compression.ZipArchive Fastest | 110.07   | 38.01    |
| System.IO.Compression.ZipArchive Optimal | 108.01   | 34.17    |

### Benchmark : compression canterbury/xargs.1

| Name                                     | MB/s     | Rate     |
|------------------------------------------|----------|----------|
| memcpy                                   | 33593.18 | 100.00   |
| K4os.Compression.LZ4 L00                 | 143.37   | 62.95    |
| DotFastLZ.Compression L2                 | 61.69    | 58.46    |
| DotFastLZ.Compression L1                 | 61.50    | 58.46    |
| K4os.Compression.LZ4 L03_HC              | 47.57    | 57.23    |
| K4os.Compression.LZ4 L09_HC              | 45.51    | 57.09    |
| System.IO.Compression.ZipArchive Fastest | 30.02    | 45.56    |
| System.IO.Compression.ZipArchive Optimal | 19.47    | 43.70    |
| K4os.Compression.LZ4 L12                 | 16.28    | 56.87    |

### Benchmark : decompression canterbury/xargs.1

| Name                                     | MB/s     | Rate     |
|------------------------------------------|----------|----------|
| memcpy                                   | 67186.36 | 100.00   |
| K4os.Compression.LZ4 L12                 | 1259.69  | 56.87    |
| K4os.Compression.LZ4 L00                 | 1153.51  | 62.95    |
| K4os.Compression.LZ4 L03_HC              | 1142.05  | 57.23    |
| K4os.Compression.LZ4 L09_HC              | 1106.35  | 57.09    |
| DotFastLZ.Compression L1                 | 244.45   | 58.46    |
| DotFastLZ.Compression L2                 | 229.23   | 58.46    |
| System.IO.Compression.ZipArchive Optimal | 112.05   | 43.70    |
| System.IO.Compression.ZipArchive Fastest | 108.05   | 45.56    |

### Benchmark : compression silesia/dickens

| Name                                     | MB/s    | Rate    |
|------------------------------------------|---------|---------|
| memcpy                                   | 9270.65 | 100.00  |
| K4os.Compression.LZ4 L00                 | 141.51  | 63.07   |
| DotFastLZ.Compression L2                 | 67.49   | 58.84   |
| DotFastLZ.Compression L1                 | 64.97   | 59.46   |
| System.IO.Compression.ZipArchive Fastest | 31.20   | 42.78   |
| K4os.Compression.LZ4 L03_HC              | 24.24   | 46.87   |
| System.IO.Compression.ZipArchive Optimal | 9.29    | 38.53   |
| K4os.Compression.LZ4 L09_HC              | 7.66    | 43.49   |
| K4os.Compression.LZ4 L12                 | 4.69    | 42.93   |

### Benchmark : decompression silesia/dickens

| Name                                     | MB/s     | Rate     |
|------------------------------------------|----------|----------|
| memcpy                                   | 19090.80 | 100.00   |
| K4os.Compression.LZ4 L00                 | 1284.88  | 63.07    |
| K4os.Compression.LZ4 L09_HC              | 704.57   | 43.49    |
| K4os.Compression.LZ4 L03_HC              | 702.67   | 46.87    |
| K4os.Compression.LZ4 L12                 | 701.33   | 42.93    |
| DotFastLZ.Compression L1                 | 158.78   | 59.46    |
| DotFastLZ.Compression L2                 | 145.14   | 58.84    |
| System.IO.Compression.ZipArchive Fastest | 123.36   | 42.78    |
| System.IO.Compression.ZipArchive Optimal | 120.18   | 38.53    |

### Benchmark : compression silesia/mozilla

| Name                                     | MB/s    | Rate    |
|------------------------------------------|---------|---------|
| memcpy                                   | 8794.28 | 100.00  |
| K4os.Compression.LZ4 L00                 | 159.41  | 51.61   |
| DotFastLZ.Compression L2                 | 71.67   | 51.66   |
| DotFastLZ.Compression L1                 | 64.17   | 52.69   |
| System.IO.Compression.ZipArchive Fastest | 33.82   | 40.31   |
| K4os.Compression.LZ4 L03_HC              | 26.04   | 44.15   |
| System.IO.Compression.ZipArchive Optimal | 15.83   | 37.18   |
| K4os.Compression.LZ4 L09_HC              | 12.64   | 43.11   |
| K4os.Compression.LZ4 L12                 | 2.11    | 42.98   |

### Benchmark : decompression silesia/mozilla

| Name                                     | MB/s     | Rate     |
|------------------------------------------|----------|----------|
| memcpy                                   | 15444.43 | 100.00   |
| K4os.Compression.LZ4 L00                 | 1135.14  | 51.61    |
| K4os.Compression.LZ4 L09_HC              | 968.80   | 43.11    |
| K4os.Compression.LZ4 L12                 | 965.81   | 42.98    |
| K4os.Compression.LZ4 L03_HC              | 957.56   | 44.15    |
| DotFastLZ.Compression L1                 | 205.42   | 52.69    |
| DotFastLZ.Compression L2                 | 197.30   | 51.66    |
| System.IO.Compression.ZipArchive Fastest | 128.57   | 40.31    |
| System.IO.Compression.ZipArchive Optimal | 128.49   | 37.18    |

### Benchmark : compression silesia/mr

| Name                                     | MB/s     | Rate     |
|------------------------------------------|----------|----------|
| memcpy                                   | 10156.02 | 100.00   |
| K4os.Compression.LZ4 L00                 | 184.21   | 54.57    |
| DotFastLZ.Compression L2                 | 76.58    | 50.59    |
| DotFastLZ.Compression L1                 | 72.23    | 50.89    |
| System.IO.Compression.ZipArchive Fastest | 33.85    | 37.98    |
| K4os.Compression.LZ4 L03_HC              | 26.69    | 46.59    |
| System.IO.Compression.ZipArchive Optimal | 13.70    | 36.36    |
| K4os.Compression.LZ4 L09_HC              | 6.58     | 42.58    |
| K4os.Compression.LZ4 L12                 | 3.19     | 42.02    |

### Benchmark : decompression silesia/mr

| Name                                     | MB/s     | Rate     |
|------------------------------------------|----------|----------|
| memcpy                                   | 18921.97 | 100.00   |
| K4os.Compression.LZ4 L00                 | 1462.67  | 54.57    |
| K4os.Compression.LZ4 L03_HC              | 1028.99  | 46.59    |
| K4os.Compression.LZ4 L09_HC              | 869.48   | 42.58    |
| K4os.Compression.LZ4 L12                 | 856.94   | 42.02    |
| DotFastLZ.Compression L1                 | 204.48   | 50.89    |
| DotFastLZ.Compression L2                 | 189.50   | 50.59    |
| System.IO.Compression.ZipArchive Optimal | 130.20   | 36.36    |
| System.IO.Compression.ZipArchive Fastest | 128.69   | 37.98    |

### Benchmark : compression silesia/nci

| Name                                     | MB/s    | Rate    |
|------------------------------------------|---------|---------|
| memcpy                                   | 9399.66 | 100.00  |
| K4os.Compression.LZ4 L00                 | 127.39  | 16.49   |
| DotFastLZ.Compression L2                 | 66.50   | 19.60   |
| DotFastLZ.Compression L1                 | 58.76   | 20.64   |
| System.IO.Compression.ZipArchive Fastest | 31.06   | 12.38   |
| K4os.Compression.LZ4 L03_HC              | 16.12   | 12.67   |
| System.IO.Compression.ZipArchive Optimal | 7.38    | 9.61    |
| K4os.Compression.LZ4 L09_HC              | 2.95    | 10.95   |
| K4os.Compression.LZ4 L12                 | 1.53    | 10.78   |

### Benchmark : decompression silesia/nci

| Name                                     | MB/s     | Rate     |
|------------------------------------------|----------|----------|
| memcpy                                   | 15960.11 | 100.00   |
| K4os.Compression.LZ4 L00                 | 480.42   | 16.49    |
| K4os.Compression.LZ4 L03_HC              | 418.23   | 12.67    |
| K4os.Compression.LZ4 L12                 | 397.79   | 10.78    |
| K4os.Compression.LZ4 L09_HC              | 347.86   | 10.95    |
| DotFastLZ.Compression L1                 | 129.62   | 20.64    |
| DotFastLZ.Compression L2                 | 123.09   | 19.60    |
| System.IO.Compression.ZipArchive Optimal | 108.14   | 9.61     |
| System.IO.Compression.ZipArchive Fastest | 104.89   | 12.38    |

### Benchmark : compression silesia/ooffice

| Name                                     | MB/s     | Rate     |
|------------------------------------------|----------|----------|
| memcpy                                   | 17033.00 | 100.00   |
| K4os.Compression.LZ4 L00                 | 172.92   | 70.53    |
| DotFastLZ.Compression L2                 | 69.24    | 68.78    |
| DotFastLZ.Compression L1                 | 62.51    | 69.48    |
| System.IO.Compression.ZipArchive Fastest | 30.09    | 54.16    |
| K4os.Compression.LZ4 L03_HC              | 29.27    | 58.64    |
| K4os.Compression.LZ4 L09_HC              | 16.74    | 57.60    |
| System.IO.Compression.ZipArchive Optimal | 16.08    | 50.36    |
| K4os.Compression.LZ4 L12                 | 7.75     | 57.46    |

### Benchmark : decompression silesia/ooffice

| Name                                     | MB/s     | Rate     |
|------------------------------------------|----------|----------|
| memcpy                                   | 19246.78 | 100.00   |
| K4os.Compression.LZ4 L00                 | 1419.20  | 70.53    |
| K4os.Compression.LZ4 L03_HC              | 1060.65  | 58.64    |
| K4os.Compression.LZ4 L09_HC              | 982.20   | 57.60    |
| K4os.Compression.LZ4 L12                 | 973.25   | 57.46    |
| DotFastLZ.Compression L1                 | 196.29   | 69.48    |
| DotFastLZ.Compression L2                 | 190.73   | 68.78    |
| System.IO.Compression.ZipArchive Fastest | 124.50   | 54.16    |
| System.IO.Compression.ZipArchive Optimal | 114.32   | 50.36    |

### Benchmark : compression silesia/osdb

| Name                                     | MB/s    | Rate    |
|------------------------------------------|---------|---------|
| memcpy                                   | 9602.90 | 100.00  |
| K4os.Compression.LZ4 L00                 | 151.44  | 52.12   |
| DotFastLZ.Compression L2                 | 76.23   | 52.65   |
| DotFastLZ.Compression L1                 | 68.59   | 65.83   |
| System.IO.Compression.ZipArchive Fastest | 35.92   | 38.86   |
| K4os.Compression.LZ4 L03_HC              | 29.22   | 40.11   |
| System.IO.Compression.ZipArchive Optimal | 18.30   | 36.57   |
| K4os.Compression.LZ4 L09_HC              | 17.37   | 39.44   |
| K4os.Compression.LZ4 L12                 | 8.26    | 39.13   |

### Benchmark : decompression silesia/osdb

| Name                                     | MB/s     | Rate     |
|------------------------------------------|----------|----------|
| memcpy                                   | 18750.16 | 100.00   |
| K4os.Compression.LZ4 L00                 | 966.51   | 52.12    |
| K4os.Compression.LZ4 L12                 | 965.27   | 39.13    |
| K4os.Compression.LZ4 L09_HC              | 937.16   | 39.44    |
| K4os.Compression.LZ4 L03_HC              | 927.41   | 40.11    |
| DotFastLZ.Compression L1                 | 307.09   | 65.83    |
| DotFastLZ.Compression L2                 | 232.38   | 52.65    |
| System.IO.Compression.ZipArchive Fastest | 156.20   | 38.86    |
| System.IO.Compression.ZipArchive Optimal | 149.91   | 36.57    |

### Benchmark : compression silesia/reymont

| Name                                     | MB/s     | Rate     |
|------------------------------------------|----------|----------|
| memcpy                                   | 10336.23 | 100.00   |
| K4os.Compression.LZ4 L00                 | 133.14   | 48.01    |
| DotFastLZ.Compression L2                 | 65.53    | 47.60    |
| DotFastLZ.Compression L1                 | 64.61    | 49.85    |
| System.IO.Compression.ZipArchive Fastest | 33.13    | 34.90    |
| K4os.Compression.LZ4 L03_HC              | 22.58    | 36.64    |
| System.IO.Compression.ZipArchive Optimal | 7.62     | 28.44    |
| K4os.Compression.LZ4 L09_HC              | 4.10     | 31.86    |
| K4os.Compression.LZ4 L12                 | 2.63     | 31.13    |

### Benchmark : decompression silesia/reymont

| Name                                     | MB/s     | Rate     |
|------------------------------------------|----------|----------|
| memcpy                                   | 19128.91 | 100.00   |
| K4os.Compression.LZ4 L00                 | 777.19   | 48.01    |
| K4os.Compression.LZ4 L03_HC              | 656.91   | 36.64    |
| K4os.Compression.LZ4 L09_HC              | 632.66   | 31.86    |
| K4os.Compression.LZ4 L12                 | 629.51   | 31.13    |
| DotFastLZ.Compression L1                 | 150.81   | 49.85    |
| DotFastLZ.Compression L2                 | 142.64   | 47.60    |
| System.IO.Compression.ZipArchive Fastest | 121.30   | 34.90    |
| System.IO.Compression.ZipArchive Optimal | 118.97   | 28.44    |

### Benchmark : compression silesia/samba

| Name                                     | MB/s     | Rate     |
|------------------------------------------|----------|----------|
| memcpy                                   | 16678.11 | 100.00   |
| K4os.Compression.LZ4 L00                 | 161.22   | 35.72    |
| DotFastLZ.Compression L2                 | 70.32    | 35.45    |
| DotFastLZ.Compression L1                 | 66.78    | 38.61    |
| System.IO.Compression.ZipArchive Fastest | 34.26    | 28.64    |
| K4os.Compression.LZ4 L03_HC              | 25.77    | 29.20    |
| System.IO.Compression.ZipArchive Optimal | 13.27    | 25.45    |
| K4os.Compression.LZ4 L09_HC              | 10.80    | 28.42    |
| K4os.Compression.LZ4 L12                 | 3.08     | 28.21    |

### Benchmark : decompression silesia/samba

| Name                                     | MB/s     | Rate     |
|------------------------------------------|----------|----------|
| memcpy                                   | 16258.32 | 100.00   |
| K4os.Compression.LZ4 L00                 | 859.60   | 35.72    |
| K4os.Compression.LZ4 L03_HC              | 720.88   | 29.20    |
| K4os.Compression.LZ4 L12                 | 715.57   | 28.21    |
| K4os.Compression.LZ4 L09_HC              | 713.06   | 28.42    |
| DotFastLZ.Compression L1                 | 174.06   | 38.61    |
| DotFastLZ.Compression L2                 | 164.42   | 35.45    |
| System.IO.Compression.ZipArchive Fastest | 135.90   | 28.64    |
| System.IO.Compression.ZipArchive Optimal | 134.22   | 25.45    |

### Benchmark : compression silesia/sao

| Name                                     | MB/s     | Rate     |
|------------------------------------------|----------|----------|
| memcpy                                   | 16931.04 | 100.00   |
| K4os.Compression.LZ4 L00                 | 208.41   | 93.63    |
| DotFastLZ.Compression L2                 | 88.87    | 88.07    |
| DotFastLZ.Compression L1                 | 68.06    | 88.08    |
| System.IO.Compression.ZipArchive Fastest | 35.36    | 76.70    |
| K4os.Compression.LZ4 L03_HC              | 31.89    | 80.96    |
| K4os.Compression.LZ4 L09_HC              | 19.04    | 79.09    |
| System.IO.Compression.ZipArchive Optimal | 18.27    | 73.80    |
| K4os.Compression.LZ4 L12                 | 12.53    | 78.17    |

### Benchmark : decompression silesia/sao

| Name                                     | MB/s     | Rate     |
|------------------------------------------|----------|----------|
| memcpy                                   | 18346.75 | 100.00   |
| K4os.Compression.LZ4 L00                 | 2363.83  | 93.63    |
| K4os.Compression.LZ4 L12                 | 1821.24  | 78.17    |
| K4os.Compression.LZ4 L09_HC              | 1529.10  | 79.09    |
| K4os.Compression.LZ4 L03_HC              | 1500.73  | 80.96    |
| DotFastLZ.Compression L1                 | 359.43   | 88.08    |
| DotFastLZ.Compression L2                 | 342.54   | 88.07    |
| System.IO.Compression.ZipArchive Fastest | 209.11   | 76.70    |
| System.IO.Compression.ZipArchive Optimal | 191.40   | 73.80    |

### Benchmark : compression silesia/webster

| Name                                     | MB/s    | Rate    |
|------------------------------------------|---------|---------|
| memcpy                                   | 9308.25 | 100.00  |
| K4os.Compression.LZ4 L00                 | 136.14  | 48.58   |
| DotFastLZ.Compression L2                 | 65.75   | 47.89   |
| DotFastLZ.Compression L1                 | 64.80   | 48.91   |
| System.IO.Compression.ZipArchive Fastest | 32.74   | 34.62   |
| K4os.Compression.LZ4 L03_HC              | 23.41   | 35.55   |
| System.IO.Compression.ZipArchive Optimal | 10.98   | 29.70   |
| K4os.Compression.LZ4 L09_HC              | 8.79    | 33.77   |
| K4os.Compression.LZ4 L12                 | 4.34    | 33.34   |

### Benchmark : decompression silesia/webster

| Name                                     | MB/s     | Rate     |
|------------------------------------------|----------|----------|
| memcpy                                   | 15401.85 | 100.00   |
| K4os.Compression.LZ4 L00                 | 907.33   | 48.58    |
| K4os.Compression.LZ4 L03_HC              | 609.55   | 35.55    |
| K4os.Compression.LZ4 L09_HC              | 596.02   | 33.77    |
| K4os.Compression.LZ4 L12                 | 595.40   | 33.34    |
| DotFastLZ.Compression L1                 | 149.74   | 48.91    |
| DotFastLZ.Compression L2                 | 143.53   | 47.89    |
| System.IO.Compression.ZipArchive Optimal | 121.16   | 29.70    |
| System.IO.Compression.ZipArchive Fastest | 117.02   | 34.62    |

### Benchmark : compression silesia/x-ray

| Name                                     | MB/s     | Rate     |
|------------------------------------------|----------|----------|
| memcpy                                   | 17857.67 | 100.00   |
| K4os.Compression.LZ4 L00                 | 575.59   | 99.01    |
| DotFastLZ.Compression L2                 | 88.00    | 96.70    |
| DotFastLZ.Compression L1                 | 73.27    | 96.74    |
| System.IO.Compression.ZipArchive Fastest | 32.05    | 74.87    |
| K4os.Compression.LZ4 L03_HC              | 31.89    | 84.99    |
| K4os.Compression.LZ4 L09_HC              | 29.75    | 84.67    |
| System.IO.Compression.ZipArchive Optimal | 24.11    | 70.77    |
| K4os.Compression.LZ4 L12                 | 19.86    | 84.64    |

### Benchmark : decompression silesia/x-ray

| Name                                     | MB/s     | Rate     |
|------------------------------------------|----------|----------|
| memcpy                                   | 19281.54 | 100.00   |
| K4os.Compression.LZ4 L00                 | 6560.77  | 99.01    |
| K4os.Compression.LZ4 L03_HC              | 1627.16  | 84.99    |
| K4os.Compression.LZ4 L09_HC              | 1559.33  | 84.67    |
| K4os.Compression.LZ4 L12                 | 1432.95  | 84.64    |
| DotFastLZ.Compression L1                 | 321.08   | 96.74    |
| DotFastLZ.Compression L2                 | 305.02   | 96.70    |
| System.IO.Compression.ZipArchive Fastest | 176.49   | 74.87    |
| System.IO.Compression.ZipArchive Optimal | 163.49   | 70.77    |

### Benchmark : compression silesia/xml

| Name                                     | MB/s     | Rate     |
|------------------------------------------|----------|----------|
| memcpy                                   | 10247.78 | 100.00   |
| K4os.Compression.LZ4 L00                 | 137.75   | 22.96    |
| DotFastLZ.Compression L2                 | 65.77    | 23.90    |
| DotFastLZ.Compression L1                 | 65.20    | 25.97    |
| System.IO.Compression.ZipArchive Fastest | 34.20    | 17.00    |
| K4os.Compression.LZ4 L03_HC              | 19.05    | 15.95    |
| System.IO.Compression.ZipArchive Optimal | 8.92     | 12.85    |
| K4os.Compression.LZ4 L09_HC              | 6.84     | 14.41    |
| K4os.Compression.LZ4 L12                 | 2.76     | 14.22    |

### Benchmark : decompression silesia/xml

| Name                                     | MB/s     | Rate     |
|------------------------------------------|----------|----------|
| memcpy                                   | 18476.46 | 100.00   |
| K4os.Compression.LZ4 L00                 | 558.65   | 22.96    |
| K4os.Compression.LZ4 L03_HC              | 483.30   | 15.95    |
| K4os.Compression.LZ4 L09_HC              | 478.02   | 14.41    |
| K4os.Compression.LZ4 L12                 | 466.60   | 14.22    |
| DotFastLZ.Compression L1                 | 134.40   | 25.97    |
| DotFastLZ.Compression L2                 | 130.31   | 23.90    |
| System.IO.Compression.ZipArchive Fastest | 118.55   | 17.00    |
| System.IO.Compression.ZipArchive Optimal | 116.18   | 12.85    |

### Benchmark : compression enwik/enwik8

| Name                                     | MB/s    | Rate    |
|------------------------------------------|---------|---------|
| memcpy                                   | 9393.92 | 100.00  |
| K4os.Compression.LZ4 L00                 | 147.04  | 57.26   |
| DotFastLZ.Compression L2                 | 65.31   | 54.52   |
| DotFastLZ.Compression L1                 | 65.08   | 55.58   |
| System.IO.Compression.ZipArchive Fastest | 32.03   | 40.87   |
| K4os.Compression.LZ4 L03_HC              | 20.65   | 43.83   |
| System.IO.Compression.ZipArchive Optimal | 11.98   | 36.95   |
| K4os.Compression.LZ4 L09_HC              | 11.65   | 42.20   |
| K4os.Compression.LZ4 L12                 | 5.71    | 41.91   |

### Benchmark : decompression enwik/enwik8

| Name                                     | MB/s     | Rate     |
|------------------------------------------|----------|----------|
| memcpy                                   | 15513.86 | 100.00   |
| K4os.Compression.LZ4 L00                 | 1119.01  | 57.26    |
| K4os.Compression.LZ4 L09_HC              | 724.62   | 42.20    |
| K4os.Compression.LZ4 L03_HC              | 712.95   | 43.83    |
| K4os.Compression.LZ4 L12                 | 697.33   | 41.91    |
| DotFastLZ.Compression L1                 | 159.32   | 55.58    |
| DotFastLZ.Compression L2                 | 152.01   | 54.52    |
| System.IO.Compression.ZipArchive Optimal | 118.54   | 36.95    |
| System.IO.Compression.ZipArchive Fastest | 111.40   | 40.87    |