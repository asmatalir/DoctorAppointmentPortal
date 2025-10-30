using LibraryManagementSystemClassLibrary.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LibraryManagementSystemClassLibrary.Models;

namespace WebApplication1.Controllers
{
    //[JwtAuthorize]
    public class BooksController : ApiController
    {
        Books booksDAL = new Books();
        Publishers publishersDAL = new Publishers();
        Departments departmentsDAL = new Departments();
        Suppliers suppliersDAL = new Suppliers();
        Users usersDAL = new Users();

        [HttpGet]
        public IHttpActionResult GetLists()
        {

            try
            {
                var publishers = publishersDAL.GetList(); 

                var response = new PublishersModel
                {
                    PublishersList = publishers
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new { message = "Server error while loading publishers list." });
            }
        }


        [HttpPost]
        public IHttpActionResult Bookgetlist(BooksModel model)
        {
            HttpResponseMessage result = null;
            try
            {
                var list = booksDAL.GetList(model);
                var response = new BooksModel
                {
                    BookList = list,
                    TotalBooksCount = model.TotalBooksCount  
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new { message = "Server error while fetching books." });
            }

        }

        [HttpGet]
        public IHttpActionResult AddEdit(int id)
        {
            BooksModel model;
            try
            {
                if(id!=0)
                {
                    model = booksDAL.LoadBooks(id);
                }
                else
                {
                    model = new BooksModel();
                }
                model.PublishersList = publishersDAL.GetList();
                model.DepartmentsList = departmentsDAL.GetList();
                model.SuppliersList = suppliersDAL.GetList();
                model.UsersList = usersDAL.GetList();

                return Ok(model);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new { message = "Server error while loading book details." });
            }
        }

        [HttpPost]
        public IHttpActionResult SaveAddEdit(BooksModel model)
        {
           
            try
            {
                if(model.BookId!= 0)
                {
                    booksDAL.BooksUpdate(model);
                    return Ok(new { success = true, message = "Book updated successfully." });
                }
                else
                {
                    booksDAL.InsertBooks(model);
                    return Ok(new { success = true, message = "Book created successfully."});
                }
                

            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new { message = "Server error while saving book." });
            }           

        }

        [HttpPost]
        public IHttpActionResult BookDelete(int BookId)
        {

            try
            {

                booksDAL.BookId = BookId;
                booksDAL.ModifiedBy = 1;
                if (booksDAL.DeleteBooks())
                {
                    return Ok(new { success = true, message = "Book deleted successfully." });
                }
                else
                {
                    return Content(HttpStatusCode.InternalServerError, new { message = "Server error while deleting book." });
                }

            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError,new { message = "Server error while deleting book." });
            }

        }

    }
}
