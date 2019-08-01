![Azure DevOps builds](https://img.shields.io/azure-devops/build/jakubplesnik0556/e2326e62-1a5d-432d-8207-cc75c54cc312/1)
![Azure DevOps tests](https://img.shields.io/azure-devops/tests/jakubplesnik0556/e2326e62-1a5d-432d-8207-cc75c54cc312/1)

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

|                                | Lesmis | Karate | USAirports | Newman   | as-22july06 | astro-ph | PP-Decagon |
|--------------------------------|--------|--------|------------|----------|-------------|----------|------------|
| Sequential                     | 6960   | 6991   | 178667     | 91008613 | 12692952    | 17202213 | 376663664  |
| Microsofrt  Task               | 5635   | 6057   | 116831     | 54133513 | 6859156     | 7776491  | 94403879   |
| Parallel Processor (own impl.) | 3870   | 6057   | 528804     | 66788621 | 4470979     | 1655129  | 28239280   |
| Microsoft Parallel.For         | 9437   | 5576   | 155050     | 66153578 | 16643949    | 10837409 | 142772174  |
| Naive Paralell Threads         | 210272 | 90015  | 2564208    | NAN      | NAN         | NAN      | NAN        |


\*	https://toreopsahl.com/datasets/#usairports

\**	https://toreopsahl.com/datasets/#newman2001

*Note: Number as average per 5 runs of alg.*
