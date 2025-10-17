using PurchaseOrderManagementSystem.Helper;
using PurchaseOrderManagementSystemClassLibrary.DAL;
using PurchaseOrderManagementSystemClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace PurchaseOrderManagementSystem.Controllers
{
   [JwtAuthorize]
    public class PurchaseOrderController : BaseController
    {
        PurchaseOrders purchaseOrderDAL = new PurchaseOrders();
        Categories categoriesDAL =  new Categories();
        PurchaseOrderStatus purchaseOderStatusDAL = new PurchaseOrderStatus();
        VendorContacts vendorContactsDAL = new VendorContacts();
        Vendors vendorsDAL = new Vendors();
        Products productsDAL = new Products();
        ErrorLogs errorLogsDAL = new ErrorLogs();

        [HttpGet]
        public ActionResult PurchaseOrderList(PurchaseOrderModel model)
        {
            try
            {
                var sessionModel = Session["PurchaseOrderSearchParameters"] as PurchaseOrderModel;
                if (sessionModel != null)
                {
                    model = sessionModel;
                }
                model.ProductCategoryList = categoriesDAL.GetList();
                model.ProductList = productsDAL.GetList();
                model.PurchaseOrderStatusList = purchaseOderStatusDAL.GetList();
                errorLogsDAL.CurrentUserId = CurrentUserId;

                return View(model);
            }
            catch(Exception ex)
            {
                errorLogsDAL.InsertErrorLogs(ex.Message, ex.StackTrace);
                return View(model);
            }
        }

        [HttpPost]
        public ActionResult PurchaseOrderListPartial(PurchaseOrderModel model)
        {
            try
            {
                model.PurchaseOrders = purchaseOrderDAL.PurchaseOrderGetList(model);
                model.TotalPages = (int)Math.Ceiling((double)model.TotalRecords / model.PageSize);
                purchaseOrderDAL.CurrentUserId = CurrentUserId;
                Session["PurchaseOrderSearchParameters"] = model;
                errorLogsDAL.CurrentUserId = CurrentUserId;
                return PartialView("_PurchaseOrderListPartial", model);
            }

            catch(Exception ex)
            {
                errorLogsDAL.InsertErrorLogs(ex.Message, ex.StackTrace);
                return PartialView("_PurchaseOrderListPartial", model);
            }
        }


        public ActionResult GetPurchaseOrderDetails(int id)
        {          
            try
            {
                var model = purchaseOrderDAL.PurchaseOrderGetDetails(id);
                errorLogsDAL.CurrentUserId = CurrentUserId;

                return View("PurchaseOrderDetails", model);
            }
            catch(Exception ex)
            {
                errorLogsDAL.InsertErrorLogs(ex.Message, ex.StackTrace);
                return View("PurchaseOrderDetails", new PurchaseOrderDetailModel());
            }
         }

        public ActionResult AddEditPurchaseOrder(int? id = 0)
        {
            var model = new PurchaseOrderModel();
            try
            {

                if (id.HasValue && id.Value != 0)
                {
                    model = purchaseOrderDAL.GetPurchaseOrderById(id.Value);
                    if (model == null || model.PurchaseOrderId == 0)
                    {
                        ViewBag.ErrorMessage = "Purchase Order details not found.";
                        return View("AddEditPurchaseOrders",new PurchaseOrderModel());
                    }
                }

                model.PurchaseOrderStatusList = purchaseOderStatusDAL.GetList();
                model.ProductList = productsDAL.GetList();
                model.VendorsList = vendorsDAL.GetList();
                model.VendorContactsList = vendorContactsDAL.GetVendorContactsByVendorId(model.VendorId);
                errorLogsDAL.CurrentUserId = CurrentUserId;

                return View("AddEditPurchaseOrders", model);
            }
            catch (Exception ex)
            {
                errorLogsDAL.InsertErrorLogs(ex.Message, ex.StackTrace);
                ModelState.AddModelError("", "An error occurred while loading purchase order details.");
                return View("AddEditPurchaseOrders", model);
            }
        }

        [HttpGet]
        public JsonResult GetVendorContactsByVendorId(int vendorId)
        {
            try
            {
                var vendorContacts = vendorContactsDAL.GetVendorContactsByVendorId(vendorId);
                errorLogsDAL.CurrentUserId = CurrentUserId;

                return Json(vendorContacts, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                errorLogsDAL.InsertErrorLogs(ex.Message, ex.StackTrace);
                return Json(new { success = false, message = "Error: " + ex.Message });
            }

        }


        [HttpPost]
        public JsonResult SavePurchaseOrder(PurchaseOrderModel model)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    var message = ModelState.Where(x => x.Value.Errors.Count > 0).ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.First().ErrorMessage);
                    return Json(new { success = false, message });
                }

                List<string> savedFilePaths = new List<string>();
                if (model.DocumentsList == null)
                    model.DocumentsList = new List<PurchaseOrderDocumentModel>();

                if (Request.Files.Count > 0)
                {
                    string uploadFolder = ConfigurationManager.AppSettings["UploadFolder"];
                    string folderPath = Server.MapPath(uploadFolder);

                    if (!Directory.Exists(folderPath))
                        Directory.CreateDirectory(folderPath);

                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        var file = Request.Files[i];
                        if (file != null && file.ContentLength > 0) 
                        {
                            string originalFileName = Path.GetFileName(file.FileName);
                            string extension = Path.GetExtension(file.FileName);
                            string uniqueFileName = $"PO_{Guid.NewGuid()}{extension}";
                            string fullPath = Path.Combine(folderPath, uniqueFileName);
                            string noteKey = $"DocumentNotes[{i}]";
                            string note = Request.Form[noteKey];

                            file.SaveAs(fullPath);
                            savedFilePaths.Add(fullPath);

                            model.DocumentsList.Add(new PurchaseOrderDocumentModel
                            {
                                DocumentName = originalFileName,
                                DocumentFileName = uniqueFileName,
                                Notes = note
                            });
                        }
                    }
                }
                purchaseOrderDAL.CurrentUserId = CurrentUserId;
                errorLogsDAL.CurrentUserId = CurrentUserId;

                int result = purchaseOrderDAL.SavePurchaseOrder(model);

                if (result <= 0)
                {
                    foreach (var path in savedFilePaths)
                    {
                        if (System.IO.File.Exists(path))
                            System.IO.File.Delete(path);
                    }
                    return Json(new { success = false, message = "Failed to save purchase order." });
                }
                   
                return Json(new { success = true, message = "Purchase order saved successfully.", id = result });
            }
            catch (Exception ex)
            {
                errorLogsDAL.InsertErrorLogs(ex.Message, ex.StackTrace);
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }

        public ActionResult DownloadFile(string storedName, string originalName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(storedName) || string.IsNullOrWhiteSpace(originalName))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                errorLogsDAL.CurrentUserId = CurrentUserId;
                string folderPath = System.Configuration.ConfigurationManager.AppSettings["UploadFolder"];
                string fullPath = Path.Combine(Server.MapPath(folderPath), storedName);

                if (!System.IO.File.Exists(fullPath))
                    return HttpNotFound("File not found.");

                string contentType = MimeMapping.GetMimeMapping(fullPath);                
                return File(fullPath, contentType, originalName);
            }
            catch (Exception ex)
            {
                errorLogsDAL.InsertErrorLogs(ex.Message, ex.StackTrace);
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "An error occurred while downloading the file.");
            }
        }


    }
}