using Google.Api.Gax;
using Google.Cloud.PubSub.V1;

Console.WriteLine("Starting publisher...");

var projectId = "gps-demo";
var topicId = "test-pub";

var publisherService = await new PublisherServiceApiClientBuilder
{
    EmulatorDetection = EmulatorDetection.EmulatorOrProduction
}.BuildAsync();

var topicName = new TopicName(projectId, topicId);

if (publisherService.GetTopic(topicName) == null)
{
    publisherService.CreateTopic(topicName);
}

var publisher = await PublisherClient.CreateAsync(
    topicName,
    new PublisherClient.ClientCreationSettings()
        .WithEmulatorDetection(EmulatorDetection.EmulatorOrProduction));

Console.WriteLine(
@"Type a message and press ENTER to send.  Type 'quit' to exit
----------------------------------"
);

while (true)
{
    var text = Console.ReadLine();

    await publisher.PublishAsync(text);

    Console.WriteLine($"SENDING: {text}");

    if (text?.Trim().ToLowerInvariant() == "quit")
    {
        break;
    }
}

await publisher.ShutdownAsync(TimeSpan.FromSeconds(15));