using BusinessLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services.GetReportService
{
    public interface IGetReportService
    {
        Stream GetReportExcel(GetReportViewModel getReport);
    }
}
