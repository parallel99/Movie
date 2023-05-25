using Movie_Api.Entities;
using System.Linq.Expressions;

namespace Movie_Api.Base
{
    public class BaseFilter
    {
        public int Skip { get; set; } = 0;
        public int Take { get; set; } = int.MaxValue;
        public bool Ascending { get; set; } = false;
        public string OrderBy { get; set; } = "";
    }
}
