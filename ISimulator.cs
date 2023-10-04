using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Many_Body_Simulation
{
    /// <summary>
    /// Defines how different simulation methods will be used
    /// </summary>
    internal interface ISimulator
    {
        /// <summary>
        /// Steps forward the given system by one timeStep
        /// </summary>
        /// <param name="bodies"></param>
        public void Tick<T>(ICollection<T> bodies, double timeStep) where T : Body;
    }
}
