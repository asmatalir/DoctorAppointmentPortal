using LibraryManagementSystemClassLibrary.DAL;
using LibraryManagementSystemClassLibrary.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace WebApplication1.Controllers
{
    public class BookIssueController : ApiController
    {
        BookIssues bookIssueDAL = new BookIssues();
        Books booksDAL = new Books();
        Members membersDAL = new Members();

        //[RequireRoles(Roles = "Admin,Librarian")]
        [HttpGet]
        public IHttpActionResult AddEditIssueBook(int id)
        {
            try
            {
                var bookIssueDAL = new BookIssues();
                var model = bookIssueDAL.GetIssueById(id);

                if (model == null)
                {
                    return Content(HttpStatusCode.NotFound, new { message = "Issue not found." });
                }

                // Add related lists
                model.BookList = booksDAL.GetList();
                model.MembersList = membersDAL.GetList();

                return Ok(model);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new { message = "Server error while loading issue." });
            }
        }

        //[RequireRoles(Roles = "Admin")]
        [HttpPost]
        public IHttpActionResult SaveBookIssue()
        {
            try
            {
                List<string> savedFilePaths = new List<string>();
                var httpRequest = HttpContext.Current.Request;               
                var jsonModel = httpRequest["model"];
                var model = JsonConvert.DeserializeObject<BookIssueModel>(jsonModel);

                model.CreatedBy = 1;
                if (model.UploadedFiles == null)
                    model.UploadedFiles = new List<FileDetailModel>();

                string uploadFolder = System.Configuration.ConfigurationManager.AppSettings["UploadFolder"];
                //string issueFolderName = $"BookIssue_{model.IssueId}";
                //string folderPath = HttpContext.Current.Server.MapPath(Path.Combine(uploadRootFolder, issueFolderName));

                string folderPath = HttpContext.Current.Server.MapPath(uploadFolder);

                if (httpRequest.Files.Count > 0)
                {
                    if (!Directory.Exists(folderPath))
                        Directory.CreateDirectory(folderPath);

                    for (int i = 0; i < httpRequest.Files.Count; i++)
                    {
                        var file = httpRequest.Files[i];
                        if (file != null && file.ContentLength > 0)
                        {
                            string originalFileName = Path.GetFileName(file.FileName);
                            string extension = Path.GetExtension(file.FileName);
                            string uniqueFileName = $"{model.IssueId}_{Guid.NewGuid()}{extension}";

                            string fullPath = Path.Combine(folderPath, uniqueFileName);
                            file.SaveAs(fullPath);
                            savedFilePaths.Add(fullPath);

                            model.UploadedFiles.Add(new FileDetailModel
                            {
                                FileName = originalFileName,
                                FilePath = uniqueFileName
                            });
                        }
                    }
                }


                
                int result = bookIssueDAL.SaveBookIssueDetails(model);

                if (result <= 0)
                {
                    DeleteFiles(savedFilePaths);
                    return Ok(new { success = false, message = "Failed to save book issue details." });
                }

                if (model.DeletedExistingFiles != null && model.DeletedExistingFiles.Count > 0)
                {
                    var fullPaths = model.DeletedExistingFiles.Select(filePath => Path.Combine(HttpContext.Current.Server.MapPath(uploadFolder), filePath));
                    DeleteFiles(fullPaths);
                }

                return Ok(new { success = true, message = "Book issue saved successfully."});
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new { message = "Unexpected server error." });
            }
        }

        private void DeleteFiles(IEnumerable<string> filePaths)
        {
            foreach (var path in filePaths)
            {
                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);
            }
        }

        [HttpGet]
        public IHttpActionResult DownloadFile(string storedName)
        {
            if (string.IsNullOrWhiteSpace(storedName))
                return BadRequest("Invalid file request");

            string folderPath = System.Configuration.ConfigurationManager.AppSettings["UploadFolder"];
            string fullPath = Path.Combine(HttpContext.Current.Server.MapPath(folderPath), storedName);

            if (!System.IO.File.Exists(fullPath))
                return NotFound();

            string contentType = MimeMapping.GetMimeMapping(fullPath);
            var bytes = System.IO.File.ReadAllBytes(fullPath);

            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(bytes)
            };

            // Just return with storedName (downloaded file name may look like GUID_xxx.pdf)
            result.Content.Headers.ContentDisposition =
                new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                {
                    FileName = storedName
                };
            result.Content.Headers.ContentType =
                new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);

            return ResponseMessage(result);
        }




    }
}
