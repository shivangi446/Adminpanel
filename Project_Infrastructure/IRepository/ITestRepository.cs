using Project_Infrastructure.EntityModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Project_Infrastructure.IRepository
{
    public interface ITestRepository
    {
        Task<bool> AddService(TestTable model);
    }
}
