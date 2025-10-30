using DoctorAppointmentPortalClassLibrary.Models;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace DoctorAppointmentPortalClassLibrary.DAL
{
    public class Talukas
    {
        private Database db;

        public Talukas()
        {
            db = DatabaseFactory.CreateDatabase();
        }

        public List<TalukasModel> GetList()
        {
            List<TalukasModel> talukaList = new List<TalukasModel>();

            try
            {
                DbCommand com = db.GetStoredProcCommand("TalukasGetList");
                DataSet ds = db.ExecuteDataSet(com);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    foreach (DataRow row in dt.Rows)
                    {
                        TalukasModel taluka = new TalukasModel()
                        {
                            TalukaId = Convert.ToInt32(row["TalukaId"]),
                            TalukaName = Convert.ToString(row["TalukaName"]),
                            DistrictId = Convert.ToInt32(row["DistrictId"]),
                            IsActive = Convert.ToBoolean(row["IsActive"])
                        };

                        talukaList.Add(taluka);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return talukaList;
        }

        public List<TalukasModel> GetTalukasByDistrict(int districtId)
        {
            List<TalukasModel> talukaList = new List<TalukasModel>();

            try
            {
                DbCommand com = db.GetStoredProcCommand("GetTalukasByDistrict");
                if (districtId > 0)
                    db.AddInParameter(com, "@DistrictId", DbType.Int32, districtId);
                else
                    db.AddInParameter(com, "@DistrictId", DbType.Int32, DBNull.Value);


                DataSet ds = db.ExecuteDataSet(com);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    foreach (DataRow row in dt.Rows)
                    {
                        TalukasModel taluka = new TalukasModel()
                        {
                            TalukaId = Convert.ToInt32(row["TalukaId"]),
                            TalukaName = Convert.ToString(row["TalukaName"]),
                            DistrictId = Convert.ToInt32(row["DistrictId"])
                        };

                        talukaList.Add(taluka);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return talukaList;
        }

    }
}
