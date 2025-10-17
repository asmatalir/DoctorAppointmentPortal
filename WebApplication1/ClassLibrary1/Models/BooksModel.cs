using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.Models
{
    public class BooksModel
    {
        public int BookId { get; set; }
        public string BookName { get; set; }
        public string PublisherName { get; set; }
        public string DepartmentName { get; set; }
        public string SupplierName { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }

   
    }
}
