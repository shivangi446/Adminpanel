using Project_Infrastructure.EntityModels;
using Project_Infrastructure.IRepository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Project_Infrastructure.Repository
{
    public class TestRepository : ITestRepository
    {
        private readonly ApplicationEntityDbContext _dbContext;
        public TestRepository(ApplicationEntityDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> AddService(TestTable model)
        {
            try
            {
                await _dbContext.TestTable.AddAsync(model).ConfigureAwait(false);
                return await _dbContext.SaveChangesAsync().ConfigureAwait(false) > 0;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
