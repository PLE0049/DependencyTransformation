# DependencyTransformation

* Dependency transformation computes two way dependency in non oriented weighted graph. 
* Input for this application is list of edges with their weight, in format `IdA;IdB;Weight`
* Result of Dependency transformation is non symetric matrix of dependencies.


## Parallel computation

In this program we use multiple method how to optimize performance of algorithm and compare their performance.

1) Sequentional approach
2) Native Microsoft `System.Threading.TasksParallel.For`
3) Microsoft Task
4) Implementation of Parallel manager - Parallel-For
5) Naive parallelism  

## Tests

We use XUnit to ensure correct results of Dependecy calculation. There is several Test Cases that tests:

* Loading graph from csv
* Computation of dependency for one node (cell of matrice)
* Computation of dependency on small sample matrix
* Computation of dependency on real data with comparison to precomputed results

## Results

### Table of average ticks per computation.

|                                | Lesmis   | Karate  | USAirports(1574 n) * | Newman (16727 n) ** |
|--------------------------------|----------|---------|----------------------|---------------------|
| Sequential                     | **31604**|**30177**| 290776               | 60973418            |
| Microsoft Parallel.For         | 77765    | 50166   | 458576               | 33848828            |
| Microsoft Task                 | 82093    | 88595   | **185884**           | **11989015**        |
| Parallel Processor (own impl.) | 201956   | 84983   | 414773               | 54411260            |
| Naive Paralell Threads         | 21056111 | 7725936 | NAN                  | NAN                 |


\*	https://toreopsahl.com/datasets/#usairports

\**	https://toreopsahl.com/datasets/#newman2001

*Note: Number as average per 5 runs of alg.*
