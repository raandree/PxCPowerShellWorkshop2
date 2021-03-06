using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My
{
    public enum CarColor
    {
        black = 1,
        white = 2,
        red = 3
    }
    public class Car
    {
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public int Speed { get; set; }
        public CarColor Color { get; set; }
        public bool IsConvertible { get; set; }

        public double SpeedMph
        {
            get
            {
                return Speed / 1.6;
            }
        }

        public void Accelerate(int km)
        {
            Speed += km;
        }

        public void Brake(int km)
        {
            Speed -= km;
        }

    }
}
