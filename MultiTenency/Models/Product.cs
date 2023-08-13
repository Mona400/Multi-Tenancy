﻿using MultiTenency.Contracts;

namespace MultiTenency.Models
{
    public class Product : IMustHaveTenant
    {
        public int Id { get; set; }
        public string Name { get; set; } = null;
        public string Description { get; set; } = null;
        public int Rate { get; set; }
        public string TenentId { get; set; } = null;
    }
}
