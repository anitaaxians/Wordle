using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wordle.Data;

namespace Wordle.Controllers
{

    [ApiController]
    [Route("api/[controller]")]

    public class WordleController : ControllerBase
    {

        public ApplicationDbContext _context;
        private static int guessCount = 0;
        private const int maxTries = 6;
     


        public WordleController(ApplicationDbContext context)
        {
            _context = context;
        }

        //1 tabel -> krejt fjalt  ->bool FjalaEDites
        //HANGFIRE -> 


        [HttpPost("guess")]
        public async Task<IActionResult> WordGuess([FromBody] string guess)
        {
           
            guess = guess.ToUpper();


            if(guess.Length != 5)
            {
                return BadRequest("Worlde has only 5 letters");
            }

                //merr prej db krejt fjalt qe jan true
                var wordList = await _context.AllWords.Where(x => x.WordOfDay).ToListAsync();

                
                if (!wordList.Any())
                {
                    return BadRequest("Try another word");
                }

               // var random = new Random();
                var chosenWord = wordList.First().Word;


            var result = new List<string>();

            for (int i = 0; i < 5; i++)
            {
                if (guess[i] == chosenWord[i])
                {
                    result.Add("correct");
                    

                }else if (chosenWord.Contains(guess[i]))
                {
                    result.Add("present");
                }
                else
                {
                    result.Add("absent");
                }
            }

            guessCount++;


            if(guessCount >= maxTries)
            {
                var allWords = await _context.AllWords.ToListAsync();

                foreach(var word in allWords)
                {
                    word.WordOfDay = false;
                }

                var rndm = new Random();
                var newWord = allWords[rndm.Next(allWords.Count)];
                newWord.WordOfDay = true;
            }

            await _context.SaveChangesAsync();
            guessCount = 0;

            return Ok(result);
        }
    }
}
