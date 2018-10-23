﻿using HAPINUT.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HAPINUT.Models.Shop
{
    public class ProductViewModel
    {
        public ProductViewModel()
        {

        }
        public ProductViewModel(Product product)
        {
            Id = product.Id;
            Name = product.Name;
            Decription = product.Decription;
            Price = product.Price;
            CategoryName = product.CategoryName;
            CategoryId = product.CategoryId;
            Image = product.ImageName;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Decription { get; set; }
        public decimal? Price { get; set; }
        public string CategoryName { get; set; }
        public int? CategoryId { get; set; }
        public string Image { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }
        public IEnumerable<string> GalleryImages { get; set; }
    }
}