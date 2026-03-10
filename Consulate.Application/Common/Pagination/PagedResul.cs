using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consulate.Application.Common.Pagination
{
    public class PagedResult<T>
    {
        public IEnumerable<T> Items { get; set; } = new List<T>();//employees list
        public int TotalCount { get; set; }//total number of employees in the database

        public int PageNumber { get; set; }//current page number

        public int PageSize { get; set; }  //number of items per page
    }
}
