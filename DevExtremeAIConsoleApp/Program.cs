using DevExtremeAI.OpenAIClient;
using DevExtremeAI.OpenAIDTO;
using System.Diagnostics;

namespace DevExtremeAIConsoleApp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Hi, I'm Andrea Piccioni's AI, chat with me!");
            Console.WriteLine("Enter a number between 0 and 2 and press Enter (as 0.2)");
            Console.ForegroundColor = ConsoleColor.Yellow;
            var tempString = Console.ReadLine();

            var parsed = double.TryParse(tempString, out double temp);
            parsed = parsed && (temp >= 0) && (temp <= 2);
            while (!parsed)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("The number is invalid :(");
                Console.WriteLine("Enter a number between 0 and 2 and press Enter (as 0.2)");
                Console.ForegroundColor = ConsoleColor.Yellow;
                tempString = Console.ReadLine();
                parsed = double.TryParse(tempString, out temp);
                parsed = parsed && (temp >= 0) && (temp <= 2);
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Press Enter to send me your message or CTRL+C to finish");

            var openAIClient = OpenAIClientFactory.CreateInstance();

            CreateChatCompletionRequest createCompletionRequest = new CreateChatCompletionRequest();
            createCompletionRequest.Model = "gpt-3.5-turbo-1106";
            createCompletionRequest.Temperature = temp;

            while (true)
            {
                
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("YOU:");
                var chat = Console.ReadLine();
                if (chat.StartsWith("YOU:"))
                {
                    chat = chat.Substring("YOU:".Length);
                }

                createCompletionRequest.Messages.Add(new ChatCompletionRequestMessage()
                {
                    Role = ChatCompletionMessageRoleEnum.User,
                    Content = chat,

                });

                var response = await openAIClient.CreateChatCompletionAsync(createCompletionRequest);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(response.OpenAIResponse.Choices[0].Message.Content);

                createCompletionRequest.Messages.Add(new ChatCompletionRequestMessage()
                {
                    Role = response.OpenAIResponse.Choices[0].Message.Role,
                    Content = response.OpenAIResponse.Choices[0].Message.Content
                });

            }

        }
    }
}