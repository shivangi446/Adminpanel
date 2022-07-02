using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Project_Infrastructure.EntityModels
{
    public class TestTable
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
