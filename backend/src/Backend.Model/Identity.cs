namespace Backend.Model
{
    public class Identity
    {
        public string Id { get; set; }
        public Role Role { get; set; }
        public string? APIToken { get; set; }
        public string? FullName { get; set; }
        public string[]? Patients { get; set; }
        public Identity(string id, Role role) {
            Id = id;
            Role = role;
            
        }
        public Identity() { }
    }
    public enum Role
    {
        Doctor,
        Patient,
        PendingDoctor,
        PendingPatient,
        Admin
    }
}