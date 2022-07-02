using AutoMapper;
using Project_Infrastructure.EntityModels;
using Project_Infrastructure.IRepository;
using Project_Service.IService;
using Project_ViewModels.RequestViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Project_Service.Service
{
    public class TestService : ITestService
    {
        private readonly ITestRepository _testRepository;
        private readonly IMapper _mapper;
        public TestService(ITestRepository testRepository, IMapper mapper)
        {
            _testRepository = testRepository;
            _mapper = mapper;
        }

        public async Task<bool> AddService(TestTableRequestViewModel model)
        {
            try
            {
                return await _testRepository.AddService(_mapper.Map<TestTable>(model)).ConfigureAwait(false);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
