using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yape.AntiFraud.Application.DTO
{
    public class ResultService
    {
        public bool Success { get; set; } = false;
        public dynamic Data { get; set; }
        public string Messaje { get; set; }
        public string Error { get; set; } = string.Empty;
        public string CodError { get; set; }
    }
}
