using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Interfaces
{
    public interface ICloudRequest<T>
    {
        public DateTime Time { get; set; }
        public string DataContentType { get; set; }
    }
}
