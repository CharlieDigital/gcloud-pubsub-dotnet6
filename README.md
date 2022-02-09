# Google Cloud Pub/Sub with .NET 6 and Docker

See the YouTube walkthrough: https://youtu.be/IyQ9SfL0IzI

This repo provides a sandbox for working with Google Cloud Pub/Sub on a local environment using Docker and .NET 6 to build publishers and subscribers.

The easiest way to get started is to grab the Google Cloud Pub/Sub Docker image: https://github.com/marcelcorso/gcloud-pubsub-emulator

If you're developing on a MacBook Pro with the M1 CPU, you'll need an additional library to work with .NET: https://github.com/einari/Grpc.Core.M1

## Motivation

When building a microservices architecture, it is very useful to be able to decouple the different microservices from synchronous interactions (e.g. direct API calls).  In some cases, this is unavoidable (latency, response time, need for synchronous response), but generally speaking, if it is possible, using a message-oriented, event-driven architecture provides:

- more scalability, 
- better resiliency,
- cleaner decoupling of services
- less complexity compared to managing service discovery
- easier testing (by virtue of decoupling)

During the initial development, it is really helpful to have a simple sandbox and local development experience to play with the different features of the system.

Google Cloud Pub/Sub is one of the few cloud messaging services that provides a local emulator (Azure Service Bus, for example, does not provide this).  By default, this emulator is executed using the `gcloud` command.  However, it is really convenient to be able to use the containerized verison of it for local testing.

This setup allows you to better understand how to interact with Google Cloud Pub/Sub without investing too much effort!

See the docs here if you'd like to learn more: https://cloud.google.com/pubsub/docs/overview
## Running the Code

To run this sandbox code, start the docker image for the [Google Cloud Pub/Sub emulator](https://cloud.google.com/pubsub/docs/emulator) from the root directory:

```
docker-compose up
```

Alternatively, you can run the container directly:

```
docker run --rm -ti -p 8681:8681 -e PUBSUB_PROJECT1=gps-demo,test-pub:test-sub messagebird/gcloud-pubsub-emulator:latest
```

Next, run the publisher and the subscriber:

```
cd publisher
export PUBSUB_EMULATOR_HOST=localhost:8681
dotnet run
```

and:

```
cd subscriber
export PUBSUB_EMULATOR_HOST=localhost:8681
dotnet run
```

Then you can trigger messages from the publisher to the subscriber by simply typing messages at the command line and pressing ENTER.

If you're planning on building multiple microservices (for example, using Google Cloud Run), then you can also containerize the other parts and add them to the `docker-compose.yaml` file.

See this article on issues related to running gRPC: https://danielstoyanoff.medium.com/apple-silicon-docker-dotnet-grpc-is-that-compatible-8a05f1a71d89

You'll need to install `protoc` and then:

```
export PROTOBUF_PROTOC=/usr/bin/protoc
```

NOTE: When connecting directly through the host, use `PUBSUB_EMULATOR_HOST=host.docker.internal:8681`