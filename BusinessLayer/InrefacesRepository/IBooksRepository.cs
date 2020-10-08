using Library.Models;
using Library.ViewModels;
using System.Threading.Tasks;

namespace Library.Services.BookContorlServices
{
    public interface IBooksRepository
    {
        Task AddBook(AddBookViewModel model, string pathWeb);
        Task DeleteBook(int? id);
        AllListBookViewModel ListBook(BookFilterModel model);
        Task<BookViewModel> GetThisBook(int? id);
        Task AddComment(CommentViewModel comment, string NameUser);
        Task AddEvaluation(EvaluationViewModel evaluation);
        void Edit(EditBookViewModel edit);
        Task<EditBookViewModel> Edit(int? id);
    }
}
