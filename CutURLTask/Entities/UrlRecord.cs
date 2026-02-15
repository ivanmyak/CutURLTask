using System.ComponentModel.DataAnnotations;

namespace CutURLTask.Entities
{
    public class UrlRecord
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string LongUrl { get; set; } = default!;

        /// <summary>
        /// Код короткой URL
        /// </summary>
        [MaxLength(8)]
        public string Code { get; set; } = default!;

        public int UsedCount { get; set; } = 0;
    }
}
