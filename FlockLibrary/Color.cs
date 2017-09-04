namespace FlockLibrary
{
    public class Color
    {

        public Color(byte r, byte g, byte b)
        {
            Red = r;
            Green = g;
            Blue = b;
        }

        public Color() { }

        public byte Red { get; set; }
        public byte Green { get; set; }
        public byte Blue { get; set; }
    }
}