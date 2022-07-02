using Microsoft.AspNetCore.Mvc;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestProjectUI.Models;

namespace TestProjectUI.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Login(LoginUser lc)
        {
            if (ModelState.IsValid)
            {
                var client = new RestClient("https://localhost:44391/api/Login/LoginAuth");
                client.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");
                var body = @"{" + "\n" + @"  ""username"": '"+lc.Username+"'," + "\n" +  @"  ""password"": '"+lc.Password+"' " + "\n" +  @"}";
                request.AddParameter("application/json", body, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                
                Console.WriteLine(response.Content);
                if (response.Content == "\"1\"")
                {
                    return RedirectToAction("Index", "Home");
                }
                
            }
            if (lc.Username == null || lc.Password == null)
            {
                return View();
            }
            ModelState.AddModelError("", "Login details are wrong.");
            return View();

        }
    }
}
