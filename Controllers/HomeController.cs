using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Security.Claims;
using System.Web;
using VIR_WebApp.Data;
using VIR_WebApp.Models;
using static Microsoft.AspNetCore.Razor.Language.TagHelperMetadata;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using VIR_WebApp.Models.ViewModels;

namespace VIR_WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ValidateVehicleRegNum(string vehicleRegNumber)
        {
            //string vehRegNum = vehicleRegNumber.Trim();
            bool findData = SaveCustomerDetails(vehicleRegNumber);
            try
            {
                if(findData)
                {
                    if (_context.CustomerInfos.Where(x => x.vehicleRegNo == vehicleRegNumber).OrderByDescending(s => s.Id).FirstOrDefault() != null)
                    {
                        CustomerInfo? customerInfo = _context.CustomerInfos.Where(x => x.vehicleRegNo == vehicleRegNumber).OrderByDescending(s => s.Id).FirstOrDefault();
                        if (customerInfo != null)
                        {
                            if (!string.IsNullOrEmpty(customerInfo.vehicle_id.ToString()))
                            {
                                HttpContext.Session.SetString("VehicleId", customerInfo.vehicle_id.ToString());
                            }
                            if (!string.IsNullOrEmpty(customerInfo.customer_id.ToString()))
                            {
                                HttpContext.Session.SetString("CustomerId", customerInfo.customer_id.ToString());
                            }
                            if (!string.IsNullOrEmpty(customerInfo.chassisNo))
                            {
                                HttpContext.Session.SetString("ChassisNo", customerInfo.chassisNo);
                            }
                        }
                        HttpContext.Session.SetString("VehRegNo", vehicleRegNumber.ToUpper());
                        var claims = new List<Claim>
                        {
                           new Claim(ClaimTypes.Name,vehicleRegNumber)
                        };
                        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var principal = new ClaimsPrincipal(identity);
                        HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                        return Json(new { success = true });
                    }
                    else
                    {
                        return Json(new { success = false });
                    }
                }
                else
                {
                    return Json(new { success = false, message = "Vehicle data not found" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false , message = $"Error: {ex.Message}" });
            }
        }

        [HttpGet]
        [Authorize]
        public IActionResult PhoneNumbers()
        {
            string? vehRegNo = HttpContext.Session.GetString("VehRegNo");
            long? custId = long.Parse(HttpContext.Session.GetString("CustomerId"));
            long? vehId = long.Parse(HttpContext.Session.GetString("VehicleId"));
            List<PhoneNumberVM> phoneNumbers = new List<PhoneNumberVM>();

            try
            {
                string? phnNum = _context.CustomerInfos.Where(x => x.vehicleRegNo == vehRegNo && x.customer_id == custId && x.vehicle_id == vehId).OrderByDescending(s => s.Id).Select(x => x.phoneNumber).FirstOrDefault();
                if (phnNum != null)
                {
                    var originalNumbers = phnNum.Split(",").ToList();
                    phoneNumbers = originalNumbers.Select(number => new PhoneNumberVM
                    {
                        Original = number,
                        Masked = MaskPhoneNumber(number)
                    }).ToList();
                }
            }
            catch (Exception ex)
            { 

            }
            ViewBag.PhoneNumbers = phoneNumbers;
            return View();
        }

        [HttpPost]
        public IActionResult SendOTP(string phoneNumber)
        {
            string? vehRegNo = HttpContext.Session.GetString("VehRegNo");

            try
            {
                long? custId = long.Parse(HttpContext.Session.GetString("CustomerId"));
                long? vehId = long.Parse(HttpContext.Session.GetString("VehicleId"));

                var otp = GenerateOTP();

                string responseMessage = SendOtpSms(phoneNumber, otp);

                if (responseMessage == "Success")
                {
                    //OTP saving part
                    GeneratedOTPs otpDetails = new GeneratedOTPs();
                    if (_context.GeneratedOTPs.Count(m => m.CustomerId == custId) > 0)
                    {
                        var removeOtpDetails = _context.GeneratedOTPs.FirstOrDefault(m => m.CustomerId == custId);
                        _context.GeneratedOTPs.Remove(removeOtpDetails);
                        _context.SaveChanges();
                    }

                    otpDetails.CustomerId = custId;
                    otpDetails.OTP = otp;
                    otpDetails.TimeStamp = DateTime.Now.AddMinutes(3);
                    _context.GeneratedOTPs.Add(otpDetails);
                    _context.SaveChanges();

                    if (!string.IsNullOrEmpty(phoneNumber))
                    {
                        HttpContext.Session.SetString("PhoneNumber", phoneNumber);
                    }

                    TempData["Success"] = "OTP sent to your mobile number successfully";
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false, message = $"Error while sending OTP. {responseMessage}" });
                }
            }
            catch (Exception ex)
            {
                HttpContext.Session.Remove("OTP");
                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("PhoneNumber")))
                {
                    HttpContext.Session.Remove("PhoneNumber");
                }

                TempData["Error"] = $"Error sending OTP {ex.Message}";
                return Json(new { success = false });
            }
        }

        [HttpGet]
        public IActionResult EnterOTP()
        {
            string? phoneNumber = HttpContext.Session.GetString("PhoneNumber");

            if (string.IsNullOrEmpty(phoneNumber))
            {
                return RedirectToAction("PhoneNumbers", "Home");
            }
            ViewData["PhoneNumber"] = MaskPhoneNumber(phoneNumber);
            return View();
        }

        [HttpPost]
        public IActionResult ValidateOTP(string otp)
        {
            long? custId = long.Parse(HttpContext.Session.GetString("CustomerId"));
            try
            {
                long? genereateOTP = _context.GeneratedOTPs.Where(x => x.CustomerId == custId).Select(x => x.OTP).FirstOrDefault();
                if (genereateOTP != null)
                {
                    if (!string.IsNullOrEmpty(otp))
                    {
                        if (genereateOTP.ToString() == otp)
                        {
                            return Json(new { success = true });
                        }
                        else
                        {
                            return Json(new { success = false, message = "Entered OTP is not valid.." });
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return View();
        }

        [HttpGet]
        [Authorize]
        public IActionResult VehicleInspection()
        {
            //string? vehRegNo = HttpContext.Session.GetString("VehRegNo");
            //long? custId = long.Parse(HttpContext.Session.GetString("CustomerId"));
            //long? vehId = long.Parse(HttpContext.Session.GetString("VehicleId"));
            //CustomerInfo? customerInfo = new CustomerInfo();
            //try
            //{
            //    customerInfo = _context.CustomerInfos.Where(x => x.customer_id == custId && x.vehicle_id == vehId && x.vehicleRegNo == vehRegNo).OrderByDescending(x => x.Id).FirstOrDefault();
            //    if(customerInfo != null)
            //    {
            //        if (!string.IsNullOrEmpty(customerInfo.chassisNo))
            //        {
            //            int noOfChars = 6;
            //            string lastSixChars = customerInfo.chassisNo.Length >= noOfChars ? customerInfo.chassisNo.Substring(customerInfo.chassisNo.Length - noOfChars) : string.Empty;
            //            customerInfo.chassisNo = lastSixChars;
            //            ViewData["ChassisNumber"] = customerInfo.chassisNo;
            //        }
            //        else
            //        {
            //            ViewData["ChassisNumber"] = string.Empty;
            //        }

            //        if (!string.IsNullOrEmpty(customerInfo.engineNo))
            //        {
            //            int noOfChars = 6;
            //            string lastSixChars = customerInfo.engineNo.Length >= noOfChars ? customerInfo.engineNo.Substring(customerInfo.engineNo.Length - noOfChars) : string.Empty;
            //            customerInfo.engineNo = lastSixChars;
            //            ViewData["EngineNumber"] = customerInfo.engineNo;
            //        }
            //        else
            //        {
            //            ViewData["EngineNumber"] = string.Empty;
            //        }

            //    }

            //}
            //catch(Exception ex)
            //{

            //}
            return View();
        }

        [HttpPost]
        public IActionResult VehicleInspection(string odoMetRead)
        {
            string? vehRegNo = HttpContext.Session.GetString("VehRegNo");
            long? custId = long.Parse(HttpContext.Session.GetString("CustomerId"));
            long? vehId = long.Parse(HttpContext.Session.GetString("VehicleId"));
            string? chassisNumber = HttpContext.Session.GetString("ChassisNo"); 
            string? phoneNum = HttpContext.Session.GetString("PhoneNumber");

            try
            {
                List<VIRDocumentsUploads> virUploads = _context.VirDocumentsUploads.Where(x => x.CustomerId == custId && x.VehicleId == vehId && x.VehicleRegNo == vehRegNo && x.ProcessName == "Endorsement").ToList();
                if (virUploads.Count > 0)
                {
                    VIRDocumentsUploads? existUpload = _context.VirDocumentsUploads.Where(x => x.CustomerId == custId && x.VehicleId == vehId && x.VehicleRegNo == vehRegNo && x.ProcessName == "Endorsement").OrderByDescending(x => x.Id).FirstOrDefault();
                    if (existUpload != null)
                    {
                        existUpload.PhoneNumber = phoneNum;
                        existUpload.OdoMeterReading = long.Parse(odoMetRead);
                        //existUpload.ChassisNoLast6Digits = chassisNo;
                        //existUpload.EngineNoLast6Digits = engineNo;
                        existUpload.LastUploadDateTime = DateTime.Now;

                        _context.VirDocumentsUploads.Update(existUpload);
                        _context.SaveChanges();
                    }
                }
                else
                {
                    VIRDocumentsUploads? virToBeUpload = new VIRDocumentsUploads();
                    virToBeUpload.CustomerId = custId;
                    virToBeUpload.VehicleId = vehId;
                    virToBeUpload.VehicleRegNo = vehRegNo;
                    virToBeUpload.ChassisNo = chassisNumber;
                    virToBeUpload.PhoneNumber = phoneNum;
                    virToBeUpload.ProcessName = "Endorsement";
                    virToBeUpload.OdoMeterReading = long.Parse(odoMetRead);
                    //virToBeUpload.ChassisNoLast6Digits = chassisNo;
                    //virToBeUpload.EngineNoLast6Digits = engineNo;
                    virToBeUpload.LastUploadDateTime = DateTime.Now;

                    _context.VirDocumentsUploads.Add(virToBeUpload);
                    _context.SaveChanges();
                }
                return Json(new { success = true });
            }
            catch(Exception ex)
            {
                return Json(new { success = false, message=$"Error while updating Vehicle informations {ex.Message}" });
            }

            
        }

        /// <summary>
        /// finding vehicle reg number and saving its data to server for logging purpose
        /// </summary>
        /// <param name="vehicleRegNo"></param>
        /// <returns>returns true if data is present or else false</returns>
        private bool SaveCustomerDetails(string vehicleRegNo)
        {
            List<CustDataVM> custDataVM;
            try
            {
                custDataVM = _context.Database.SqlQueryRaw<CustDataVM>("CALL vehregSearch(@vehicleRegNo)", new MySqlParameter("@vehicleRegNo", vehicleRegNo)).ToList();

                if (custDataVM != null && custDataVM.Count != 0)
                {
                    foreach (var item in custDataVM)
                    {
                        if (_context.CustomerInfos.Where(x => x.vehicle_id == item.vehicle_id && x.customer_id == item.customer_id).Count() > 0)
                        {
                            List<CustomerInfo> oldCustomerInfo = _context.CustomerInfos.Where(x => x.vehicle_id == item.vehicle_id && x.customer_id == item.customer_id).ToList();
                            _context.CustomerInfos.RemoveRange(oldCustomerInfo);
                            _context.SaveChanges();
                        }
                    }
                }

                if (custDataVM != null && custDataVM.Count != 0)
                {
                    foreach (var custInfo in custDataVM)
                    {
                        CustomerInfo customerInfo = new CustomerInfo()
                        {
                            customer_id = custInfo.customer_id,
                            vehicle_id = custInfo.vehicle_id,
                            vehicleRegNo = vehicleRegNo.ToUpper(),
                            chassisNo = custInfo.chassisNo,
                            customerName = custInfo.customerName,
                            engineNo = custInfo.engineNo,
                            phoneNumber = custInfo.phoneNumber
                        };
                        _context.CustomerInfos.Add(customerInfo);
                        _context.SaveChanges();
                    }
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// To Generate 4 digit OTP
        /// </summary>
        /// <returns>6 digits</returns>
        private int GenerateOTP()
        {
            var random = new Random();
            return random.Next(1000, 9999);
        }

        /// <summary>
        /// API to send otp message to preffered phone number
        /// </summary>
        /// <param name="toPhoneNumber"></param>
        /// <param name="message"></param>
        private string SendOtpSms(string toPhoneNumber, long otp)
        {
            long? custId = long.Parse(HttpContext.Session.GetString("CustomerId"));
            string? vehRegNo = HttpContext.Session.GetString("VehRegNo").ToUpper();
            SMSTemplates? smsTemplate = new SMSTemplates();
            string? apiUrl = string.Empty;
            string? smsBody = string.Empty;
            string? message = string.Empty;
            string? apiResponseContent = string.Empty;

            try
            {
                smsTemplate = _context.SMSTemplates.FirstOrDefault(x => x.ServiceType == "Service Implicit" && x.IsActive == true);

                if (smsTemplate != null)
                {
                    if (!string.IsNullOrEmpty(smsTemplate.SmsTemplate))
                    {
                        smsBody = smsTemplate.SmsTemplate.Replace("{{OTP}}", otp.ToString()).Replace("{{Reg No}}", vehRegNo);
                    }

                    apiUrl = smsTemplate.SmsApi + "sender=" + smsTemplate.DealerName + "&" + "number=" + toPhoneNumber.Trim() + "&" + "sms=" + smsBody;

                    using (var httpClient = new HttpClient())
                    {

                        httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                        HttpResponseMessage response = httpClient.GetAsync(apiUrl).Result;

                        if (response.IsSuccessStatusCode)
                        {
                            message = "Success";
                            string content = response.Content.ReadAsStringAsync().Result;
                            apiResponseContent = content;
                        }
                        else
                        {
                            message = "Error";
                            string content = response.Content.ReadAsStringAsync().Result;
                            apiResponseContent = content;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    if (ex.InnerException.InnerException != null)
                    {
                        message = ex.InnerException.InnerException.Message;
                    }
                    else
                    {
                        message = ex.InnerException.Message;
                    }
                }
                else
                {
                    message = ex.Message;
                }

                return message;
            }
            finally
            {
                SMSInteraction smsInteraction = new SMSInteraction();
                smsInteraction.CustomerId = custId;
                smsInteraction.MobileNumber = long.Parse(toPhoneNumber);
                smsInteraction.Message = smsBody;
                smsInteraction.MsgType = "Text Msg";
                smsInteraction.SmsType = "OTP";
                smsInteraction.MsgStatus = message;
                smsInteraction.ResponseFromGateway = apiResponseContent;
                smsInteraction.InteractionDateTime = DateTime.Now;

                _context.SMSInteractions.Add(smsInteraction);
                _context.SaveChanges();

            }
            return message;
        }

        /// <summary>
        /// phone numbers masking
        /// </summary>
        /// <param name="number"></param>
        /// <returns>eg.12XXXX7890</returns>
        private string MaskPhoneNumber(string number)
        {
            if (number.StartsWith("+91") && number.Length >= 13)
            {
                return number.Substring(0, 5) + "XXXX" + number.Substring(9);
            }
            else if (!number.StartsWith("+91") && number.Length >= 10)
            {
                return number.Substring(0, 2) + "XXXX" + number.Substring(6);
            }
            return number;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
