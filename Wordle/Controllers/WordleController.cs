using Microsoft.AspNetCore.Mvc;

namespace Wordle.Controllers
{

    [ApiController]
    [Route("api/[controller]")]

    public class WordleController : ControllerBase
    {

        //1 tabel -> krejt fjalt  ->bool FjalaEDites
        //HANGFIRE -> 
     
        static readonly string[] WordleList = { "APPLE", "GRAPE", "PLANT", "CLOUD", "MOUSE", "BRAIN", "STONE", "LEMON", "BRUSH", "TRAIN", "CRANE", "PLANE", "SHEEP", "SNAKE", "LIGHT", "WATER", "TREES", "FLOOR", "CHAIR", "SHINE", "GLASS", "SWEET", "SOUND", "BRICK", "STORM" };
        static string FinalWordle = WordleList[new Random().Next(WordleList.Length)];

        [HttpGet("final")]
        public string GetFinal() => FinalWordle;

        [HttpPost("guess")]
        public IActionResult WordGuess([FromBody] string guess)
        {
            guess = guess.ToUpper();
            var result = new List<string>();

            for(int i = 0; i < guess.Length; i++)
            {
                if (guess[i] == FinalWordle[i])
                {
                    result.Add("correct");
                }
                else if (FinalWordle.Contains(guess[i]))
                {
                    result.Add("present");
                }
                else
                {
                    result.Add("absent");
                }
            }
            return Ok(result);
        }

        [HttpPost("restart")]
        public IActionResult Restart()
        {
            FinalWordle = WordleList[new Random().Next(WordleList.Length)];
            return Ok("Game restarted");
        }

    }
}
