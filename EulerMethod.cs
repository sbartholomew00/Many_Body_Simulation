using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Many_Body_Simulation
{
    internal class EulerMethod : ISimulator
    {
        public void Tick<T>(ICollection<T> bodies, double timeStep) where T : Body
        {
            // Update velocities
            foreach(T body in bodies)
            {
                ValueTuple<double, double, double> newVelocity = body.Velocity.ToValueTuple();
                foreach (T otherBody in bodies)
                {
                    if (otherBody.Equals(body))
                        continue;
                    ValueTuple<double, double, double> positionDiff =
                        (otherBody.Position.Item1 - body.Position.Item1,
                        otherBody.Position.Item2 - body.Position.Item2,
                        otherBody.Position.Item3 - body.Position.Item3);
                    double distanceSquared =
                        positionDiff.Item1 * positionDiff.Item1
                        + positionDiff.Item2 * positionDiff.Item2
                        + positionDiff.Item3 * positionDiff.Item3;
                    if (distanceSquared == 0) // Problem
                        continue;
                    double velocityChangeFactor = timeStep * (otherBody.Mass / distanceSquared) / Math.Sqrt(distanceSquared);
                    newVelocity.Item1 += velocityChangeFactor * positionDiff.Item1;
                    newVelocity.Item2 += velocityChangeFactor * positionDiff.Item2;
                    newVelocity.Item3 += velocityChangeFactor * positionDiff.Item3;
                }
                body.Velocity = newVelocity.ToTuple();
            }

            // Update positions using new velocities (not technically Euler?)
            foreach (T body in bodies)
            {
                body.Position = new(body.Position.Item1 + body.Velocity.Item1 * timeStep,
                    body.Position.Item2 + body.Velocity.Item2 * timeStep,
                    body.Position.Item3 + body.Velocity.Item3 * timeStep);
            }
        }
    }
}
