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
        /// <remarks>Ограничил максимальное количество символов, чтобы и в БД занимало меньше, и было действительно "короткой" ссылкой</remarks>
        [MaxLength(8)]
        public string Code { get; set; } = default!;

        public int UsedCount { get; set; } = 0;

        /// <summary> 
        /// Дата и время создания записи 
        /// </summary> 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
