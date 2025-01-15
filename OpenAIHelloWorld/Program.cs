using LangChain.Providers;
using LangChain.Providers.OpenAI;

//Get the OpenAI API Key you created (see README)
var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
ArgumentException.ThrowIfNullOrWhiteSpace(apiKey);

//Create the LLM object with the API Key and model name you want to use
var modelName = "gpt-4o-mini";
var llm = new OpenAiChatModel(apiKey, modelName);

while (true)
{
    //Get user input
    Console.Write("You:");
    var userInput = Console.ReadLine();
    ArgumentException.ThrowIfNullOrEmpty(userInput);

    //Get the generated response from the LLM
    var response = await GetResponse(userInput, llm);
    Console.Write($"AI: {response}");

    Console.WriteLine();
    Console.WriteLine();
}

static async Task<string> GetResponse(string userInput, OpenAiChatModel llm)
{
    //First, we have to build the prompt.

    //This is the system message part of the prompt where we 
    //tell it exactly how we want it to behave. 
    var systemMessage = new Message(
                    """
                    You're a helpful librarian and translator. Answer the user's question in the following format:
                    <answer to the question>

                    German translation:
                    <answer to question in German>

                    Shakespeare: 
                    <provide the answer using Shakespeare's way of writing>
                    """, MessageRole.System);

    //The user message part of the prompt. 
    var userMessage = new Message(userInput, MessageRole.Human);

    var chatRequest = new ChatRequest
    {
        Messages =
        [
            systemMessage,
            userMessage
        ]
    };

    var chatResponse = await llm.GenerateAsync(chatRequest);

    return chatResponse.ToString();
}