using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjecrtCars.Models
{
    public class UpdateOwner
    {
        public string CarId { get; set; }
        public string Name { get; set; }
        public string NewName { get; set; }
        public string NewPeriod { get; set; }
    }
}