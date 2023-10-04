using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Many_Body_Simulation
{
    internal interface I3dDisplay
    {
        void Update();
        public enum Direction
        {
            None,
            Up,
            Left,
            Right,
            Down,
            In,
            Out
        }
        public void SetCameraMovement(Direction direction);
        public void RotateCamera(System.Windows.Vector v);
        public void setVisuals<T>(List<T> newVisuals) where T : System.Windows.Media.Media3D.Visual3D;
    }
}
