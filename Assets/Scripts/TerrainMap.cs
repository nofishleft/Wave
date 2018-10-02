using UnityEngine;

namespace nz.rishaan.DynamicCuboidTerrain
{

    public class TerrainMap : MonoBehaviour
    {
        //Public variables/settings
        public int XSCALE = 10;
        public int ZSCALE = 10;
        public float maxHeight;
        public float minHeight;
        public float blockSize;

        //Getters & Setters


        //Internal
        public float[,] heightMap;

        private TerrainMap()
        {

        }

        public void procGen(int X, int Z, float seedX, float seedZ)
        {
            float heightRange = maxHeight - minHeight;
            for (int x = 0; x < X; x += 1)
            {
                for (int z = 0; z < Z; z += 1)
                {
                    heightMap[x, z] = minHeight + heightRange * Mathf.PerlinNoise(seedX + x * 0.1f, seedZ + z * 0.1f);
                }
            }
        }

        public void generate(int X, int Z, float seedX, float seedZ)
        {
            heightMap = new float[X, Z];
            float heightRange = maxHeight - minHeight;
            for (int x = 0; x < X; x += 1)
            {
                for (int z = 0; z < Z; z += 1)
                {
                    heightMap[x, z] = minHeight + heightRange * Mathf.PerlinNoise(seedX + x * 0.1f, seedZ + z * 0.1f);
                }
            }
        }

    }
}
