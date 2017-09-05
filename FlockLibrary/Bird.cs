using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlockLibrary
{
    public class Bird
    {

        public const double NOSE_LENGTH = 20.0;
        public const double TAIL_LENGTH = 6.0;
        public const double WING_LENGTH = 4.0;
        public Vector<double> Position { get; set; }
        public Vector<double> Speed { get; set; }
        public Color Color { get; set; }
        readonly Random _Random = new Random();
        private readonly Matrix<double> Rotate90 = DenseMatrix.OfArray(new double[,]
        {
            {0, 1 },
            {-1, 0 }
        });
        public string Id { get; set; }
       public double fearlvl { get; set; }





        public Vector<double> Vectorbetween(Bird other)
        {
            return this.Position - other.Position;
        }
        public Vector<double> Vectorbetween(Queen other)
        {
            return this.Position - other.Position;
        }
        public Vector<double> Vectorbetween(Predator other)
        {
            return this.Position - other.Position;
        }




        public Bird()
        {
            Position = DenseVector.OfArray(new double[] { 0, 0 });
            Speed = DenseVector.OfArray(new double[] { 0, 0 });
            Id = null;
            Color = new Color(0, 0, 0);
        }

        public Vector<double> NormalizedSpeed
        {
            get
            {
                if (Speed.Norm(1.0) == 0.0)
                {
                    var phi = _Random.NextDouble() * Math.PI * 2;
                    return DenseVector.OfArray(new double[] { Math.Cos(phi), Math.Sin(phi) });
                }
                else
                {
                    return Speed.Normalize(1.0);
                }
            }
        }

        public void SetSpeed(double vx, double vy)
        {
            Speed = DenseVector.OfArray(new double[] { vx, vy });
        }

        public void SetPosition(double x, double y)
        {
            Position = DenseVector.OfArray(new double[] { x, y });
        }

        public Vector<double> NosePosition
        {
            get
            {
                return Position + NormalizedSpeed * NOSE_LENGTH;
            }
        }

        public Vector<double> TailPosition
        {
            get
            {
                return Position - NormalizedSpeed * TAIL_LENGTH;
            }
        }

        public Vector<double> TipPosition1
        {
            get
            {
                return TailPosition + Rotate90 * NormalizedSpeed * WING_LENGTH;
            }
        }

        public Vector<double> TipPosition2
        {
            get
            {
                return TailPosition - Rotate90 * NormalizedSpeed * WING_LENGTH;
            }
        }
    }
}
