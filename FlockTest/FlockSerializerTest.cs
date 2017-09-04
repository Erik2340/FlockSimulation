using System;
using FlockLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra;

namespace FlockTest
{
    [TestClass]
    public class FlockSerializerTest
    {
        [TestMethod]
        public void TestString()
        {
            var serializer = FlockSerializer.Instance;

            var flock = MakeTestFlock();

            Assert.IsNotNull(flock.Birds);
            Assert.AreEqual(2, flock.Birds.Count);
            var mertBird = flock.Birds[1];
            Assert.IsNotNull(mertBird);
            Assert.AreEqual("Mert", mertBird.Id);

            var json = serializer.FlockToJson(flock);
            Assert.IsNotNull(json);
            Assert.IsTrue(json.Contains("Mert"));

            flock = serializer.JsonToFlock(json);
            Assert.IsNotNull(flock);
            Assert.IsNotNull(flock.Birds);
            Assert.AreEqual(2, flock.Birds.Count);
            mertBird = flock.Birds[1];
            Assert.IsNotNull(mertBird);
            Assert.AreEqual("Mert", mertBird.Id);
        }

        [TestMethod]
        public void TestNorm()
        {
            Vector<double> v1 = DenseVector.OfArray(new double[] { 1, 1 });
            Assert.AreEqual(Math.Sqrt(2), v1.Norm(2));

            Vector<double> v2 = DenseVector.OfArray(new double[] { 1, 0 });

            var delta = v1 - v2;
            Assert.AreEqual(1, delta.Norm(2));
        }

        [TestMethod]
        public void TestStream()
        {
            var serializer = FlockSerializer.Instance;
            var fileName = Path.GetTempFileName();
            var flock = MakeTestFlock();

            serializer.FlockToFile(flock, fileName);

            Assert.IsTrue(File.Exists(fileName));

            flock = serializer.FileToFlock(fileName);

            // File.Delete(fileName);
            Console.WriteLine(fileName);
            Assert.IsNotNull(flock);
            Assert.IsNotNull(flock.Birds);
            Assert.AreEqual(2, flock.Birds.Count);
            var mertBird = flock.Birds[1];
            Assert.IsNotNull(mertBird);
            Assert.AreEqual("Mert", mertBird.Id);
        }

        private Flock MakeTestFlock()
        {
            return new Flock
            {
                Birds =
                {
                    new Bird {
                        Id = "Erik"  ,
                        Speed = DenseVector.OfArray(new double[] {0, 0})
                    },
                    new Bird {
                        Id = "Mert"
                    }
                }
            };
        }
    }
}
