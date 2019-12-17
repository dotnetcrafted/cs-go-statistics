using System.Collections.Generic;
using CsStat.Domain.Entities;

namespace CsStat.Web.Models
{
    public class UsefulLinksViewModel
    {
        public IEnumerable<UsefulInfo> Items { get; set; }
        public bool IsAdminMode { get; set; }
    }
}