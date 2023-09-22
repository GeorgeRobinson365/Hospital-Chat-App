using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Commands
{
    public class SendMessage
    {
        public string Content { get; set; }
        public string SenderId { get; set; }
        public string ChatId { get; set; }
    }
}
