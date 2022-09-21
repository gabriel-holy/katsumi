# Katsumi API
## _The monolithic MVP lab was created to explore and learn architectural concepts_

[![.NET Build and Test](https://github.com/gabriel-holy/katsumi/actions/workflows/dotnet.yml/badge.svg)](https://github.com/gabriel-holy/katsumi/actions/workflows/dotnet.yml)

***

As an object of study, I've chosen a social media context as a core product to be fed with this application.

My intention, in this case, is to demonstrate that we could start any basic structure smaller and simple, caring about good writing, and seeking evolutions during the time. 

As long we evolve within the problem, we can easily decouple this primary service into many others, reaching an evolutional microservices mesh as a long-term goal, without giving up the product needs on the other hand.

## Installation

Katsumi is currently in [Donet](https://dotnet.microsoft.com/en-us/) 5.0.0+ version.
Download a clone branch, and start the server as follow below. 
```sh
dotnet build
dotnet test
dotnet run --project src/KatsumiApp/KatsumiApp.csproj
```

Finally, access your Swagger (https://localhost:14001/index.html) page and try some actions

## Key Features of Katsumi 

I proposed the following key features to this first API version:

* Follow and unfollow any person, except yourself
* View any profile 
* Create, comment, and repost some simple postages

## Why a monolithic application?

Monoliths are not villains, we had been used to abominate this architectural pattern, but it solves some problems that we should mention here.

* Cost: start-ups have no money to spend, or sometimes they are simply testing the market with an idea
* Time to market: Start-ups need to run fast their initial products to obtain feedback from customers. Running a microservice mesh is cool and solves a certain set of problems, but it demands more cognitive comprehension, and good domain modeling as well. An organized and clean monolithic could bring good value initially, meanwhile, we think about the evolution of the context

## Learning Key Objectives and Learning Goals

In many ways, I tried to answer the given questions with that application

* How could I build an uncoupled domain without creating more than one service initially?
* Building a unique application, which precautions should I care to not make unfeasible future domain splitting?
* How to do not over-engineering?
* How could I write a very simple and legible code?
* A testable code, how could I reach that from zero?
* What about pipelines and containers? 
* And the most important for me: How simple is the code for a new recently hired junior developer?


## Tech

As solution design, I chose [Vertical Slice](https://jimmybogard.com/vertical-slice-architecture/), it is the real magic running here, uncoupling the service horizontally, producing simplicity, and avoiding side-effects symptoms when we got in trouble.
The features are independent of each other, and the architecture yells the splitting goals that we must pursue.

- [Donet] - looking for the last versions as long as I can
- [Entity Framework Core] - as an initial data repository for tests
- [Docker] - as the initial container method for tests

## To Do

The following list contains some features that I must craft as long as I can

- Docker composing 
- KEDA 
- Publish on AKS
- Publish on GCP
- Load tests with K6 + Grafana
- Observability and tracing
- Remove Entity Framework in Memory and store the data on RavenDB
- Enrich the Domain and bring Domain Events as well
- Expand the features and reduce code complexity
- Build a micro front for a fun

## License

MIT

***

**The application name goes in homage to my beloved daughter, Katarina Katsumi Koyanagi Oliveira.**
**I love you child, and I hope that you could see this code in the future and think _" what an obsolete mess, Daddy!"_**

