using System.Collections.Generic;

namespace Models
{
    public class Map
    {
        public int xSize { get; set; }
        public int zSize { get; set; }
        public List<ExternalRessource> Models { get; set; }
        public List<ExternalRessource> Textures { get; set; }
        public int[][] CasesModels { get; set; }
        public int[][] CasesTextures { get; set; }

        public static Map Instancied(int xSize, int zSize)
        {
            Map mapModel = new Map();
            mapModel.Models = new List<ExternalRessource>();
            mapModel.Textures = new List<ExternalRessource>();
            mapModel.xSize = xSize;
            mapModel.zSize = zSize;
            mapModel.CasesModels = new int[zSize][];
            mapModel.CasesTextures = new int[zSize][];
            for (int i = 0; i < zSize; i++)
            {
                mapModel.CasesTextures[i] = new int[xSize];
                mapModel.CasesModels[i] = new int[xSize];
            }
            return mapModel;
        }
    }

    public class ExternalRessource
    {
        public int Value { get; set; }
        public string Path { get; set; }
    }

    public class ModelsProperties
    {

    }
}
