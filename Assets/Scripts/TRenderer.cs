using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace nz.rishaan.DynamicCuboidTerrain
{

    public class TRenderer : MonoBehaviour
    {
        public Player player;
        public Transform cam;
        public TerrainMap map;
        public Transform[,] cuboids; //Should always have odd dimensions

        public Transform[,] groundCuboids;

        public GameObject waterPrefab;
        public GameObject groundPrefab;

        public float asympConst = 0.9f;
        public float speed = 14f;
        Vector3 desiredCamPos = new Vector3();

        public int mapSize;
        float accumTime;
        public int renderRange;
        public float minBlockSize = 1f;

        public float scrollSpeed = 1f;
        public float camSpeed = 5f;

        Vector3 direction = new Vector3();
        List<Vector3> lastdirection = new List<Vector3>();

        public TRenderer()
        {

        }

        void Update()
        {
            Vector3 dir = GetInputTranslationDirection();
            if (dir.sqrMagnitude > 0)
            {
                move(dir);
            }
            accumTime += scrollSpeed * Time.deltaTime;
            map.procGen(mapSize, mapSize, player.x - 0.5f * renderRange + accumTime, player.z - 0.5f * renderRange);
            updateHeights();
            float y = map.heightMap[-cuboids.GetLength(0) / 2 + (int)(player.x) + (int)Mathf.Round(player.obj.transform.position.x), -cuboids.GetLength(1) / 2 + (int)player.z + (int)Mathf.Round(player.obj.transform.position.z)];
            Vector3 a = new Vector3(player.obj.transform.position.x, 0, player.obj.transform.position.z);
            Vector3 b = new Vector3((int)player.gloPos.x,0,(int)player.gloPos.z);
            a = 0.1f * a + 0.9f * b - a;
            player.obj.transform.position += a * 10f * Time.deltaTime;
            player.obj.transform.position += new Vector3(0,0.01f*player.obj.transform.position.y + 0.99f * y- player.obj.transform.position.y, 0)*10f*Time.deltaTime;
            desiredCamPos = player.obj.transform.position + new Vector3(-20, 20, -20);
            cam.position += (((1 - asympConst) * cam.position + asympConst * desiredCamPos) - cam.position) * camSpeed * Time.deltaTime;
            
        }

        void move(Vector3 dir)
        {
            dir = player.gloPos + dir;
            dir.x = Mathf.Clamp(dir.x, 0f, renderRange-1);
            dir.z = Mathf.Clamp(dir.z, 0f, renderRange-1);
            player.gloPos = dir;
            Debug.Log(dir.x);
            

            /*
            player.position += dir;
            player.x = Mathf.Clamp(player.x, renderRange, mapSize - renderRange);
            player.z = Mathf.Clamp(player.z, renderRange, mapSize - renderRange);
            player.height = map.heightMap[(int)player.x, (int)player.z];
            */
        }

        void add(Vector3 v) {
            int i = lastdirection.IndexOf(v);
            if (i >= 0) lastdirection.RemoveAt(i);
            lastdirection.Add(v);
        }

        void remove(Vector3 v) {
            int i = lastdirection.IndexOf(v);
            if (i >= 0) lastdirection.RemoveAt(i);
        }

        Vector3 GetInputTranslationDirection()
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                add(Vector3.right);
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                add(Vector3.left);
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                add(Vector3.forward);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                add(Vector3.back);
            }
            if (Input.GetKeyUp(KeyCode.W))
            {
                remove(Vector3.right);
            }
            if (Input.GetKeyUp(KeyCode.S))
            {
                remove(Vector3.left);
            }
            if (Input.GetKeyUp(KeyCode.A))
            {
                remove(Vector3.forward);
            }
            if (Input.GetKeyUp(KeyCode.D))
            {
                remove(Vector3.back);
            }
            /*if (Input.GetKey(KeyCode.Q))
            {
                direction += Vector3.down;
            }
            if (Input.GetKey(KeyCode.E))
            {
                direction += Vector3.up;
            }*/
            direction = lastdirection.ElementAt<Vector3>(lastdirection.Count-1);
            direction.Normalize();
            return direction * speed * Time.deltaTime;
        }

        void Start()
        {
            lastdirection.Insert(0, new Vector3());
            player.position = new Vector3(renderRange, 0, renderRange);
            if (mapSize % 2 == 0) mapSize++;
            if (renderRange % 2 == 0) renderRange++;
            map.generate(mapSize, mapSize, 0, 0);
            groundCuboids = new Transform[renderRange, renderRange];
            for (int x = 0; x < groundCuboids.GetLength(0); x++)
            {
                for (int z = 0; z < groundCuboids.GetLength(1); z++)
                {
                    GameObject cube = Instantiate<GameObject>(groundPrefab, new Vector3(x, 0, z), Quaternion.identity);
                    //GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    //cube.AddComponent<Rigidbody>();
                    //cube.transform.position = new Vector3(x, 0, z);
                    groundCuboids[x, z] = cube.transform;
                }
            }
            cuboids = new Transform[renderRange, renderRange];
            for (int x = 0; x < cuboids.GetLength(0); x++)
            {
                for (int z = 0; z < cuboids.GetLength(1); z++)
                {
                    GameObject cube = Instantiate<GameObject>(waterPrefab, new Vector3(x, 0, z), Quaternion.identity);
                    //GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    //cube.AddComponent<Rigidbody>();
                    //cube.transform.position = new Vector3(x, 0, z);
                    cuboids[x, z] = cube.transform;
                }
            }
            //player.obj = Instantiate(player.prefab, new Vector3(renderRange/2,0,renderRange/2), Vector3.forward) as GameObject;
            //p.AddComponent<Material>();
            updateHeights();
            int loc = renderRange / 2;
            player.obj.transform.position = new Vector3(loc, cuboids[loc, loc].position.y + 1, loc);
        }

        /*Spherical render range
        float renderRange {
          get => Mathf.Sqrt(renderRangeSqrd);
          set => renderRangeSqrd = Math.Pow(value, 2);
        }
        float renderRangeSqrd;*/


        public void CreateBetweenTwo(Transform transform, float top, float bot) {
            Vector3 v = transform.position;
            v.y = 0.5f * top + 0.5f * bot;
            transform.position = v;
            transform.localScale = new Vector3(1f, (top-bot), 1f);
        }

        //Call on movement not on frame update
        public void updateHeights()
        {
            int X = cuboids.GetLength(0);
            int Z = cuboids.GetLength(1);
            for (int x = 0; x < X; x++)
            {
                for (int z = 0; z < Z; z++)
                {
                    /*Debug.Log("x: " + xShift + " : " + x);
                    Debug.Log("z: " + zShift + " : " + z);
                    */
                    //Debug.Log("y: " + map.heightMap[xShift + x,zShift + z]);
                    //float temp = Mathf.Abs(x + z - player.x - player.z);
                    //if (temp < 10) {
                    float relHeight = map.heightMap[-X / 2 + x + (int)(player.x), -Z / 2 + z + (int)player.z] - 0.5f;
                    //relHeight += Mathf.Clamp(temp,0,10);
                    float scale = Mathf.Max(relHeight - map.minHeight,minBlockSize);
                    relHeight = relHeight - 0.5f * scale;
                    //Instantaneous
                    groundCuboids[x, z].localPosition = new Vector3(x, relHeight - 5f, z);
                    CreateBetweenTwo(groundCuboids[x, z], relHeight - scale/2, -10f);

                    cuboids[x, z].localPosition = new Vector3(x, relHeight, z);
                    cuboids[x, z].localScale = new Vector3(1, scale, 1);

                    
                    
                    //}
                    //Asymptotic Averaging
                    //float max = speed * Time.deltaTime;


                    /*cuboids[x,z].localPosition = new Vector3(x,
                      Mathf.Clamp(asympConst*relHeight,-max,max)
                        + (1-asympConst)*cuboids[x,z].localPosition.y,z);*/

                    /*cuboids[x,z].localScale = new Vector3 (1f,
                      Mathf.Clamp(asympConst*scale,-max,max)
                        + (1-asympConst)*cuboids[x,z].localScale.y,1f);*/
                }
            }
            //int loc = renderRange / 2;
            //player.obj.transform.position = new Vector3(loc, cuboids[loc, loc].position.y + 1, loc);
            //player.obj.transform.position = new Vector3(0,player.height + 1,0);
        }
    }
}
