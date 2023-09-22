using Backend.Interfaces;
using Backend.Model;

namespace Backend.Commands
{
    public class CreateIdentity
    {
        public string Id { get; set; }
        public Role Role { get; set; }
        // public string Fullname {get; set;}
    }
}