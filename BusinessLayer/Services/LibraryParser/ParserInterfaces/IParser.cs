using System.Threading.Tasks;

namespace BusinessLayer.Services.LibraryParser.ParserInterfaces
{
    public interface IParser<T> where T : class
    {
        T Parse(string href);
        Task<T> ParseAsync(string href);
    }
}
