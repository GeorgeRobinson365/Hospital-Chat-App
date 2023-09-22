using Backend.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Backend.Database
{
    public class CloudRequest<T> : ICloudRequest<T> where T: class
    {
        public DateTime Time { get; set; }
        public string DataContentType { get; set; }
        public T Data { get; set; }
    }
}
