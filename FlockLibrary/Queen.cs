using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlockLibrary
{
    public class Queen
    {
        public Vector<double> Position { get; set; }
        public Color Color { get; set; }
        public Queen()
        { 
            setPosition(50,50);

        }

        public void setPosition(double x, double y)
        {
            Position = DenseVector.OfArray(new double[] { x, y });
        }
    }
}
