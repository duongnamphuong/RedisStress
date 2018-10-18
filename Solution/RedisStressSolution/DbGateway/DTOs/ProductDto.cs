using Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbGateway.DTOs
{
    public class ProductDto
    {
        public long Id { get; set; }
        public string Imei { get; set; }
        public int State { get; set; }
        public DateTime? LastHbUtc { get; set; }
    }
}
