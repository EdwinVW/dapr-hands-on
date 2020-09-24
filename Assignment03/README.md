# Assignment 3 - Add Dapr state management

In this assignment, you're going to add Dapr **state management** in the TrafficControl service to store vehicle information.

## Dapr State management building block

Dapr offers key/value storage APIs for state management. If a microservice uses state management, it can use these APIs to leverage any of the supported state stores, without adding or learning a third party SDK.

When using state management your application will also be able to leverage several other features that would otherwise be complicated and error-prone to build yourself such as:

- Distributed concurrency and data consistency
- Retry policies
- Bulk CRUD operations

See below for a diagram of state management's high level architecture:

![](img/state_management.png)

For this hands-on assignment, this is all you need to know about this building-block. If you want to get more detailed information, read the [introduction to this building-block](https://github.com/dapr/docs/blob/master/concepts/state-management/README.md) in the Dapr documentation.

## Assignment goals

In order to complete this assignment, the following goals must be met:

- The TrafficControl service saves the state of a vehicle (VehicleState class) using the state management building block after vehicle entry.
- The TrafficControl service reads, updates and saves the state of a vehicle using the state management building block after vehicle exit.

For both these tasks you can use the Dapr client for .NET.

## DIY instructions

First open the `Assignment 3` folder in this repo in VS Code. Then open the [Dapr documentation](https://github.com/dapr/docs) and start hacking away. Make sure you use the default Redis state-store component provided out of the box by Dapr.

## Step by step instructions

To get step-by-step instructions to achieve the goals, open the [step-by-step instructions](step-by-step.md).

## Next assignment

Make sure you stop all running processes before proceeding to the next assignment.

Go to [assignment 4](../Assignment04/README.md).
