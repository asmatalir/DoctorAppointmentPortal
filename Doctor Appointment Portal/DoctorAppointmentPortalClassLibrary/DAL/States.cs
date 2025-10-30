using DoctorAppointmentPortalClassLibrary.Models;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace DoctorAppointmentPortalClassLibrary.DAL
{
    public class States
    {
        private Database db;

        public States()
        {
            db = DatabaseFactory.CreateDatabase();
        }

        public List<StatesModel> GetList()
        {
            List<StatesModel> stateList = new List<StatesModel>();

            try
            {
                DbCommand com = db.GetStoredProcCommand("StatesGetList");
                DataSet ds = db.ExecuteDataSet(com);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    foreach (DataRow row in dt.Rows)
                    {
                        StatesModel state = new StatesModel()
                        {
                            StateId = Convert.ToInt32(row["StateId"]),
                            StateName = Convert.ToString(row["StateName"]),
                            IsActive = Convert.ToBoolean(row["IsActive"])
                        };

                        stateList.Add(state);
                    }
                }
            }
            catch (Exception ex)
            {
                throw; 
            }

            return stateList;
        }
    }
}
