﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarStore.AppContracts.Dtos
{
    public class OrderDto
    {
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }

        public decimal Price { get; set; }

        public string ProductName { get; set; }

        public Guid OwnerId { get; set; }

        public Guid BuyerId { get; set; }

        public string? PictureUrl { get; set; }
    }
}
