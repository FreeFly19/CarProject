using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjecrtCars.Models
{
    public class CarPatch
    {
        public string Id { get; set; }
        public string Company { get; set; }
        public string Model { get; set; }
        public int? Year { get; set; }
        public List<Owner> Owners { get; set; }
    }
}