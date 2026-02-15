using CutURLTask.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace CutURLTask.Services
{
    public static class GeneratorService
    {
        private static char[] _chars = (
            "0123456789" +
            "abcdefghijklmnopqrstuvwxyz" +
            "ABCDEFGHIJKLMNOPQRSTUVWXYZ" +
            "абвгдеёжзийклмопрсуфхцчшщъыьэюя" +
            "АБВГДЕЁЖЗИЙКЛМОПРСУФХЦЧШЩЪЫЬЭЮЯ").ToCharArray();

        /// <summary> 
        /// Генерация уникального кода для короткой ссылки 
        /// </summary> 
        /// <param name="context">DbContext для проверки уникальности</param> 
        /// <param name="length">Длина кода (по умолчанию 8)</param>
        /// <returns>Уникальный код</returns> 
        public static async Task<string> GenerateCodeAsync(CutUrlDbContext context, int length = 8)
        {
            string code;
            do
            {
                code = GenerateCode(length);
            }
            while
            (await context.UrlRecords
            .AsNoTracking()
            .AnyAsync(u => u.Code == code));
            return code;
        }

        /// <summary> 
        /// Генерация случайного кода из заданного диапазона символов
        /// </summary> 
        private static string GenerateCode(int length = 8)
        {
            var chars = RandomNumberGenerator.GetItems<char>(_chars, length);
            return new string(chars);
        }

        /// <summary>
        /// Вытаскивание из короткой ссылки только её код
        /// </summary>
        /// <param name="shortUrl">короткая строка-url</param>
        /// <returns></returns>
        public static string ExtractCode(string shortUrl)
        {
            if (Uri.TryCreate(shortUrl, UriKind.Absolute, out var uri))
            { // Берём последний сегмент пути
                return uri.Segments.Last().Trim('/');
            }
            else
            {
                // Если пришёл просто код без домена 
                return shortUrl.Trim('/');
            }
        }



    }
}
