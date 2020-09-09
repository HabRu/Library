using Library.Models;
using Library.ViewModels;
using System.Threading.Tasks;

namespace Library.Services.BookContorlServices
{
    public interface IBookControlService
    {
        Task AddBook(AddBookViewModel model);
        Task DeleteBook(int? id);
        AllListBookViewModel ListBook(BookFilterModel model);
        Task<BookViewModel> GetThisBook(int? id);
        Task AddComment(CommentViewModel comment, string NameUser);
        Task AddEvaluation(EvaluationViewModel evaluation);
        void Edit(EditBookViewModel edit);
        EditBookViewModel Edit(int? id);
    }
}
