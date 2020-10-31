namespace Models
{
    public class Map
    {
        public int xSize { get; set; }
        public int zSize { get; set; }
        public Model Models { get; set; }
        public int[][] Cases { get; set; }
    }

    public class Model
    {
        public int Value { get; set; }
        public string Path { get; set; }
    }
}
