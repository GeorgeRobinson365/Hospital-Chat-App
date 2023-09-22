using Backend.Interfaces;
using Backend.Model;

namespace Backend.Commands
{
    public class SubmitPatientAccess
    {
        public string DoctorId { get; set; }
        public string PatientId { get; set; }
        // public string Fullname {get; set;}
    }
}