using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Media.Media3D;
using System.Windows.Media;
using System.Windows.Controls;

namespace Many_Body_Simulation
{
    internal class Viewport3dDerived : Viewport3D
    {
        public void AddBody(Visual3D model)
        {
            Children.Add(model);
        }
    }
}
