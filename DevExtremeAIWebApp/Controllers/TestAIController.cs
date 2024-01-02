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
            var completion = new CreateCompletionRequest();
            completion.Model = "gpt-3.5-turbo-instruct";
            string prompt = $"Is the number {id} even or odd?";
            completion.AddCompletionPrompt(prompt);
            var response = await _openAIApiClient.CreateCompletionAsync(completion);
            return $"{prompt} -> {response?.OpenAIResponse?.Choices[0]?.Text}";
        }

    }
}
