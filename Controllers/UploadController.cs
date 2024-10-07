using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using VIR_WebApp.Data;
using VIR_WebApp.Models;
using MySql.Data.MySqlClient;

namespace VIR_WebApp.Controllers
{
    public class UploadController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ApplicationDbContext _context;

        public UploadController(IWebHostEnvironment webHostEnvironment, ApplicationDbContext context)
        {
            this._webHostEnvironment = webHostEnvironment;
            this._context = context;
        }

        [HttpGet]
        public IActionResult UploadLeftSide90()
        {
            ViewData["Position"] = "LEFT(90°)";
            ViewData["CaptureInfo"] = "Please Take a Picture of the Left Side (Non-Driver Side) of your vehicle.";
            ViewData["CarSampleImg"] = "left_90.png";
            return View();
        }

        [HttpGet]
        public IActionResult UploadRightSide90()
        {
            ViewData["Position"] = "RIGHT(90°)";
            ViewData["CaptureInfo"] = "Please Take a Picture of the Right Side (Driver Side) of your vehicle.";
            ViewData["CarSampleImg"] = "right_90.png";
            return View();
        }

        [HttpGet]
        public IActionResult UploadFront90()
        {
            ViewData["Position"] = "FRONT(90°)";
            ViewData["CaptureInfo"] = "Please Take a Picture of the Front Side of your vehicle.";
            ViewData["CarSampleImg"] = "front_90.png";
            return View();
        }

        [HttpGet]
        public IActionResult UploadRear90()
        {
            ViewData["Position"] = "REAR(90°)";
            ViewData["CaptureInfo"] = "Please Take a Picture of the Back Side of your vehicle.";
            ViewData["CarSampleImg"] = "rear_90.png";
            return View();
        }

        [HttpGet]
        public IActionResult UploadFrontLeft45()
        {
            ViewData["Position"] = "FRONT LEFT(45°)";
            ViewData["CaptureInfo"] = "Please Take a Picture of the Front Left corner (Non-Driver Side) of your vehicle.";
            ViewData["CarSampleImg"] = "front_left_45.png";
            return View();
        }

        [HttpGet]
        public IActionResult UploadFrontRight45()
        {
            ViewData["Position"] = "FRONT RIGHT(45°)";
            ViewData["CaptureInfo"] = "Please Take a Picture of the Front Right corner (Driver Side) of your vehicle.";
            ViewData["CarSampleImg"] = "front_right_45.png";
            return View();
        }

        [HttpGet]
        public IActionResult UploadRearLeft45()
        {
            ViewData["Position"] = "REAR LEFT(45°)";
            ViewData["CaptureInfo"] = "Please Take a Picture of the Back Left corner of your vehicle.";
            ViewData["CarSampleImg"] = "rear_left_45.png";
            return View();
        }

        [HttpGet]
        public IActionResult UploadRearRight45()
        {
            ViewData["Position"] = "REAR RIGHT(45°)";
            ViewData["CaptureInfo"] = "Please Take a Picture of the Back Right corner of your vehicle.";
            ViewData["CarSampleImg"] = "rear_right_45.png";
            return View();
        }

        [HttpGet]
        public IActionResult UploadOpenBonnet()
        {
            ViewData["Position"] = "OPEN BONNET";
            ViewData["CaptureInfo"] = "Please Take a Picture of the Bonnet of your vehicle. Ensure that the Bonnet is open";
            ViewData["CarSampleImg"] = "open_bonnet.png";
            return View();
        }

        [HttpGet]
        public IActionResult UploadOpenDickey()
        {
            ViewData["Position"] = "OPEN DICKEY";
            ViewData["CaptureInfo"] = "Please Take a Picture of the Dickey of your vehicle. Ensure that the Dickey is open";
            ViewData["CarSampleImg"] = "open_dickey.png";
            return View();
        }

        [HttpGet]
        public IActionResult UploadUnderBodyFromFrontSide()
        {
            ViewData["Position"] = "UNDER BODY FROM FRONT SIDE";
            ViewData["CaptureInfo"] = "Please Take a Picture of the under body of the vehicle from the front side";
            ViewData["CarSampleImg"] = "under_body_from_front_side.png";
            return View();
        }

        [HttpGet]
        public IActionResult UploadChassisNumberEmbossed()
        {
            ViewData["Position"] = "CHASSIS NUMBER (EMBOSSED)";
            ViewData["CaptureInfo"] = "Please Take a Picture of the chassis number such that the number is clearly visible";
            ViewData["CarSampleImg"] = "chassis_no.png";
            return View();
        }

