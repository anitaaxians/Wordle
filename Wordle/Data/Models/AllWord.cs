using System.ComponentModel.DataAnnotations;

namespace Wordle.Data.Models
{
    public class AllWord
    {
        [Key]
        public int Id { get; set; }
        public string Word { get; set; }
        public bool WordOfDay { get; set; }

    }
}
