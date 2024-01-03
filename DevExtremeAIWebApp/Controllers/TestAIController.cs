using DevExtremeAI.OpenAIClient;
using DevExtremeAI.OpenAIDTO;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DevExtremeAIWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class TestAIController : ControllerBase
    {
        private IOpenAIAPIClient _openAIApiClient;
        public TestAIController(IOpenAIAPIClient openAIApiClient)
        {
            _openAIApiClient = openAIApiClient;
        }


        // GET api/<TestAIController>/5
        [HttpGet("{id}")]
        public async Task<string> Get(int id)
        {
            var chat = new CreateChatCompletionRequest();
            chat.Model = "gpt-3.5-turbo-1106";
            string prompt = $"Is the number {id} even or odd?";
            chat.AddMessage(new ChatCompletionUserContentMessage() { Content = prompt });
            var response = await _openAIApiClient.CreateChatCompletionAsync(chat);
            return $"{prompt} -> {response?.OpenAIResponse?.Choices[0]?.Message?.Content}";
        }

    }
}
