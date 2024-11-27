using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopping.Core.Specification
{
    public class SpecParams
    {
        public string? sort { get; set; }
        public int? brandId { get; set; }
        public int? CategoryId { get; set; }
        public int ? pageSize { get; set; }
        public int? pageIndex { get; set; }
    }
}
