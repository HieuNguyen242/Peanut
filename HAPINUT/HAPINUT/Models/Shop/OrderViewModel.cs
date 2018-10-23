﻿using HAPINUT.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HAPINUT.Models.Shop
{
    public class OrderViewModel
    {
        public OrderViewModel()
        {

        }

        public OrderViewModel(Order order)
        {
            OrderId = order.OrderId;
            UserId = order.UserId;
            OrderDate = order.OrderDate;
        }

        public int OrderId { get; set; }
        public int? UserId { get; set; }
        public DateTime? OrderDate { get; set; }


    }
}