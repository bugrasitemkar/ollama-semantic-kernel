using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.Ollama;

var builder = Kernel.CreateBuilder();
builder.AddOllamaChatCompletion("smollm:135m", new Uri("http://localhost:11434"));

var kernel = builder.Build();
var chatService = kernel.GetRequiredService<IChatCompletionService>();

var chatHistory = new ChatHistory();
chatHistory.AddSystemMessage("You are a helpful assistant.");

while (true)
{
    Console.Write("User: ");
    var userMessage = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(userMessage) || userMessage.ToLower() == "exit")
        break;

    chatHistory.AddUserMessage(userMessage);

    var response = await chatService.GetChatMessageContentAsync(chatHistory);

    Console.WriteLine($"Assistant: {response.Content}");

    chatHistory.AddMessage(response.Role, response.Content ?? string.Empty);
}