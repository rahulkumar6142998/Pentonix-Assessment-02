using Manager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.Text;
using System.IO;
using System.Threading;
using System.Security.Cryptography;
using System.Data.Entity;

namespace Manager.Controllers
{
    public class ForgetPasswordController : Controller
    {
        static string reciver;

        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(string EmailID)
        {
           
            string message = "";
            bool status = false;

            using (pentonixEntities1 _db = new pentonixEntities1())
            {
                var account = _db.Users.Where(a => a.Email == EmailID).FirstOrDefault();
                if (account != null)
                {

                    string baseUrl = string.Format("{0}://{1}",
                        Request.Url.Scheme, Request.Url.Host);
                     reciver = account.Email;

                    var code = account.Password;

                    var activationUrl = CreateCode(6);

                    var sender = "rahulkumar614299@gmail.com";

                    var body1 = "Name: " + reciver;

                    var body = @"<html>
                      <body>
                      Dear Mr/Ms </body</html>" + account.FirstName +
                              @"<html>
                      <body>
                      <br>You have got a request to verify your account for Pentonix </body> </html>" +
                             @"<html>
                      <body>
                      <br>Your code is </body> </html>" +
                             @"<html>
                      <body>
                        <br>" + activationUrl;

                    status = Send(reciver,sender,body1,body);

                    account.ResetPasswordCode = activationUrl;
                    if (ModelState.IsValid)
                    {
                        _db.Entry(account).State = EntityState.Modified;

                        try
                        {
                            _db.SaveChanges();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Put your error message here.{0}", e);
                           
                        }
                        return RedirectToAction("EnterCode");
                    }
                }
                else
                {
                    ViewBag.Message = "Account is not Registered";
                    return View();
                }


            }

            ViewBag.Message = message;
            return View();
        }


        public ActionResult EnterCode()
        {
            return View();
        }

        [HttpPost]
        public ActionResult EnterCode(string code)
        {
            using (pentonixEntities1 _db = new pentonixEntities1())
            {
                var verify = _db.Users.Where(x => x.Email == reciver).FirstOrDefault();

                if( verify.ResetPasswordCode == code)
                {
                    //return View("~/Views/ForgetPassword/ResetPassword.cshtml");
                    return RedirectToRoute(new { controller = "ForgetPassword", action = "ResetPassword" }); 
                }




            }

                return View();
        }


        public ActionResult ResetPassword()
        {

            var model = new ResetPasswordModel();
            return View(model);
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordModel model)
        {
            var message = "";
            if (ModelState.IsValid)
            {
                using (pentonixEntities1 dc = new pentonixEntities1())
                {
                    var user = dc.Users.Where(x => x.Email == reciver).FirstOrDefault();
                    if (user != null)
                    {
                        user.Password = model.NewPassword;
                        user.ConfirmPassword = model.ConfirmPassword;
                      
                        dc.Configuration.ValidateOnSaveEnabled = false;
                        dc.SaveChanges();
                        message = "New password updated successfully";
                    }
                }
            }
            else
            {
                message = "Something invalid";
            }
            ViewBag.Message = message;
            return View(model);
        }


        [NonAction]
        public bool Send(string to, string from, string subject, string content)
        {
            try
            {
                string[] Scopes = { GmailService.Scope.GmailSend };
                UserCredential credential;
                using (var stream = new FileStream(
                    @"C:\Users\RAHUL\Desktop\Manager\Manager\client_secret.json",
                    FileMode.Open,
                    FileAccess.Read
                ))
                {
                    //string credPath = "token_Send.json";
                    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                                 GoogleClientSecrets.FromStream(stream).Secrets,
                                  Scopes,
                                  "rahulkumar614299",
                                  CancellationToken.None
                                 ).Result;
                    //Console.WriteLine("Credential file saved to: " + credPath);
                }
                // Create Gmail API service.
                var service = new GmailService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "Pentonix",
                });
                //Parsing HTML 
                string message = $"To: {to}\r\nSubject: {subject}\r\nContent-Type: text/html;charset=utf-8\r\n\r\n{content}";
                var newMsg = new Message();
                newMsg.Raw = this.Base64UrlEncode(message.ToString());
                Message response = service.Users.Messages.Send(newMsg, "me").Execute();

                if (response != null)
                    return true;
                else
                    return false;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        private string Base64UrlEncode(string input)
        {
            var inputBytes = Encoding.UTF8.GetBytes(input);
            // Special "url-safe" base64 encode.
            return Convert.ToBase64String(inputBytes)
              .Replace('+', '-')
              .Replace('/', '_')
              .Replace("=", "");
        }




        [NonAction]
        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");

            }
            return byte2String;
        }


        [NonAction]
        public string CreateCode(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }
    }




}