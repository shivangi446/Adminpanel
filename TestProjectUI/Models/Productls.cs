using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TestProjectUI.Models
{
    public class Productls
    {
        public List<string> HSNCode { get; set; }
        public List<string> Categoryname { get; set; }
        [Required]
        public string Productname { get; set; }
        [Required]
        public string SD { get; set; }
        [Required]
        public string LD { get; set; }
      
        public List<SelectListItem> psize { get; set; }

        public List<SelectListItem> pcolor { get; set; }
        
        public List<SelectListItem> numpacks { get; set; }
        [Required]
        public string MRP { get; set; }
        [Required]
        public string DP { get; set; }
        public string BrandName { get; set; }
        [Required]
        public string Availqty { get; set; }
        [Required]
        public string status { get; set; }
    }
}
