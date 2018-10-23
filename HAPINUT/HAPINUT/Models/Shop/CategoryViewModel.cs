using HAPINUT.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HAPINUT.Models
{
    public class CategoryViewModel
    {
        public CategoryViewModel()
        { }
        public CategoryViewModel(Category category)
        {
            Id = category.Id;
            Name = category.Name;
        }

        public int Id { get; set; }
        public string Name { get; set; }
    }
}