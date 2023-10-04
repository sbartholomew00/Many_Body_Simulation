using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Media.Media3D;

namespace Many_Body_Simulation
{
    internal class VisualBody : ModelVisual3D, Body
    {
        private Tuple<double, double, double> _Position;
        public Tuple<double, double, double> Position
        {
            get { return _Position; }
            set
            {
                _Position = value;
                Transform = new TranslateTransform3D(_Position.Item1, _Position.Item2, _Position.Item3);
            }
        }
        public Tuple<double, double, double> Velocity { get; set; }
        public double Mass { get; set; }

        public VisualBody(VisualBodyState state)
        {
            Position = state.position;
            Velocity = state.velocity;
            Mass = state.mass;

            CreateMesh();
        }

        public VisualBody(Tuple<double, double, double> position, Tuple<double, double, double> velocity, double mass)
        {
            Position = position;
            Velocity = velocity;
            Mass = mass;

            CreateMesh();
        }

        private void CreateMesh()
        {
            MeshGeometry3D mesh = new();

            // Sphere
            int numberOfStrips = 100; // >=3
            int sectionsPerStrip = 10; // >=2
            double radius = 0.1;

            mesh.Positions.Add(new Point3D(0.0, radius, 0.0));
            mesh.Positions.Add(new Point3D(0.0, -radius, 0.0));

            int verticiesPerStrip = 2 * sectionsPerStrip - 2; // Not including polar points

            for (int i = 0; i < numberOfStrips; ++i)
            {
                double startAngle = (double)i * 2 * Math.PI / numberOfStrips;
                double endAngle = (double)(i + 1) * 2 * Math.PI / (double)numberOfStrips;
                double anglePerSection = Math.PI / sectionsPerStrip;
                mesh.Positions.Add(new Point3D(-radius * Math.Sin(startAngle) * Math.Sin(anglePerSection),
                    radius * Math.Cos(anglePerSection),
                    radius * Math.Cos(startAngle) * Math.Sin(anglePerSection)));
                mesh.Positions.Add(new Point3D(-radius * Math.Sin(endAngle) * Math.Sin(anglePerSection),
                    radius * Math.Cos(anglePerSection),
                    radius * Math.Cos(endAngle) * Math.Sin(anglePerSection)));
                mesh.TriangleIndices.Add(0);
                mesh.TriangleIndices.Add(3 + i * verticiesPerStrip);
                mesh.TriangleIndices.Add(2 + i * verticiesPerStrip);
                for (int j = 2; j < sectionsPerStrip; ++j)
                {
                    mesh.Positions.Add(new Point3D(-radius * Math.Sin(startAngle) * Math.Sin(j * anglePerSection),
                        radius * Math.Cos(j * anglePerSection),
                        radius * Math.Cos(startAngle) * Math.Sin(j * anglePerSection)));
                    mesh.Positions.Add(new Point3D(-radius * Math.Sin(endAngle) * Math.Sin(j * anglePerSection),
                        radius * Math.Cos(j * anglePerSection),
                        radius * Math.Cos(endAngle) * Math.Sin(j * anglePerSection))); // Each point is added twice
                    mesh.TriangleIndices.Add(2 + (j - 2) * 2 + i * verticiesPerStrip);
                    mesh.TriangleIndices.Add(3 + (j - 2) * 2 + i * verticiesPerStrip);
                    mesh.TriangleIndices.Add(5 + (j - 2) * 2 + i * verticiesPerStrip);

                    mesh.TriangleIndices.Add(2 + (j - 2) * 2 + i * verticiesPerStrip);
                    mesh.TriangleIndices.Add(5 + (j - 2) * 2 + i * verticiesPerStrip);
                    mesh.TriangleIndices.Add(4 + (j - 2) * 2 + i * verticiesPerStrip);
                }
                mesh.TriangleIndices.Add(4 + (sectionsPerStrip - 3) * 2 + i * verticiesPerStrip);
                mesh.TriangleIndices.Add(5 + (sectionsPerStrip - 3) * 2 + i * verticiesPerStrip);
                mesh.TriangleIndices.Add(1);

            }

            /* test shape
            mesh.Positions.Add(new Point3D(0.0, 0.0, 0.0));
            mesh.Positions.Add(new Point3D(0.1, -0.1, 0.0));
            mesh.Positions.Add(new Point3D(-0.1, -0.1, 0.0));
            mesh.Positions.Add(new Point3D(0.0, -0.1, 0.1));

            mesh.TriangleIndices.Add(0);
            mesh.TriangleIndices.Add(1);
            mesh.TriangleIndices.Add(2);

            mesh.TriangleIndices.Add(0);
            mesh.TriangleIndices.Add(3);
            mesh.TriangleIndices.Add(1);

            mesh.TriangleIndices.Add(0);
            mesh.TriangleIndices.Add(2);
            mesh.TriangleIndices.Add(3);

            mesh.TriangleIndices.Add(1);
            mesh.TriangleIndices.Add(3);
            mesh.TriangleIndices.Add(2);*/

            DiffuseMaterial material = new(System.Windows.Media.Brushes.Brown);
            Content = new GeometryModel3D(mesh, material);
        }

        public struct VisualBodyState
        {
            public Tuple<double, double, double> position { get; set; }
            public Tuple<double, double, double> velocity { get; set; }
            public double mass { get; set; }
        }

        public VisualBodyState getState()
        {
            VisualBodyState state = new VisualBodyState
            {
                position = Position,
                velocity = Velocity,
                mass = Mass
            };
            return state;
        }
    }
}
