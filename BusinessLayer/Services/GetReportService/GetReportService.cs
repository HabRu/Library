using BusinessLayer.InrefacesRepository;
using BusinessLayer.ViewModels;
using Library.Models;
using NPOI.SS.Formula;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services.GetReportService
{
    public class GetReportService : IGetReportService
    {
        private readonly IRepository<Reservation> repRes;
        private readonly IRepository<Book> bookRep;

        public GetReportService(IRepository<Reservation> repRes, IRepository<Book> bookRep)
        {
            this.repRes = repRes;
            this.bookRep = bookRep;
        }

        public Stream GetReportExcel(GetReportViewModel getReport)
        {
            var workBook = new XSSFWorkbook();
            var sheet = workBook.CreateSheet("Отчет бронирвоания");

            var rowH = sheet.CreateRow(0);
            rowH.CreateCell(0).SetCellValue("Название книги");
            rowH.CreateCell(1).SetCellValue("Кол. бронирования");
            var i = 0;

            foreach (var book in bookRep.GetAll())
            {
                var ress = repRes.GetAll().Where(r => (r.BookIdentificator == book.Id && getReport.FromData <= r.DataBooking && getReport.ToData >= r.DataBooking));
                var count = ress?.Count();
                if (count > 0)
                {
                    i++;
                    var row = sheet.CreateRow(i);
                    row.CreateCell(0).SetCellValue(book.Title);
                    row.CreateCell(1).SetCellValue((double)count);
                }
            }

            sheet.AutoSizeColumn(0);

            var stream = new MemoryStream();

            workBook.Write(stream, true);
            stream.Position = 0;

            return stream;
        }
    }
}
