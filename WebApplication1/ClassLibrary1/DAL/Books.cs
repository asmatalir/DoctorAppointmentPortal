using System;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace ClassLibrary1.DAL
{
    public class Books
    {
        private Database db;
        
        public Books()
        {
            db = DatabaseFactory.CreateDatabase();
        }


        public DataSet GetList()
        {
            DataSet ds = new DataSet();
            try
            {
                DbCommand com = db.GetStoredProcCommand("BooksGetList");
                ds = db.ExecuteDataSet(com);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetList: {ex.Message}");
                throw;
                
            }

            return ds;
        }

    }
}
