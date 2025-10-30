using DoctorAppointmentPortalClassLibrary.Models;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace DoctorAppointmentPortalClassLibrary.DAL
{
    public class Cities
    {
        private Database db;

        public Cities()
        {
            db = DatabaseFactory.CreateDatabase();
        }

        public List<CitiesModel> GetList()
        {
            List<CitiesModel> cityList = new List<CitiesModel>();

            try
            {
                DbCommand com = db.GetStoredProcCommand("CitiesGetList");
                DataSet ds = db.ExecuteDataSet(com);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    foreach (DataRow row in dt.Rows)
                    {
                        CitiesModel city = new CitiesModel()
                        {
                            CityId = Convert.ToInt32(row["CityId"]),
                            CityName = Convert.ToString(row["CityName"]),
                            TalukaId = Convert.ToInt32(row["TalukaId"]),
                            IsActive = Convert.ToBoolean(row["IsActive"])
                        };

                        cityList.Add(city);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return cityList;
        }

        public List<CitiesModel> GetCitiesByTaluka(int talukaId)
        {
            List<CitiesModel> cityList = new List<CitiesModel>();

            try
            {

                DbCommand com = db.GetStoredProcCommand("GetCitiesByTaluka");
                if(talukaId > 0)
                        db.AddInParameter(com, "@TalukaId", DbType.Int32, talukaId);
                else
                        db.AddInParameter(com, "@TalukaId", DbType.Int32, DBNull.Value);

                    DataSet ds = db.ExecuteDataSet(com);

                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        DataTable dt = ds.Tables[0];
                        foreach (DataRow row in dt.Rows)
                        {
                            CitiesModel city = new CitiesModel()
                            {
                                CityId = Convert.ToInt32(row["CityId"]),
                                CityName = Convert.ToString(row["CityName"]),
                                TalukaId = Convert.ToInt32(row["TalukaId"])
                            };

                            cityList.Add(city);
                        }
                    }
            }
            catch (Exception ex)
            {
                throw;
            }

            return cityList;
        }

    }
}
