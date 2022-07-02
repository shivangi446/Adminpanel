using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TestProjectUI.Models
{
    public class CatList
    {
      
        public List<SelectListItem> catedata { get; set; }
        [Required]
        public string CatName { get; set; }
        [Required]
        public string CD { get; set; }
        [Required]
        public string status { get; set; }
        
       // public string Imgpath { get; set; }
    }
}
