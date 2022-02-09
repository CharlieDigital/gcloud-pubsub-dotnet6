using Google.Api.Gax;
using Google.Cloud.PubSub.V1;
using Grpc.Core;
using Grpc.Core.Logging;

GrpcEnvironment.SetLogger(new ConsoleLogger());

Console.WriteLine("Starting subscriber...");

var projectId = "gps-demo";
var topicId = "test-pub";
var subscriptionId = "test-sub";

var subscriberService = await new SubscriberServiceApiClientBuilder
{
    EmulatorDetection = EmulatorDetection.EmulatorOrProduction
}.BuildAsync();

var subscriptionName = new SubscriptionName(projectId, subscriptionId);
var topicName = new TopicName(projectId, topicId);

if (subscriberService.GetSubscription(subscriptionName) == null)
{
    subscriberService.CreateSubscription(subscriptionName,
        topicName, pushConfig: null, ackDeadlineSeconds: 60);
}

var subscriber = await SubscriberClient.CreateAsync(
    subscriptionName,
    new SubscriberClient.ClientCreationSettings()
        .WithEmulatorDetection(EmulatorDetection.EmulatorOrProduction));

await subscriber.StartAsync((msg, cancellationToken) =>
{
    Console.WriteLine($"Received message {msg.MessageId} published at {msg.PublishTime.ToDateTime()}");
    Console.WriteLine($"Text: '{msg.Data.ToStringUtf8()}'");

    if (msg.Data.ToStringUtf8() == "quit")
    {
        subscriber.StopAsync(TimeSpan.FromSeconds(15));
    }

    return Task.FromResult(SubscriberClient.Reply.Ack);
});
