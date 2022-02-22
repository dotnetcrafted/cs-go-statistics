using System;
using System.Collections.Generic;
using System.Linq;
using CsStat.Domain;
using CsStat.Domain.Entities.Demo;
using CsStat.Web.Models.Matches;

namespace CsStat.Web.Models
{
    public class MatchesViewModel
    {
        public IEnumerable<BaseMatch> Matches { get; set; }
        public Pagination Pagination { get; set; }
    }

    public class Pagination
    {
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalItems { get; set; }
    }
}