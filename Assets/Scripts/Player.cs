using UnityEngine;

namespace nz.Rishaan.DynamicCuboidTerrain
{

    public class Player : MonoBehaviour
    {
        public Vector3 position;
        public Vector3 gloPos;
        public GameObject obj;

        public float Health;
        public float MaxHealth;

        //public GameObject prefab;
        /*public float height
        {
            get { return position.y; }
            set { position.y = value; }
        }*/
        public float height
        {
            get { return position.y; }
            set { position.y = value; }
        }
        public float x
        {
            get { return position.x; }
            set
            {
                position.x = value;
            }
        }
        public float z
        {
            get { return position.z; }
            set
            {
                position.z = value;
            }
        }

        public float zPos
        {
            get { return gloPos.z; }
            set
            {
                gloPos.z = value;
            }
        }
        public float xPos
        {
            get { return gloPos.x; }
            set
            {
                gloPos.x = value;
            }
        }

    }

}
