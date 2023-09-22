using Backend.Model;
using FirebaseAdmin.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Interfaces
{
    public interface IFileWriter
    {
        public byte[] GeneratePDFBytes(Identity identity, UserRecord user);
    }
}
