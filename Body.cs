using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Many_Body_Simulation
{
    /// <summary>
    /// Contains the state of a body
    /// </summary>
    internal interface Body
    {
        public Tuple<double, double, double> Position { get; set; }
        public Tuple<double, double, double> Velocity { get; set; }
        public double Mass { get; set; }
    }
}
