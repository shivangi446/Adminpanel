using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Project_Service.IService;
using Project_Utility.Constants;
using Project_ViewModels.RequestViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TestProjectUI.Models;
using RestSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using System.Windows;
namespace TestProjectUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITestService _testService;
        
        List<SelectListItem> packls = new List<SelectListItem> ();
        List<SelectListItem> sizels = new List<SelectListItem>();
        List<SelectListItem> Brandls = new List<SelectListItem> ();
        List<SelectListItem> ls1 = new List<SelectListItem> ();
        List<SelectListItem> ls = new List<SelectListItem> ();
        List<SelectListItem> colordict = new List<SelectListItem>();
        string Pid;
        public HomeController(ILogger<HomeController> logger, ITestService testService)
        {
            _logger = logger;
            _testService = testService;
        }

        public IActionResult Index()
        {
            ViewBag.WelcomeMessage = string.Format(Constants.WelcomeMessage, "Shivangi");
            
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> TestView()
        {
            var model = new TestTableRequestViewModel()
            {
                Name = "Test Data"
            };
            await _testService.AddService(model).ConfigureAwait(false);
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult products(Productls psl, IFormCollection fm)
        {
            products();
            string HSNCode = fm["HSN Code"].ToString();
            string Categoryname = fm["Category"].ToString();
            string Brandname = fm["Brands"].ToString();
            string PSize = fm["psize"].ToString();
            string PColor = fm["pcolor"].ToString();
            string Pnumpacks = fm["numpacks"].ToString();
            if (ModelState.IsValid)
            {
                 var client = new RestClient("https://localhost:44391/api/Product/insproductmaster");
                 client.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
                 client.Timeout = -1;
                 var request = new RestRequest(Method.POST);
                 request.AddHeader("Content-Type", "application/json");
                 var body = @"{  " + "\n" +
                                 @"  ""cmid"": '"+Categoryname+"',  " + "\n" + @"  ""brandid"":'"+Brandname+"', " + "\n" +
                                 @"  ""pname"": '"+ psl.Productname + "', " + "\n" +
                                 @"  ""ld"": '"+psl.LD+"', " + "\n" + @"  ""sd"": '"+psl.SD+"', " + "\n" +
                                 @"  ""mrp"": '"+psl.MRP+"', " + "\n" +
                                 @"  ""dp"": '"+psl.DP+"',  " + "\n" + @"  ""availqty"":'"+psl.Availqty+"', " + "\n" +
                                 @"  ""status"": '"+psl.status+"', " + "\n" +
                                 @"  ""gid"": '"+HSNCode+"' " + "\n" +
                                 @"}";
                 request.AddParameter("application/json", body, ParameterType.RequestBody);
                 IRestResponse response = client.Execute(request);
                var clients = new RestClient("https://localhost:44391/api/Product/getproID?pname="+psl.Productname+"");
                clients.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
                clients.Timeout = -1;
                var requestS = new RestRequest(Method.POST);
                IRestResponse responseS = clients.Execute(requestS);
                var jObject = JsonConvert.DeserializeObject<dynamic>(responseS.Content.ToString());
                foreach (JObject item in jObject)
                {
                    Pid = item.GetValue("Pid").ToString();

                }
                for (int i = 0; i < PColor.Split(',').Count();i++)
                {
                    var clientcolor = new RestClient("https://localhost:44391/api/Product/inscolormaster?CMID="+ PColor.Split(',')[i] + "&ProID="+Pid+"&StatusID="+psl.status+"");
                    clientcolor.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
                    clientcolor.Timeout = -1;
                    var request1 = new RestRequest(Method.POST);
                    IRestResponse response1 = clientcolor.Execute(request1);
                }
                for (int i = 0; i < PSize.Split(',').Count(); i++)
                {
                    var clientsize = new RestClient("https://localhost:44391/api/Product/inssizemaster?SMID=" + PSize.Split(',')[i] + "&ProID=" + Pid + "&StatusID=" + psl.status + "");
                    clientsize.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
                    clientsize.Timeout = -1;
                    var request1 = new RestRequest(Method.POST);
                    IRestResponse response1 = clientsize.Execute(request1);
                }
                for (int i = 0; i < Pnumpacks.Split(',').Count(); i++)
                {
                    var Pnumpack = new RestClient("https://localhost:44391/api/Product/inspackmaster?MPackID=" + Pnumpacks.Split(',')[i] + "&ProID=" + Pid + "&StatusID=" + psl.status + "");
                    Pnumpack.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
                    Pnumpack.Timeout = -1;
                    var request2 = new RestRequest(Method.POST);
                    IRestResponse response2 = Pnumpack.Execute(request2);
                }
                ModelState.AddModelError("", "Inserted Successfully");
                IList<productmodel> prodyctdetailstbl = getproductdata();
                ViewBag.tblProduct = prodyctdetailstbl;

                return View();
            }
            return View(psl);
        }


        [HttpGet]
        public ActionResult products()
        {
            var client = new RestClient("https://localhost:44391/api/Login/ReadGSTInfo");       
            client.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            var jObject = JsonConvert.DeserializeObject<dynamic>(response.Content.ToString());
            foreach (JObject item in jObject)
            {
                string name = item.GetValue("HSNCode").ToString();
                string gid = item.GetValue("GId").ToString();
                ls1.Add(new SelectListItem { Text=name , Value=gid});
            }
            // return ls1;
            // List<string> result = MyJsonList(response, "HSNCode");
            ViewBag.gstinfo = ls1;
            var MainCat = new RestClient("https://localhost:44391/api/Category/MainCategory");
            MainCat.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            MainCat.Timeout = -1;
            var request1 = new RestRequest(Method.GET);
            request1.AddHeader("Content-Type", "application/json");
           // var body = @"{" + "\n" + @"  ""cmid"": 0," + "\n" +  @"  ""parentid"": 0," + "\n" + @"  ""cname"": ""string""," + "\n" +  @"  ""cdescription"": ""string""," + "\n" +  @"  ""status"": 0," + "\n" + @"  }";
           // request1.AddParameter("application/json", body, ParameterType.RequestBody);    
            IRestResponse response1 = MainCat.Execute(request1);
            var jObject1 = JsonConvert.DeserializeObject<dynamic>(response1.Content.ToString());
            List<SelectListItem> maincatlist = new List<SelectListItem> { };
            foreach (JObject item in jObject1)
            {
                string name = item.GetValue("Cname").ToString();
                string cmid = item.GetValue("Cmid").ToString();
                maincatlist.Add(new SelectListItem { Text=name , Value=cmid});
            }
            ViewBag.maincat = maincatlist;
          //  TempData["items"] = new SelectList(ls);
            //var model = new Productls();
           // model.Categoryname = maincatlist;
           // model.HSNCode = result;
            List<SelectListItem> psize = productsize();
            ViewBag.prosize = psize;
            // List<string> pcolor = productcolor();
            //ViewBag.procolor = new SelectList(pcolor);
            List<SelectListItem> pcolor = productcolor();
            TempData["pcolors"] = pcolor;
            List<SelectListItem> pPack = productpackage();
            ViewBag.propack = pPack;
            List<SelectListItem> Brandcat = BrandCat();
            TempData["brandcat"] = Brandcat;
            IList < productmodel > prodyctdetailstbl = getproductdata();
            ViewBag.tblProduct = prodyctdetailstbl;
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Categories(CatList cs , IFormCollection form)
        {
            string Parentname = form["Category"].ToString();
            Categories();
            if (ModelState.IsValid)
            {
                if (Parentname == "")
                {
                    Parentname = "0";
                }
                var client = new RestClient("https://localhost:44391/api/Category/insSubCategorymaster");
                client.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                request.AddHeader("Content-Type", "application/json");
                var body = @"{  " + "\n" +
                                @"  ""cmid"": 0, " + "\n" +
                                @"  ""parentid"": '"+Parentname+"',  " + "\n" +
                                @"  ""cname"": '"+cs.CatName+"',  " + "\n" +
                                @"  ""cdescription"": '"+cs.CD+"',  " + "\n" +
                                @"  ""status"": '"+cs.status+"' " + "\n" + @"}";
                request.AddParameter("application/json", body, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                ModelState.AddModelError("", "Inserted Successfully");
                IList<Categorymodel> category = populatecategory();
                ViewBag.protab = category;
                return View();
            }
            return View(cs);  
        }


        [HttpGet]
        public ActionResult Categories()
        {
            var client = new RestClient("https://localhost:44391/api/Category/categorymaster");
            client.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Content-Type", "application/json");
           // var body = @"{" + "\n" + @"    ""cmid"":0,""parentid"":0,""cname"":""string"",""cdescription"":""string"",""status"":0" + "\n" + @"    }";
          //  request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            var jObject = JsonConvert.DeserializeObject<dynamic>(response.Content.ToString());
            foreach (JObject item in jObject)
            {
                string name = item.GetValue("Cname").ToString();
                string cmid = item.GetValue("Cmid").ToString();
                ls.Add(new SelectListItem { Text = name, Value = cmid });          
            }
            TempData["items"] = ls;
            IList<Categorymodel> category = populatecategory();
            ViewBag.protab = category;
            return View();  
        }

        private static List<Categorymodel> populatecategory()
        {
            var MainCat = new RestClient("https://localhost:44391/api/Category/MainCategory");
            MainCat.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            MainCat.Timeout = -1;
            var request1 = new RestRequest(Method.GET);
            request1.AddHeader("Content-Type", "application/json");
            //var body1 = @"{" + "\n" + @"  ""cmid"": 0," + "\n" + @"  ""parentid"": 0," + "\n" + @"  ""cname"": ""string""," + "\n" + @"  ""cdescription"": ""string""," + "\n" + @"  ""status"": 0," + "\n" + @"  }";
           // request1.AddParameter("application/json", body1, ParameterType.RequestBody);
            IRestResponse response1 = MainCat.Execute(request1);
            var jObject1 = JsonConvert.DeserializeObject<dynamic>(response1.Content.ToString());
            List<string> maincatlist = new List<string> { };
            List<Categorymodel> category = new List<Categorymodel>();
            foreach (JObject item in jObject1)
            {
                string cmid = item.GetValue("Cmid").ToString();
                string pid = item.GetValue("Parentid").ToString();
                string Cname = item.GetValue("Cname").ToString();
                string CD = item.GetValue("Cdescription").ToString();
                string status = item.GetValue("Status").ToString();
                string isdelete = item.GetValue("IsDelete").ToString();
                category.Add(new Categorymodel { Cmid = cmid, Catname = pid, Cname =Cname, CDescription = CD, cstatus = status, cdelete = isdelete });
                //maincatlist.Add(name);
            }
            return category;
        }


        public List<SelectListItem> productsize()
        {
            var client = new RestClient("https://localhost:44391/api/Filter/filtersizemaster");
            client.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            var body = @"{ " + "\n" +  @"  ""product_id"": 0,   " + "\n" +  @"  ""category"": 0, " + "\n" +    @"  ""size"": 0, " + "\n" +  @"  ""color"": 0, " + "\n" + @"  ""package"": 0, " + "\n" +   @"  ""tag"": 0 " + "\n" +  @"}";
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            var jObject = JsonConvert.DeserializeObject<dynamic>(response.Content.ToString());
            foreach (JObject item in jObject)
            {
                string name = item.GetValue("Stype").ToString();
                string smid = item.GetValue("SMId").ToString();
                sizels.Add(new SelectListItem { Text=name , Value=smid});
            }
            return sizels;
        }

        public List<SelectListItem> BrandCat()
        {
            var client = new RestClient("https://localhost:44391/api/Category/BrandCategory");
            client.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Content-Type", "application/json");
           // var body1 = @"{" + "\n" + @"  ""cmid"": 0," + "\n" + @"  ""parentid"": 0," + "\n" + @"  ""cname"": ""string""," + "\n" + @"  ""cdescription"": ""string""," + "\n" + @"  ""status"": 0," + "\n" + @"  }";
           // request.AddParameter("application/json", body1, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            var jObject = JsonConvert.DeserializeObject<dynamic>(response.Content.ToString());
            foreach (JObject item in jObject)
            {
                string name = item.GetValue("Brandname").ToString();
                string bid = item.GetValue("Bid").ToString();
                Brandls.Add(new SelectListItem { Text=name , Value=bid});
            }
            return Brandls;
        }


        public List<SelectListItem> productcolor()
        {
            var client = new RestClient("https://localhost:44391/api/Filter/filtercolormaster");
            client.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            var body = @"{ " + "\n" + @"  ""product_id"": 0,   " + "\n" + @"  ""category"": 0, " + "\n" + @"  ""size"": 0, " + "\n" + @"  ""color"": 0, " + "\n" + @"  ""package"": 0, " + "\n" + @"  ""tag"": 0 " + "\n" + @"}";
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            var jObject = JsonConvert.DeserializeObject<dynamic>(response.Content.ToString());
            foreach (JObject item in jObject)
            {
                string name = item.GetValue("Ctype").ToString();
                string CMid = item.GetValue("CMId").ToString();
                colordict.Add(new SelectListItem() { Text = name, Value = CMid });
               // colorls.Add(name);
            }
            return colordict;
        }

        public List<SelectListItem> productpackage()
        {
            var client = new RestClient("https://localhost:44391/api/Filter/filterpackagemaster");
            client.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            var body = @"{ " + "\n" + @"  ""product_id"": 0,   " + "\n" + @"  ""category"": 0, " + "\n" + @"  ""size"": 0, " + "\n" + @"  ""color"": 0, " + "\n" + @"  ""package"": 0, " + "\n" + @"  ""tag"": 0 " + "\n" + @"}";
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            var jObject = JsonConvert.DeserializeObject<dynamic>(response.Content.ToString());
            foreach (JObject item in jObject)
            {
                string name = item.GetValue("PackName").ToString();
                string MPackid = item.GetValue("MPackID").ToString();
                packls.Add(new SelectListItem { Text=name,Value=MPackid});
            }
            return packls;
        }

        public static List<productmodel> getproductdata()
        {
            var client = new RestClient("https://localhost:44391/api/Product/getproductdetail");
            client.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            var jObject1 = JsonConvert.DeserializeObject<dynamic>(response.Content.ToString());
            List<string> mainprolist = new List<string> { };
            List<productmodel> productdata = new List<productmodel>();
            foreach (JObject item in jObject1)
            {
                string pname = item.GetValue("Pname").ToString();
                string cname = item.GetValue("Cname").ToString();
                string ld = item.GetValue("LDdescription").ToString();
                string mrp = item.GetValue("MRP").ToString();
                string dp = item.GetValue("DP").ToString();
                string size = item.GetValue("Stype").ToString();
                string color = item.GetValue("Ctype").ToString();
                string status = item.GetValue("status").ToString();
                productdata.Add(new productmodel { productName = pname, Product_category = cname, Description = ld, mrp=mrp,dp = dp, size = size, color=color ,status=status });
                
            }

            return productdata;
                
        }
        public ActionResult gst()
        {
            return View();
        }
        public ActionResult District()
        {
            return View();
        }
        public ActionResult City()
        {
            return View();
        }
        public ActionResult State()
        {
            return View();
        }
    }
}
