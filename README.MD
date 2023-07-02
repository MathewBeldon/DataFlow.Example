# DataFlow.Example

*TLDR: the different methods are within the Features folder*

 There are various approaches to process data efficiently, each with its own trade-offs. In this demo, I have created three different strategies: TPL Dataflow, which utilizes a network of concurrently executing blocks; Semaphore-based control, which simply limits the number of tasks running in parallel; and Single-threaded processing, which executes tasks sequentially without any concurrency.

### Dataflow (TPL Dataflow)

Task Parallel Library (TPL) Dataflow is ideal for building high-performance, data-driven applications. This approach constructs a processing pipeline using different blocks (like TransformManyBlock, TransformBlock, BroadcastBlock, etc.), each handling a specific type of processing. The blocks are linked, creating a complex data processing pipeline. Execution options are used to control concurrency levels, with the MaxDegreeOfParallelism property set to `Environment.ProcessorCount` for certain blocks (simulated CPU processes), allowing this many concurrent operations. Other blocks are configured to have an unbounded degree of parallelism (simulated I/O tasks). The model ensures that the completion status and exceptions are propagated through the pipeline.

### Semaphore

Semaphores are synchronization primitives that maintain a count representing the number of allowed accesses to a resource. This approach uses a SemaphoreSlim object with an initial count of `Environment.ProcessorCount`(for simulated CPU processes, simulated I/O tasks are ran all at once), allowing up to this many concurrent operations. The tasks must acquire the semaphore before processing and release it afterward.

### Sequential Processing with Asynchronous Tasks

This approach performs processing sequentially but leverages asynchronous tasks to prevent blocking. The method fetches, transforms, saves, and posts telemetry data one item at a time. While tasks are asynchronous, they are executed sequentially for each item.

## K6 Load Testing

These results represent the performance of three different processing approaches: Dataflow, Semaphore, and Sequential Processing with Asynchronous Tasks. Each approach is tested with a load script for 1 minute and 10 virtual users (VUs).

### Dataflow (loadTestDataflowProcess.js):
* Total of 1557 iterations completed.
* 100% of HTTP requests received status code 200, indicating success.
* The average request duration was around 193 milliseconds.
* Data received at a rate of approximately 108 kB/s.
* Average iteration duration was around 386 milliseconds.

### Semaphore (loadTestSemaphoreProcess.js):
* Total of 1512 iterations completed.
* 100% of HTTP requests received status code 200, indicating success.
* The average request duration was around 199 milliseconds.
* Data received at a rate of approximately 104 kB/s.
* Average iteration duration was around 399 milliseconds.

### Sequential Processing with Asynchronous Tasks (loadTestSingleProcess.js):
* Total of 30 iterations completed.
* 100% of HTTP requests received status code 200, indicating success.
* The average request duration was considerably higher, around 13.45 seconds.
* Data received at a rate of approximately 1.7 kB/s.
* Average iteration duration was around 26.91 seconds.

## Gantt Diagrams

The following diagrams illustrate three different approaches to processing data: a single-threaded process, a semaphore-controlled process that limits concurrency to 4 concurrent tasks, and a dataflow process where each block is allowed to process up to 4 tasks concurrently. Each approach executes a set process in which it:
1. retreives *n* amount of data (70ms simulated I/O)
2. transforms that data into *n*\*2 items (30ms simulated CPU per req(*n*))
3. saves those items and return an id (70ms simulated I/O per req(*n*\*2))
4. post the id as telemetry data (50ms simulated I/O per req(*n*\*2)) (fire and forget for dataflow and semaphore)

