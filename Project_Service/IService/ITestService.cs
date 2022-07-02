using Project_ViewModels.RequestViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Project_Service.IService
{
    public interface ITestService
    {
        Task<bool> AddService(TestTableRequestViewModel model);
    }
}
