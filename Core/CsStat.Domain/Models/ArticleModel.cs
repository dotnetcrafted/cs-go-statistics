using System;
using System.Collections.Generic;

namespace CsStat.Domain.Models
{
    public class ArticleModel
    {

        public string Id { get; set; }
        public List<Tag> Tags { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }

    public class Tag
    {
        public string Id { get; set; }
        public string Caption { get; set; }

    }
}