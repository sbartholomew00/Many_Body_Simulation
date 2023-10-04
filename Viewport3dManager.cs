using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Media.Media3D;
using static Many_Body_Simulation.I3dDisplay;

namespace Many_Body_Simulation
{
    internal class Viewport3dManager : I3dDisplay
    {
        System.Windows.Controls.Viewport3D viewport;
        Direction direction;
        ModelVisual3D light;
        public Viewport3dManager(System.Windows.Controls.Viewport3D viewport)
        {
            this.viewport = viewport;
            PerspectiveCamera camera = new(new Point3D(0.0, 0.0, 10.0), new Vector3D(0.0, 0.0, -1.0), new Vector3D(0.0, 1.0, 0.0), 90.0);
            viewport.Camera = camera;
            direction = Direction.None;

            // Light
            System.Windows.Media.Color color = new();
            color.R = 255;
            color.G = 255;
            color.B = 255;
            light = new();
            light.Content = new DirectionalLight(color, new Vector3D(0.0, 0.0, -1.0));
            viewport.Children.Add(light);
        }

        // Updates camera only
        public void Update()
        {
            double moveSpeed = 0.05;

            // Camera
            if (viewport.Camera is not PerspectiveCamera camera)
                throw new Exception("Camera not perspective camera");
            Vector3D moveDirection;
            switch (direction)
            {
                case Direction.Up:
                case Direction.Down:
                    moveDirection = camera.UpDirection;
                    moveDirection.Normalize();
                    moveDirection *= moveSpeed;
                    if (direction == Direction.Down)
                        moveDirection *= -1.0;
                    break;
                case Direction.Left:
                case Direction.Right:
                    moveDirection = new(camera.LookDirection.Y * camera.UpDirection.Z - camera.LookDirection.Z * camera.UpDirection.Y,
                        camera.LookDirection.Z * camera.UpDirection.X - camera.LookDirection.X * camera.UpDirection.Z,
                        camera.LookDirection.X * camera.UpDirection.Y - camera.LookDirection.Y * camera.UpDirection.X); // Could just use Vector3D.CrossProduct
                    moveDirection.Normalize();
                    moveDirection *= moveSpeed;
                    if (direction == Direction.Left)
                        moveDirection *= -1.0;
                    break;
                case Direction.In:
                case Direction.Out:
                    moveDirection = camera.LookDirection;
                    moveDirection.Normalize();
                    moveDirection *= moveSpeed;
                    if (direction == Direction.Out)
                        moveDirection *= -1.0;
                    break;
            }

            Vector3D point = (Vector3D)camera.Position;
            point += moveDirection;
            PerspectiveCamera newCamera = new((Point3D)point, camera.LookDirection, new Vector3D(0.0, 1.0, 0.0), 90.0);
            viewport.Camera = newCamera;
        }
        public void SetCameraMovement(Direction newDirection)
        {
            direction = newDirection;
        }
        public void RotateCamera(System.Windows.Vector v) // TODO: fix bug, can get stuck looking vertically
        {
            double rotationSpeed = 0.002;

            if (viewport.Camera is not PerspectiveCamera camera)
                throw new Exception("Camera not perspective camera");

            Vector3D lookDir = camera.LookDirection;
            lookDir.Normalize();
            lookDir -= Vector3D.CrossProduct(camera.LookDirection, camera.UpDirection) * v.X * rotationSpeed; // up,look direction not guaranteed normalised
            lookDir += camera.UpDirection * v.Y * rotationSpeed; // updirection not guaranteed normalised
            lookDir.Normalize();

            PerspectiveCamera newCamera = new(camera.Position, lookDir, new Vector3D(0.0, 1.0, 0.0), 90.0);
            viewport.Camera = newCamera;
        }

        public void setVisuals<T>(List<T> newVisuals) where T : Visual3D
        {
            viewport.Children.Clear();
            viewport.Children.Add(light);
            foreach(T visual in newVisuals)
            {
                viewport.Children.Add(visual);
            }
        }
    }
}
