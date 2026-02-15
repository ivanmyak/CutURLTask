using CutURLTask.Entities;

namespace CutURLTask.Models
{
    public class DetailsUrlViewModel
    {
        public DetailsUrlViewModel()
        {
        }

        public DetailsUrlViewModel(UrlRecord record, string? baseDomain = "localhost")
        {
            Id = record.Id;
            LongUrl = record.LongUrl;
            Code = record.Code;
            ShortUrl = $"{baseDomain}/{record.Code}";
            UsedCount = record.UsedCount;
            CreatedAt = record.CreatedAt;
        }

        public int Id { get; set; }

        public string LongUrl { get; set; } = default!;

        public string Code { get; set; } = default!;

        public string ShortUrl { get; set; } = default!;

        public int UsedCount { get; set; } = 0;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
