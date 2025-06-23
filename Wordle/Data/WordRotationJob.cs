using Microsoft.EntityFrameworkCore;

namespace Wordle.Data
{
    public class WordRotationJob
    {

        private readonly ApplicationDbContext _context;

        public WordRotationJob(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task RotateWordsDaily()
        {
            var allWords = await _context.AllWords.ToListAsync();

            foreach (var word in allWords)
            {
                word.WordOfDay = false;
            }

            var rndm = new Random();
            var newWord = allWords[rndm.Next(allWords.Count)];
            newWord.WordOfDay = true;


            await _context.SaveChangesAsync();
        }
    }
}