```mermaid
    gantt
        title Single process (20)
        dateFormat x
        axisFormat %L
        section Single
            GetDataAsync (20) : 1, 77ms
            TransformDataAsync (0) :crit, 83, 43ms
            SaveDataAsync (0) : 127, 77ms
            PostTelemetry (0) : 204, 63ms
            SaveDataAsync (0) : 268, 76ms
            PostTelemetry (0) : 344, 63ms
            TransformDataAsync (1) :crit, 407, 31ms
            SaveDataAsync (1) : 438, 79ms
            PostTelemetry (1) : 517, 62ms
            SaveDataAsync (1) : 579, 78ms
            PostTelemetry (1) : 657, 63ms
            TransformDataAsync (2) :crit, 720, 31ms
            SaveDataAsync (2) : 751, 77ms
            PostTelemetry (2) : 828, 62ms
            SaveDataAsync (2) : 891, 77ms
            PostTelemetry (2) : 968, 62ms
            TransformDataAsync (3) :crit, 1030, 31ms
            SaveDataAsync (3) : 1061, 78ms
            PostTelemetry (3) : 1139, 63ms
            SaveDataAsync (3) : 1203, 79ms
            PostTelemetry (3) : 1282, 61ms
            TransformDataAsync (4) :crit, 1343, 31ms
            SaveDataAsync (4) : 1374, 79ms
            PostTelemetry (4) : 1453, 62ms
            SaveDataAsync (4) : 1515, 78ms
            PostTelemetry (4) : 1593, 63ms
            TransformDataAsync (5) :crit, 1656, 31ms
            SaveDataAsync (5) : 1687, 77ms
            PostTelemetry (5) : 1764, 62ms
            SaveDataAsync (5) : 1826, 76ms
            PostTelemetry (5) : 1902, 62ms
            TransformDataAsync (6) :crit, 1964, 32ms
            SaveDataAsync (6) : 1996, 79ms
            PostTelemetry (6) : 2075, 62ms
            SaveDataAsync (6) : 2137, 79ms
            PostTelemetry (6) : 2216, 62ms
            TransformDataAsync (7) :crit, 2278, 31ms
            SaveDataAsync (7) : 2309, 77ms
            PostTelemetry (7) : 2387, 62ms
            SaveDataAsync (7) : 2449, 77ms
            PostTelemetry (7) : 2526, 62ms
            TransformDataAsync (8) :crit, 2588, 31ms
            SaveDataAsync (8) : 2620, 78ms
            PostTelemetry (8) : 2698, 63ms
            SaveDataAsync (8) : 2761, 78ms
            PostTelemetry (8) : 2839, 62ms
            TransformDataAsync (9) :crit, 2901, 31ms
            SaveDataAsync (9) : 2932, 78ms
            PostTelemetry (9) : 3010, 62ms
            SaveDataAsync (9) : 3072, 78ms
            PostTelemetry (9) : 3150, 63ms
            TransformDataAsync (10) :crit, 3213, 30ms
            SaveDataAsync (10) : 3243, 80ms
            PostTelemetry (10) : 3323, 61ms
            SaveDataAsync (10) : 3385, 76ms
            PostTelemetry (10) : 3462, 61ms
            TransformDataAsync (11) :crit, 3523, 30ms
            SaveDataAsync (11) : 3553, 77ms
            PostTelemetry (11) : 3630, 63ms
            SaveDataAsync (11) : 3693, 79ms
            PostTelemetry (11) : 3772, 63ms
            TransformDataAsync (12) :crit, 3835, 30ms
            SaveDataAsync (12) : 3865, 76ms
            PostTelemetry (12) : 3941, 62ms
            SaveDataAsync (12) : 4003, 77ms
            PostTelemetry (12) : 4080, 62ms
            TransformDataAsync (13) :crit, 4142, 31ms
            SaveDataAsync (13) : 4173, 78ms
            PostTelemetry (13) : 4251, 63ms
            SaveDataAsync (13) : 4314, 78ms
            PostTelemetry (13) : 4392, 62ms
            TransformDataAsync (14) :crit, 4454, 31ms
            SaveDataAsync (14) : 4485, 76ms
            PostTelemetry (14) : 4561, 62ms
            SaveDataAsync (14) : 4623, 76ms
            PostTelemetry (14) : 4699, 63ms
            TransformDataAsync (15) :crit, 4762, 31ms
            SaveDataAsync (15) : 4793, 78ms
            PostTelemetry (15) : 4871, 62ms
            SaveDataAsync (15) : 4933, 78ms
            PostTelemetry (15) : 5011, 63ms
            TransformDataAsync (16) :crit, 5074, 32ms
            SaveDataAsync (16) : 5106, 77ms
            PostTelemetry (16) : 5183, 63ms
            SaveDataAsync (16) : 5246, 77ms
            PostTelemetry (16) : 5323, 61ms
            TransformDataAsync (17) :crit, 5384, 45ms
            SaveDataAsync (17) : 5430, 78ms
            PostTelemetry (17) : 5508, 60ms
            SaveDataAsync (17) : 5568, 80ms
            PostTelemetry (17) : 5648, 62ms
            TransformDataAsync (18) :crit, 5710, 31ms
            SaveDataAsync (18) : 5741, 78ms
            PostTelemetry (18) : 5819, 63ms
            SaveDataAsync (18) : 5882, 78ms
            PostTelemetry (18) : 5960, 62ms
            TransformDataAsync (19) :crit, 6023, 31ms
            SaveDataAsync (19) : 6054, 79ms
            PostTelemetry (19) : 6133, 63ms
            SaveDataAsync (19) : 6196, 80ms
            PostTelemetry (19) : 6276, 60ms
```

