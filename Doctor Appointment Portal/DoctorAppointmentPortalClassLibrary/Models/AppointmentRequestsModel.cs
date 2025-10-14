using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointmentPortalClassLibrary.Models
{
    public class AppointmentRequestsModel
    {
        public int AppointmentRequestId { get; set; }

        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public int StatusId { get; set; }
        public int SpecializationId { get; set; }
        public string MedicalConcern { get; set; }
        public DateTime PreferredDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public TimeSpan FinalStartTime { get; set; }
        public TimeSpan FinalEndTime { get; set; }
        public DateTime FinalDate { get; set; }

        public DateTime CreatedOn { get; set; }
        public int LastModifiedBy { get; set; }
        public DateTime LastModifiedOn { get; set; }

        public string SpecializationName { get; set; }
        public string PatientName { get; set; }
        public string DoctorName { get; set; }
        public string StatusName { get; set; }

        public string SearchedPatientName { get; set; }
        public string SearchedDoctorName { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set;
        }
        public List<AppointmentRequestsModel> AppointmentRequestList { get; set; }
        public List<SpecializationsModel> SpecializationsList { get; set; }
    }
}
