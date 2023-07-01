# DataFlow.Example

*TLDR: the different methods are within the Features folder*

 There are various approaches to process data efficiently, each with its own trade-offs. In this demo, I have created three different strategies: TPL Dataflow, which utilizes a network of concurrently executing blocks; Semaphore-based control, which simply limits the number of tasks running in parallel; and Single-threaded processing, which executes tasks sequentially without any concurrency.

### Dataflow

* **Concurrency Model:** Dataflow employs a message-passing model where data moves through a network of linked blocks, and each block can process data concurrently up to a defined limit.
* **Composition:** Easy to create complex processing pipelines by linking multiple blocks. This provides clear separation of concerns for different stages of processing.
* **Scalability:** Can be highly scalable and can efficiently utilize resources, especially for I/O-bound operations.
* **Control:** Provides fine-grained control over how data moves through the blocks and how concurrency is managed.
* **Use Case:** Well-suited for scenarios where there are multiple stages of processing and there is a need for efficient management of resources.

### Semaphore

* **Concurrency Model:** Uses semaphore to limit the number of concurrent tasks. Multiple tasks can be run in parallel but the number of executing tasks is capped.
* **Composition:** The logic is often written inline and can be less modular compared to Dataflow.
* **Scalability:** Scalability is good but requires manual management compared to the automatic handling by Dataflow.
* **Control:** Provides control over concurrency, but less control over data flow compared to Dataflow.
* **Use Case:** Suited for scenarios where you want to limit the number of concurrent tasks to prevent resource contention, but do not necessarily need the structured pipeline that Dataflow offers.

### Single-threaded processing

* **Concurrency Model:** Processes tasks one at a time on a single thread.
* **Composition:** Like the Semaphore approach, logic is usually written inline.
* **Scalability:** Not scalable. Processing time increases linearly with the number of tasks.
* **Control:** Simple control, as there is no concurrency or parallelism.
* **Use Case:** Good for simple scenarios where the data processing is not resource-intensive, or where concurrency and parallelism would not offer substantial benefits. Also, easier to debug.

## K6 Load Testing

This load testing was performed with K6 under the scenario of 10 max Virtual Users (VUs) and a maximum duration of 1 minute 30 seconds, including a graceful stop of 30 seconds. Three different processing strategies were evaluated: Single Process, Semaphore Process, and Dataflow Process. The evaluation measures their performance in handling HTTP requests.

### Single Process

* **Status:** 100% success rate (200 OK)
* **Data Received:** 32 kB
* **Data Sent:** 15 kB
* **Number of HTTP Requests:** 140
* **Average Request Duration:** 4.77 seconds
* **Average Iteration Duration:** 9.55 seconds
* **Throughput:** Approximately 2.09 requests/second
* **Number of Iterations:** 70
* **VUs:** 10

### Semaphore Process

* **Status:** 100% success rate (200 OK)
* **Data Received:** 91 kB
* **Data Sent:** 50 kB
* **Number of HTTP Requests:** 620
* **Average Request Duration:** 985.12 milliseconds
* **Average Iteration Duration:** 1.97 seconds
* **Throughput:** Approximately 10.15 requests/second
* **Number of Iterations:** 310
* **VUs:** 10

### Dataflow Process

* **Status:** 100% success rate (200 OK)
* **Data Received:** 181 kB
* **Data Sent:** 94 kB
* **Number of HTTP Requests:** 1260
* **Average Request Duration:** 478.04 milliseconds
* **Average Iteration Duration:** 956.44 milliseconds
* **Throughput:** Approximately 20.83 requests/second
* **Number of Iterations:** 630
* **VUs:** 10

Comparing the three processing strategies, it is clear that the Dataflow Process has the highest throughput, processing requests more than twice as fast as the Semaphore Process, and ten times faster than the Single Process. 

## Gantt Diagrams

The following diagrams illustrate three different approaches to processing data: a single-threaded process, a semaphore-controlled process that limits concurrency to 4 concurrent tasks, and a dataflow process where each block is allowed to process up to 4 tasks concurrently. Each approach executes a set process in which it:
1. retreives *n* amount of data (130ms simulated I/O per req(1))
2. transforms that data into *n*\*10 items (300ms simulated CPU per req(*n*))
3. saves those items (30ms simulated I/O per req(*n*\*10))

```mermaid
    gantt
        title Single process (10)
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
        title Semaphore process (10)
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
        title Dataflow process (10)
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