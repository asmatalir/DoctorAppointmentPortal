using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClassLibrary1.DAL;
using System.Data;

namespace WebApplication1.Controllers
{
    public class BookController : Controller
    {
        // GET: Book

        Books booksDAL = new Books();
        public ActionResult Index()
        {
            DataSet ds = booksDAL.GetList();
            DataTable dt = ds.Tables[0]; 

            return View(dt);
        }
    }
}