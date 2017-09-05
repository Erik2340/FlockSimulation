using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlockLibrary
{
    public class Predator
    {
        public Vector<double> Position { get; set; }
        public Color Color { get; set; }
        public Vector <double> Speed { get; set; }
        public Vector<double> size { get; set; }
        public double killzone { get; set; }

        public Predator()
        {
            Position = DenseVector.OfArray(new double[] { 100, 100 });
            size = DenseVector.OfArray(new double[] { 10, 10 });
            killzone = 10;
        }

        public void setPosition(double x, double y)
        {
            Position = DenseVector.OfArray(new double[] { x, y });
        }
        public void setSpeed(double vx, double vy)
        {
            Speed = DenseVector.OfArray(new double[] { vx, vy });
        }
        public Vector<double> Vectorbetween(Bird other)
        {
            return this.Position - other.Position;
        }
    }
}
