using System;
using System.Collections.Generic;

namespace ProjecrtCars.Models
{
    public class Car
    {
        public string Id { get; set; }
        public string Company { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public List<Owner> Owners{ get; set; }
    }
}