```mermaid
    gantt
        title Semaphore process (20)
        dateFormat x
        axisFormat %L
        section Semaphore
            GetDataAsync (20) : 1, 75ms
            TransformDataAsync (19) :crit, 98, 41ms
            TransformDataAsync (2) :crit, 99, 40ms
            TransformDataAsync (0) :crit, 98, 41ms
            TransformDataAsync (1) :crit, 98, 41ms
            TransformDataAsync (3) :crit, 140, 45ms
            TransformDataAsync (4) :crit, 140, 45ms
            TransformDataAsync (6) :crit, 140, 46ms
            TransformDataAsync (5) :crit, 140, 45ms
            TransformDataAsync (9) :crit, 186, 31ms
            TransformDataAsync (10) :crit, 186, 31ms
            TransformDataAsync (11) :crit, 186, 31ms
            TransformDataAsync (8) :crit, 186, 31ms
            SaveDataAsync (0) : 146, 71ms
            SaveDataAsync (19) : 146, 71ms
            SaveDataAsync (2) : 146, 71ms
            SaveDataAsync (19) : 146, 71ms
            SaveDataAsync (2) : 146, 71ms
            SaveDataAsync (1) : 146, 71ms
            SaveDataAsync (0) : 146, 71ms
            SaveDataAsync (1) : 146, 71ms
            TransformDataAsync (15) :crit, 218, 45ms
            TransformDataAsync (12) :crit, 217, 46ms
            TransformDataAsync (13) :crit, 218, 45ms
            TransformDataAsync (14) :crit, 218, 45ms
            SaveDataAsync (5) : 186, 78ms
            SaveDataAsync (3) : 186, 78ms
            SaveDataAsync (4) : 186, 77ms
            SaveDataAsync (4) : 186, 78ms
            SaveDataAsync (6) : 186, 78ms
            SaveDataAsync (5) : 186, 78ms
            SaveDataAsync (6) : 186, 78ms
            SaveDataAsync (3) : 186, 79ms
            PostTelemetry (19) : 223, 57ms
            PostTelemetry (0) : 223, 57ms
            PostTelemetry (1) : 223, 57ms
            PostTelemetry (2) : 223, 57ms
            PostTelemetry (1) : 222, 58ms
            PostTelemetry (2) : 222, 58ms
            PostTelemetry (0) : 222, 59ms
            PostTelemetry (19) : 222, 58ms
            SaveDataAsync (11) : 218, 77ms
            SaveDataAsync (9) : 217, 78ms
            SaveDataAsync (11) : 218, 77ms
            SaveDataAsync (8) : 218, 77ms
            SaveDataAsync (10) : 218, 77ms
            SaveDataAsync (10) : 218, 77ms
            SaveDataAsync (8) : 218, 77ms
            SaveDataAsync (9) : 217, 80ms
            TransformDataAsync (7) :crit, 264, 33ms
            TransformDataAsync (18) :crit, 264, 33ms
            TransformDataAsync (17) :crit, 264, 33ms
            TransformDataAsync (16) :crit, 264, 33ms
            PostTelemetry (4) : 265, 60ms
            PostTelemetry (6) : 265, 60ms
            PostTelemetry (6) : 265, 60ms
            PostTelemetry (3) : 265, 60ms
            PostTelemetry (3) : 265, 60ms
            PostTelemetry (5) : 265, 60ms
            PostTelemetry (5) : 265, 60ms
            PostTelemetry (4) : 264, 61ms
            SaveDataAsync (13) : 264, 76ms
            SaveDataAsync (13) : 264, 76ms
            SaveDataAsync (15) : 264, 76ms
            SaveDataAsync (14) : 264, 76ms
            SaveDataAsync (14) : 264, 76ms
            SaveDataAsync (12) : 264, 76ms
            SaveDataAsync (15) : 264, 76ms
            SaveDataAsync (12) : 264, 76ms
            PostTelemetry (11) : 295, 61ms
            PostTelemetry (8) : 295, 61ms
            PostTelemetry (11) : 295, 61ms
            PostTelemetry (9) : 298, 58ms
            PostTelemetry (9) : 301, 55ms
            PostTelemetry (8) : 295, 61ms
            PostTelemetry (10) : 295, 61ms
            PostTelemetry (10) : 295, 61ms
            SaveDataAsync (7) : 298, 74ms
            SaveDataAsync (17) : 298, 74ms
            SaveDataAsync (16) : 303, 69ms
            SaveDataAsync (18) : 298, 74ms
            SaveDataAsync (18) : 301, 71ms
            SaveDataAsync (16) : 298, 74ms
            SaveDataAsync (17) : 302, 70ms
            SaveDataAsync (7) : 302, 70ms
```

