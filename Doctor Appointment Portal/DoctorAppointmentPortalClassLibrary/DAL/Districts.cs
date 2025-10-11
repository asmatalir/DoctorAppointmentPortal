using DoctorAppointmentPortalClassLibrary.Models;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace DoctorAppointmentPortalClassLibrary.DAL
{
    public class Districts
    {
        private Database db;

        public Districts()
        {
            db = DatabaseFactory.CreateDatabase();
        }

        public List<DistrictsModel> GetList()
        {
            List<DistrictsModel> districtList = new List<DistrictsModel>();

            try
            {
                DbCommand com = db.GetStoredProcCommand("DistrictsGetList");
                DataSet ds = db.ExecuteDataSet(com);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    foreach (DataRow row in dt.Rows)
                    {
                        DistrictsModel district = new DistrictsModel()
                        {
                            DistrictId = Convert.ToInt32(row["DistrictId"]),
                            DistrictName = Convert.ToString(row["DistrictName"]),
                            StateId = Convert.ToInt32(row["StateId"]),
                            IsActive = Convert.ToBoolean(row["IsActive"])
                        };

                        districtList.Add(district);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetList: {ex.Message}");
            }

            return districtList;
        }
    }
}
