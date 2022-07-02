using AutoMapper;
using Project_Infrastructure.EntityModels;
using Project_ViewModels.RequestViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestProjectUI.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<TestTableRequestViewModel, TestTable>();
        }
    }
}