        [HttpGet]
        public IActionResult UploadRegCertiCopyFront()
        {
            ViewData["Position"] = "REGISTRATION CERTIFICATE COPY FRONT";
            ViewData["CaptureInfo"] = "Please Take a Picture of the front side of your Registration Certificate";
            ViewData["CarSampleImg"] = "reg_cert_copy_front.png";
            return View();
        }

        [HttpGet]
        public IActionResult UploadRegCertiCopyBack()
        {
            ViewData["Position"] = "REGISTRATION CERTIFICATE COPY BACK";
            ViewData["CaptureInfo"] = "Please Take a Picture of the back side of your Registration Certificate";
            ViewData["CarSampleImg"] = "reg_cert_copy_back.png";
            return View();
        }

        [HttpGet]
        public IActionResult UploadOdoMeterReading()
        {
            ViewData["Position"] = "ODO METER READING";
            ViewData["CaptureInfo"] = "Please Take a Picture ODO Meter of your vehicle";
            ViewData["CarSampleImg"] = "odo_meter_reading.png";
            return View();
        }

        /// <summary>
        /// To Upload the user capture documents to server
        /// </summary>
        /// <param name="file"></param>
        /// <param name="imageDetails"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult UploadVehicleDocuments(IFormFile file, string imageDetails)
        {
            string? vehicleRegNo = HttpContext.Session.GetString("VehRegNo");
            long? custId = long.Parse(HttpContext.Session.GetString("CustomerId"));
            long? vehId = long.Parse(HttpContext.Session.GetString("VehicleId"));
            string? chassisNo = HttpContext.Session.GetString("ChassisNo");

            if (file != null && file.Length > 0)
            {
                try
                {
                    //string wwwRootPath = _webHostEnvironment.WebRootPath;
                    string drivePath = @"C:\VIRDocuments";

                    string imagePos = CleanString(imageDetails);

                    var uniqueIdentifier = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

                    var fileName = imagePos + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + "_" + uniqueIdentifier;

                    var folderPath = Path.Combine(drivePath, "Endorsement", vehicleRegNo);

                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    var imgfilePath = Path.Combine(folderPath, fileName);

                    using (var stream = new FileStream(imgfilePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    string filePath = "https://crm.autosherpas.com/EndorsementUploads/Endorsement/" + vehicleRegNo + "/" + fileName;

                    List<VIRDocumentsUploads> vIRDocumentsUploads = _context.VirDocumentsUploads.Where(x => x.VehicleRegNo == vehicleRegNo && x.VehicleId == vehId && x.CustomerId == custId && x.ProcessName == "Endorsement").ToList();
                    //updating
                    if (vIRDocumentsUploads.Count > 0)
                    {
                        VIRDocumentsUploads? uploads = _context.VirDocumentsUploads.Where(x => x.VehicleRegNo == vehicleRegNo && x.VehicleId == vehId && x.CustomerId == custId && x.ProcessName == "Endorsement").OrderByDescending(x => x.Id).FirstOrDefault();

                        if (uploads != null)
                        {
                            if (imagePos == "LEFT90")
                            {
                                if (!string.IsNullOrEmpty(uploads.ImgUrl_Left90))
                                {
                                    RemoveExistingImage(uploads.ImgUrl_Left90);
                                }
                                uploads.ImgUrl_Left90 = filePath;
                                uploads.UploadDateTime_Left90 = DateTime.Now.ToString("dd/MM/yy hh:mm:ss tt");
                            }
                            else if (imagePos == "RIGHT90")
                            {
                                if (!string.IsNullOrEmpty(uploads.ImgUrl_Right90))
                                {
                                    RemoveExistingImage(uploads.ImgUrl_Right90);
                                }
                                uploads.ImgUrl_Right90 = filePath;
                                uploads.UploadDateTime_Right90 = DateTime.Now.ToString("dd/MM/yy hh:mm:ss tt");
                            }
                            else if (imagePos == "FRONT90")
                            {
                                if (!string.IsNullOrEmpty(uploads.ImgUrl_Front90))
                                {
                                    RemoveExistingImage(uploads.ImgUrl_Front90);
                                }
                                uploads.ImgUrl_Front90 = filePath;
                                uploads.UploadDateTime_Front90 = DateTime.Now.ToString("dd/MM/yy hh:mm:ss tt");
                            }
                            else if (imagePos == "REAR90")
                            {
                                if (!string.IsNullOrEmpty(uploads.ImgUrl_Rear90))
                                {
                                    RemoveExistingImage(uploads.ImgUrl_Rear90);
                                }
                                uploads.ImgUrl_Rear90 = filePath;
                                uploads.UploadDateTime_Rear90 = DateTime.Now.ToString("dd/MM/yy hh:mm:ss tt");
                            }
                            else if (imagePos == "FRONT_LEFT45")
                            {
                                if (!string.IsNullOrEmpty(uploads.ImgUrl_FrontLeft45))
                                {
                                    RemoveExistingImage(uploads.ImgUrl_FrontLeft45);
                                }
                                uploads.ImgUrl_FrontLeft45 = filePath;
                                uploads.UploadDateTime_FrontLeft45 = DateTime.Now.ToString("dd/MM/yy hh:mm:ss tt");
                            }
                            else if (imagePos == "FRONT_RIGHT45")
                            {
                                if (!string.IsNullOrEmpty(uploads.ImgUrl_FrontRight45))
                                {
                                    RemoveExistingImage(uploads.ImgUrl_FrontRight45);
                                }
                                uploads.ImgUrl_FrontRight45 = filePath;
                                uploads.UploadDateTime_FrontRight45 = DateTime.Now.ToString("dd/MM/yy hh:mm:ss tt");
                            }
                            else if (imagePos == "REAR_LEFT45")
                            {
                                if (!string.IsNullOrEmpty(uploads.ImgUrl_RearLeft45))
                                {
                                    RemoveExistingImage(uploads.ImgUrl_RearLeft45);
                                }
                                uploads.ImgUrl_RearLeft45 = filePath;
                                uploads.UploadDateTime_RearLeft45 = DateTime.Now.ToString("dd/MM/yy hh:mm:ss tt");
                            }
                            else if (imagePos == "REAR_RIGHT45")
                            {
                                if (!string.IsNullOrEmpty(uploads.ImgUrl_RearRight45))
                                {
                                    RemoveExistingImage(uploads.ImgUrl_RearRight45);
                                }
                                uploads.ImgUrl_RearRight45 = filePath;
                                uploads.UploadDateTime_RearRight45 = DateTime.Now.ToString("dd/MM/yy hh:mm:ss tt");
                            }
                            else if (imagePos == "OPEN_BONNET")
                            {
                                if (!string.IsNullOrEmpty(uploads.ImgUrl_OpenBonnet))
                                {
                                    RemoveExistingImage(uploads.ImgUrl_OpenBonnet);
                                }
                                uploads.ImgUrl_OpenBonnet = filePath;
                                uploads.UploadDateTime_OpenBonnet = DateTime.Now.ToString("dd/MM/yy hh:mm:ss tt");
                            }
                            else if (imagePos == "OPEN_DICKEY")
                            {
                                if (!string.IsNullOrEmpty(uploads.ImgUrl_OpenDickey))
                                {
                                    RemoveExistingImage(uploads.ImgUrl_OpenDickey);
                                }
                                uploads.ImgUrl_OpenDickey = filePath;
                                uploads.UploadDateTime_OpenDickey = DateTime.Now.ToString("dd/MM/yy hh:mm:ss tt");
                            }
                            else if (imagePos == "UNDER_BODY_FROM_FRONT_SIDE")
                            {
                                if (!string.IsNullOrEmpty(uploads.ImgUrl_UnderBodyFromFrontSide))
                                {
                                    RemoveExistingImage(uploads.ImgUrl_UnderBodyFromFrontSide);
                                }
                                uploads.ImgUrl_UnderBodyFromFrontSide = filePath;
                                uploads.UploadDateTime_UnderBodyFromFrontSide = DateTime.Now.ToString("dd/MM/yy hh:mm:ss tt");
                            }
                            else if (imagePos == "CHASSIS_NUMBER_EMBOSSED")
                            {
                                if (!string.IsNullOrEmpty(uploads.ImgUrl_ChassisNo))
                                {
                                    RemoveExistingImage(uploads.ImgUrl_ChassisNo);
                                }
                                uploads.ImgUrl_ChassisNo = filePath;
                                uploads.UploadDateTime_ChassisNo = DateTime.Now.ToString("dd/MM/yy hh:mm:ss tt");
                            }
                            else if (imagePos == "REGISTRATION_CERTIFICATE_COPY_FRONT")
                            {
                                if (!string.IsNullOrEmpty(uploads.ImgUrl_RCCopyFront))
                                {
                                    RemoveExistingImage(uploads.ImgUrl_RCCopyFront);
                                }
                                uploads.ImgUrl_RCCopyFront = filePath;
                                uploads.UploadDateTime_RCCopyFront = DateTime.Now.ToString("dd/MM/yy hh:mm:ss tt");
                            }
                            else if (imagePos == "REGISTRATION_CERTIFICATE_COPY_BACK")
                            {
                                if (!string.IsNullOrEmpty(uploads.ImgUrl_RCCopyBack))
                                {
                                    RemoveExistingImage(uploads.ImgUrl_RCCopyBack);
                                }
                                uploads.ImgUrl_RCCopyBack = filePath;
                                uploads.UploadDateTime_RCCopyBack = DateTime.Now.ToString("dd/MM/yy hh:mm:ss tt");
                            }
                            else if (imagePos == "ODO_METER_READING")
                            {
                                if (!string.IsNullOrEmpty(uploads.ImgUrl_OdoMeterReading))
                                {
                                    RemoveExistingImage(uploads.ImgUrl_OdoMeterReading);
                                }
                                uploads.ImgUrl_OdoMeterReading = filePath;
                                uploads.UploadDateTime_OdoMeterReading = DateTime.Now.ToString("dd/MM/yy hh:mm:ss tt");
                            }

                            uploads.DocsUploadedLink = Url.Action("", "", null, Request.Scheme);
                            uploads.LastUploadDateTime = DateTime.Now;

                            _context.VirDocumentsUploads.Update(uploads);
                            _context.SaveChanges();
                        }
                    }
                    //adding
                    else
                    {
                        VIRDocumentsUploads uploads = new VIRDocumentsUploads();

                        uploads.VehicleId = vehId;
                        uploads.CustomerId = custId;
                        uploads.VehicleRegNo = vehicleRegNo;
                        uploads.ChassisNo = chassisNo;
                        uploads.LastUploadDateTime = DateTime.Now;
                        uploads.DocsUploadedLink = Url.Action("", "", null, Request.Scheme);

                        if (imagePos == "LEFT90")
                        {
                            uploads.ImgUrl_Left90 = filePath;
                            uploads.UploadDateTime_Left90 = DateTime.Now.ToString("dd/MM/yy hh:mm:ss tt");
                        }
                        else if (imagePos == "RIGHT90")
                        {
                            uploads.ImgUrl_Right90 = filePath;
                            uploads.UploadDateTime_Right90 = DateTime.Now.ToString("dd/MM/yy hh:mm:ss tt");
                        }
                        else if (imagePos == "FRONT90")
                        {
                            uploads.ImgUrl_Front90 = filePath;
                            uploads.UploadDateTime_Front90 = DateTime.Now.ToString("dd/MM/yy hh:mm:ss tt");
                        }
                        else if (imagePos == "REAR90")
                        {
                            uploads.ImgUrl_Rear90 = filePath;
                            uploads.UploadDateTime_Rear90 = DateTime.Now.ToString("dd/MM/yy hh:mm:ss tt");
                        }
                        else if (imagePos == "FRONT_LEFT45")
                        {
                            uploads.ImgUrl_FrontLeft45 = filePath;
                            uploads.UploadDateTime_FrontLeft45 = DateTime.Now.ToString("dd/MM/yy hh:mm:ss tt");
                        }
                        else if (imagePos == "FRONT_RIGHT45")
                        {
                            uploads.ImgUrl_FrontRight45 = filePath;
                            uploads.UploadDateTime_FrontRight45 = DateTime.Now.ToString("dd/MM/yy hh:mm:ss tt");
                        }
                        else if (imagePos == "REAR_LEFT45")
                        {
                            uploads.ImgUrl_RearLeft45 = filePath;
                            uploads.UploadDateTime_RearLeft45 = DateTime.Now.ToString("dd/MM/yy hh:mm:ss tt");
                        }
                        else if (imagePos == "REAR_RIGHT45")
                        {
                            uploads.ImgUrl_RearRight45 = filePath;
                            uploads.UploadDateTime_RearRight45 = DateTime.Now.ToString("dd/MM/yy hh:mm:ss tt");
                        }
                        else if (imagePos == "OPEN_BONNET")
                        {
                            uploads.ImgUrl_OpenBonnet = filePath;
                            uploads.UploadDateTime_OpenBonnet = DateTime.Now.ToString("dd/MM/yy hh:mm:ss tt");
                        }
                        else if (imagePos == "OPEN_DICKEY")
                        {
                            uploads.ImgUrl_OpenDickey = filePath;
                            uploads.UploadDateTime_OpenDickey = DateTime.Now.ToString("dd/MM/yy hh:mm:ss tt");
                        }
                        else if (imagePos == "UNDER_BODY_FROM_FRONT_SIDE")
                        {
                            uploads.ImgUrl_UnderBodyFromFrontSide = filePath;
                            uploads.UploadDateTime_UnderBodyFromFrontSide = DateTime.Now.ToString("dd/MM/yy hh:mm:ss tt");
                        }
                        else if (imagePos == "CHASSIS_NUMBER_EMBOSSED")
                        {
                            uploads.ImgUrl_ChassisNo = filePath;
                            uploads.UploadDateTime_ChassisNo = DateTime.Now.ToString("dd/MM/yy hh:mm:ss tt");
                        }
                        else if (imagePos == "REGISTRATION_CERTIFICATE_COPY_FRONT")
                        {
                            uploads.ImgUrl_RCCopyFront = filePath;
                            uploads.UploadDateTime_RCCopyFront = DateTime.Now.ToString("dd/MM/yy hh:mm:ss tt");
                        }
                        else if (imagePos == "REGISTRATION_CERTIFICATE_COPY_BACK")
                        {
                            uploads.ImgUrl_RCCopyBack = filePath;
                            uploads.UploadDateTime_RCCopyBack = DateTime.Now.ToString("dd/MM/yy hh:mm:ss tt");
                        }
                        else if (imagePos == "ODO_METER_READING")
                        {
                            uploads.ImgUrl_RCCopyBack = filePath;
                            uploads.UploadDateTime_OdoMeterReading = DateTime.Now.ToString("dd/MM/yy hh:mm:ss tt");
                        }

                        _context.VirDocumentsUploads.Add(uploads);
                        _context.SaveChanges();
                    }

                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
                    string exception = string.Empty;
                    if (ex.InnerException != null)
                    {
                        if (ex.InnerException.InnerException != null)
                        {
                            exception = ex.InnerException.InnerException.Message;
                        }
                        else
                        {
                            exception = ex.InnerException.Message;
                        }
                    }
                    else
                    {
                        exception = ex.Message;
                    }
                    return Json(new { success = false, message = $"Error uploading {exception}" });
                }
            }
            else
            {
                return Json(new { success = false, message = "No image found to upload, please capture before uploading" });
            }
        }

        [HttpGet]
        public IActionResult UploadedDocsPreview()
        {
            HttpContext.Session.SetString("HasVisitedDocsPreview", "true");

            string? vehicleRegNo = HttpContext.Session.GetString("VehRegNo");
            long? custId = long.Parse(HttpContext.Session.GetString("CustomerId"));
            long? vehId = long.Parse(HttpContext.Session.GetString("VehicleId"));
            VIRDocumentsUploads? virDocUploads = null;
            try
            {
                if (_context.VirDocumentsUploads.Where(x => x.VehicleRegNo == vehicleRegNo && x.CustomerId == custId && x.VehicleId == vehId && x.ProcessName == "Endorsement").OrderByDescending(x => x.Id).FirstOrDefault() != null)
                {
                    virDocUploads = _context.VirDocumentsUploads.Where(x => x.VehicleRegNo == vehicleRegNo && x.CustomerId == custId && x.VehicleId == vehId && x.ProcessName == "Endorsement").OrderByDescending(x => x.Id).FirstOrDefault();

                    virDocUploads.ImgUrl_Left90 = GetFileName(virDocUploads.ImgUrl_Left90);
                    virDocUploads.ImgUrl_Right90 = GetFileName(virDocUploads.ImgUrl_Right90);
                    virDocUploads.ImgUrl_Front90 = GetFileName(virDocUploads.ImgUrl_Front90);
                    virDocUploads.ImgUrl_Rear90 = GetFileName(virDocUploads.ImgUrl_Rear90);
                    virDocUploads.ImgUrl_FrontLeft45 = GetFileName(virDocUploads.ImgUrl_FrontLeft45);
                    virDocUploads.ImgUrl_FrontRight45 = GetFileName(virDocUploads.ImgUrl_FrontRight45);
                    virDocUploads.ImgUrl_RearLeft45 = GetFileName(virDocUploads.ImgUrl_RearLeft45);
                    virDocUploads.ImgUrl_RearRight45 = GetFileName(virDocUploads.ImgUrl_RearRight45);
                    virDocUploads.ImgUrl_OpenBonnet = GetFileName(virDocUploads.ImgUrl_OpenBonnet);
                    virDocUploads.ImgUrl_OpenDickey = GetFileName(virDocUploads.ImgUrl_OpenDickey);
                    virDocUploads.ImgUrl_UnderBodyFromFrontSide = GetFileName(virDocUploads.ImgUrl_UnderBodyFromFrontSide);
                    virDocUploads.ImgUrl_ChassisNo = GetFileName(virDocUploads.ImgUrl_ChassisNo);
                    virDocUploads.ImgUrl_RCCopyFront = GetFileName(virDocUploads.ImgUrl_RCCopyFront);
                    virDocUploads.ImgUrl_RCCopyBack = GetFileName(virDocUploads.ImgUrl_RCCopyBack);
                    virDocUploads.ImgUrl_OdoMeterReading = GetFileName(virDocUploads.ImgUrl_OdoMeterReading);
                }
            }
            catch (Exception ex)
            {

            }

            return View(virDocUploads);
        }

        public IActionResult UploadSuccess(string latitude, string longitude)
        {
            string? vehicleRegNo = HttpContext.Session.GetString("VehRegNo");
            long? custId = long.Parse(HttpContext.Session.GetString("CustomerId"));
            long? vehId = long.Parse(HttpContext.Session.GetString("VehicleId"));
            try
            {
                var virDocUploads = _context.VirDocumentsUploads.Where(x => x.VehicleRegNo == vehicleRegNo && x.CustomerId == custId && x.VehicleId == vehId && x.ProcessName == "Endorsement").OrderByDescending(x => x.Id).FirstOrDefault();
                if (virDocUploads != null)
                {
                    virDocUploads.Latitude = latitude;
                    virDocUploads.Longitude = longitude;
                    _context.VirDocumentsUploads.Update(virDocUploads);
                    _context.SaveChanges();

                    int rowsInserted = _context.Database.ExecuteSqlRaw("CALL LivedateInsertion(@inid)", new MySqlParameter("@inid", virDocUploads.Id));

                    virDocUploads.IsSubmitted = true;
                    _context.VirDocumentsUploads.Update(virDocUploads);
                    _context.SaveChanges();

                    return Json(new { success = true });
                }
                return Json(new { success = false, message = "Data not submitted.. no data found" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"{ex.Message}" });
            }
        }

        /// <summary>
        /// removing image if its already exist in server before uploading new one
        /// </summary>
        /// <param name="imageUrl"></param>
        public void RemoveExistingImage(string imageUrl)
        {
            try
            {
                if (!string.IsNullOrEmpty(imageUrl))
                {
                    Uri uri = new Uri(imageUrl);
                    string relativePath = uri.LocalPath.Replace("/EndorsementUploads", "").Replace('/', '\\');

                    string imgDirectory = @"C:\VIRDocuments";
                    string filePath = System.IO.Path.Combine(imgDirectory, relativePath.TrimStart('\\'));

                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
        }

        /// <summary>
        /// to remove special characters except numbers and letters, and replace space with underscore
        /// </summary>
        /// <param name="input"></param>
        /// <returns>string</returns>
        public string CleanString(string input)
        {
            string cleaned = Regex.Replace(input, @"[^a-zA-Z0-9\s]", "");

            cleaned = cleaned.Replace(" ", "_");

            return cleaned;
        }

        /// <summary>
        /// extract file name from image url
        /// </summary>
        /// <param name="imageUrl"></param>
        /// <returns>string:fileName</returns>
        public string GetFileName(string imageUrl)
        {
            if (!string.IsNullOrEmpty(imageUrl))
            {
                //int index = imageUrl.LastIndexOf("\\");
                //string fileName = imageUrl.Substring(index + 1);
                //return fileName;

                Uri uri = new Uri(imageUrl);
                string fileName = System.IO.Path.GetFileName(uri.LocalPath);
                return fileName;
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
