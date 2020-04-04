using System.Collections.Generic;

namespace CsStat.Web.Models
{
    public class UsefulLinksViewModel
    {
        public IEnumerable<InfoViewModel> Items { get; set; }
        public bool IsAdminMode { get; set; }
    }
}