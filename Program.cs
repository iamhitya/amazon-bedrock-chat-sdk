using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

var builder = Host.CreateApplicationBuilder();

builder.Services.AddBedrockChatCompletionService("amazon.titan-text-premier-v1:0");

builder.Services.AddTransient(sp =>
{
    return new Kernel(sp);
});

var app = builder.Build();

var kernel = app.Services.GetRequiredService<Kernel>();

var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

ChatHistory history = [];

while (true)
{
    Console.Write("You: ");
    var userInput = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(userInput))
    {
        Console.WriteLine("Goodbye!");
        break;
    }

    // Add user message to chat history
    history.AddUserMessage(userInput);

    Console.Write("AI: ");

    // Get AI response
    var response = await chatCompletionService.GetChatMessageContentAsync(
        history,
        kernel: kernel
    );

    // Add AI response to chat history
    history.Add(response);

    Console.WriteLine(response);
    Console.WriteLine();
}

ChatHistory streamingHistory = [];

while (true)
{
    Console.Write("You: ");
    var streamingUserInput = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(streamingUserInput))
    {
        Console.WriteLine("Exiting streaming chat. Goodbye!");
        break;
    }

    // Add user message to streaming chat history
    streamingHistory.AddUserMessage(streamingUserInput);

    Console.Write("AI (streaming): ");

    var streamingResponse = chatCompletionService.GetStreamingChatMessageContentsAsync(
        chatHistory: streamingHistory,
        kernel: kernel
    );

    var fullResponse = "";
    await foreach (var chunk in streamingResponse)
    {
        Console.Write(chunk);
        fullResponse += chunk;
        //await Task.Delay(100);
    }

    // Add AI response to streaming chat history
    streamingHistory.Add(new ChatMessageContent(AuthorRole.Assistant, fullResponse));

    Console.WriteLine();
    Console.WriteLine();
}
