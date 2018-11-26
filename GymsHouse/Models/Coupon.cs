using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GymsHouse.Models
{
    public class Coupon
    {
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string CouponType { get; set; }

        public enum ECouponType { Percent = 0, Dollar = 1, Gift = 2 }

        [Required]
        public double Discount { get; set; }

        [Required]
        public double MinimumAmount { get; set; }

        public byte[] Picture { get; set; }

        public bool IsActive { get; set; }

        [Required]
        public string CouponFor { get; set; }

        public enum ECouponFor { Class = 0, Shopping = 1 }

    }
}
