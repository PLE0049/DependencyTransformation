# DependencyTransformation

* Dependency transformation computes two way dependency in non oriented weighted graph. 
* Input for this application is list of edges with their weight, in format `IdA;IdB;Weight`
* Result of Dependency transformation is non symetric matrix of dependencies.


## Parallel computation

In this program we use multiple method how to optimize performance of algorithm and compare their performance.

1) Sequentional approach
2) Naive parallelism
3) Native Microsoft `System.Threading.TasksParallel.For`
4) Own Implementation of Parallel manager - Parallel-For

## Results
