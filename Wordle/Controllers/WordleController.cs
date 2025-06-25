using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wordle.Data;
using Wordle.Services;

namespace Wordle.Controllers
{

    [ApiController]
    [Route("api/[controller]")]

    public class WordleController : ControllerBase
    {

        public ApplicationDbContext _context;
        private readonly SessionServices _sessionServices;
        private const int maxTries = 6;

        public WordleController(ApplicationDbContext context, SessionServices sessionServices)
        {
            _context = context;
            _sessionServices = sessionServices;
        }

        //1 tabel -> krejt fjalt  ->bool FjalaEDites
        //HANGFIRE -> 


        [HttpPost("guess")]
        public async Task<IActionResult> WordGuess([FromBody] string guess)
        {

            var userData = _sessionServices.GetCookie();
            userData ??= new();

            if (userData.GuessDate.Date == DateTime.Now.Date && userData.HasFinished)
            { 
                
            }
            if (userData.GuessDate.Date != DateTime.Now.Date)
            {
                userData.GuessCount = 0;
            }

            if (userData.GuessCount >= maxTries)
            {
                return BadRequest("You run out of tries");
            }

            guess = guess.ToUpper();


            if (guess.Length != 5)
            {
                return BadRequest("Worlde has only 5 letters");
            }

            //merr prej db krejt fjalt qe jan true
            var wordList = await _context.AllWords.Where(x => x.WordOfDay).FirstOrDefaultAsync();


            if (wordList is null)
            {
                return BadRequest("Something went wrong");
            }


            var chosenWord = wordList.Word;

            var result = new List<string>();

            for (int i = 0; i < 5; i++)
            {
                if (guess[i] == chosenWord[i])
                {
                    result.Add("correct");
                }
                else if (chosenWord.Contains(guess[i]))
                {
                    result.Add("present");
                }
                else
                {
                    result.Add("absent");
                }
            }
            userData.GuessCount++;
            userData.HasFinished = !result.Contains("absent") && !result.Contains("present");
            userData.GuessDate = DateTime.Now;

            _sessionServices.SetCookie(userData);



            //if (guessCount >= maxTries)
            //{
            //    var allWords = await _context.AllWords.ToListAsync();

            //    foreach (var word in allWords)
            //    {
            //        word.WordOfDay = false;
            //    }

            //    var rndm = new Random();
            //    var newWord = allWords[rndm.Next(allWords.Count)];
            //    newWord.WordOfDay = true;
            //}

            //await _context.SaveChangesAsync();
            //guessCount = 0;

            return Ok(new { 
                result, 
                hasWon = userData.HasFinished,
                remainingTries = maxTries - userData.GuessCount
            });
        }
    }
}
