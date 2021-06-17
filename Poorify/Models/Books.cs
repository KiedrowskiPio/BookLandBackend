using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookLand.Models
{
    public class Books
    {
        [Required]
        public int BookId { get; set; }

        [DataType(DataType.Text)]
        [Required(AllowEmptyStrings = true)]
        public string BookTitle { get; set; }

        [DataType(DataType.Text)]
        [Required(AllowEmptyStrings = true)]
        public string BookAuthor { get; set; }

        [DataType(DataType.DateTime)]
        [Required]
        public DateTime BookRelese { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string PhotoFileName { get; set; }
    }
}
