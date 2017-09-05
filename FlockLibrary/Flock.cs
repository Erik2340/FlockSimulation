using System.Collections.Generic;

namespace FlockLibrary
{
    public class Flock
    {

        public List<Bird> Birds { get; set; }
        public Queen Queen { get; set; }
        public Predator Predator { get; set; }
        public double KineticEnergy
        {
            get
            {
                var energy = 0.0;
                foreach (var bird in Birds)
                {
                    energy += 0.5 * bird.Speed.Norm(2) * bird.Speed.Norm(2);                    
                }
                return energy;
            }
        }

        public Flock()
        {
            Birds = new List<Bird>();
            Queen = new Queen();
        }
    }
}