```mermaid
    gantt
        title Dataflow process (20)
        dateFormat x
        axisFormat %L
        section Dataflow
            GetDataAsync (20) : 24, 80ms
            TransformDataAsync (0) :crit, 116, 35ms
            TransformDataAsync (3) :crit, 116, 35ms
            TransformDataAsync (2) :crit, 116, 35ms
            TransformDataAsync (1) :crit, 116, 35ms
            TransformDataAsync (4) :crit, 153, 44ms
            TransformDataAsync (5) :crit, 153, 44ms
            TransformDataAsync (7) :crit, 153, 44ms
            TransformDataAsync (6) :crit, 153, 44ms
            TransformDataAsync (10) :crit, 197, 30ms
            TransformDataAsync (11) :crit, 197, 30ms
            TransformDataAsync (8) :crit, 197, 30ms
            TransformDataAsync (9) :crit, 197, 30ms
            SaveDataAsync (0) : 161, 71ms
            SaveDataAsync (0) : 161, 71ms
            SaveDataAsync (1) : 161, 71ms
            SaveDataAsync (3) : 163, 71ms
            SaveDataAsync (1) : 162, 72ms
            SaveDataAsync (2) : 163, 72ms
            SaveDataAsync (2) : 163, 72ms
            SaveDataAsync (3) : 165, 71ms
            TransformDataAsync (12) :crit, 229, 32ms
            TransformDataAsync (13) :crit, 229, 32ms
            TransformDataAsync (14) :crit, 229, 33ms
            TransformDataAsync (15) :crit, 232, 31ms
            SaveDataAsync (4) : 197, 78ms
            SaveDataAsync (6) : 198, 77ms
            SaveDataAsync (6) : 197, 78ms
            SaveDataAsync (5) : 197, 78ms
            SaveDataAsync (7) : 198, 77ms
            SaveDataAsync (5) : 197, 78ms
            SaveDataAsync (7) : 198, 77ms
            SaveDataAsync (4) : 197, 78ms
            PostTelemetry (0) : 234, 57ms
            PostTelemetry (3) : 236, 55ms
            PostTelemetry (2) : 236, 55ms
            PostTelemetry (3) : 236, 55ms
            PostTelemetry (1) : 236, 55ms
            PostTelemetry (0) : 234, 57ms
            PostTelemetry (1) : 234, 57ms
            PostTelemetry (2) : 236, 55ms
            TransformDataAsync (16) :crit, 261, 44ms
            TransformDataAsync (17) :crit, 261, 44ms
            TransformDataAsync (19) :crit, 263, 43ms
            TransformDataAsync (18) :crit, 263, 43ms
            SaveDataAsync (10) : 232, 74ms
            SaveDataAsync (10) : 232, 74ms
            SaveDataAsync (11) : 232, 74ms
            SaveDataAsync (11) : 232, 74ms
            SaveDataAsync (8) : 230, 76ms
            SaveDataAsync (9) : 231, 75ms
            SaveDataAsync (8) : 230, 76ms
            SaveDataAsync (9) : 230, 76ms
            SaveDataAsync (12) : 262, 75ms
            PostTelemetry (7) : 276, 61ms
            PostTelemetry (7) : 276, 61ms
            PostTelemetry (6) : 276, 61ms
            PostTelemetry (6) : 276, 61ms
            PostTelemetry (5) : 276, 61ms
            SaveDataAsync (15) : 263, 74ms
            SaveDataAsync (15) : 263, 74ms
            SaveDataAsync (12) : 262, 75ms
            PostTelemetry (4) : 276, 61ms
            SaveDataAsync (14) : 263, 74ms
            SaveDataAsync (13) : 263, 74ms
            SaveDataAsync (13) : 263, 74ms
            PostTelemetry (4) : 276, 61ms
            PostTelemetry (5) : 276, 61ms
            SaveDataAsync (14) : 263, 74ms
            PostTelemetry (11) : 310, 58ms
            PostTelemetry (11) : 310, 58ms
            PostTelemetry (9) : 310, 58ms
            PostTelemetry (8) : 308, 61ms
            PostTelemetry (10) : 310, 58ms
            PostTelemetry (8) : 308, 61ms
            PostTelemetry (10) : 310, 59ms
            PostTelemetry (9) : 308, 61ms
            SaveDataAsync (16) : 306, 78ms
            SaveDataAsync (17) : 306, 78ms
            SaveDataAsync (16) : 306, 79ms
            SaveDataAsync (17) : 306, 79ms
            SaveDataAsync (18) : 306, 79ms
            SaveDataAsync (19) : 307, 78ms
            SaveDataAsync (19) : 306, 79ms
            SaveDataAsync (18) : 306, 79ms
            PostTelemetry (13) : 337, 50ms
            PostTelemetry (12) : 337, 50ms
            PostTelemetry (12) : 337, 50ms
            PostTelemetry (13) : 337, 50ms
```