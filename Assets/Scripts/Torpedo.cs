using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace nz.Rishaan.DynamicCuboidTerrain
{

    public class Torpedo : MonoBehaviour
    {
        public GameObject Eff;
        public bool collided = false;
        public bool thruster = true;
        public bool thruster_enabled_in_water = true;
        public float thrust = 1f;
        public float water_decel = 1f;
        public bool in_water = false;
        public float speed = 20f;
        public float offset = 0f;

        private void Start()
        {
            Destroy(this.gameObject, Constants.DESPAWN_TIME_SHELL);
            thruster = true;
            thruster_enabled_in_water = true;
            thrust = 10f;
            water_decel = 0f;
            speed = 20f;
        }

        public void Update()
        {
            Vector3 v = transform.localPosition;
            if (!collided && (thruster || in_water))
            {
                if (in_water)
                    speed -= water_decel * Time.deltaTime;
                if (thruster && (thruster_enabled_in_water || !in_water))
                    speed += thrust * Time.deltaTime;

                v += transform.localRotation * Vector3.forward * speed * Time.deltaTime;
            }
            if (v.x < 0 || v.z < 0 || v.x >= TRenderer.render || v.z >= TRenderer.render) Destroy(this.gameObject);
            else v.y = 0.5f * v.y + 0.5f * (TRenderer.cuboids[(int)v.x, (int)v.z].position.y + TRenderer.cuboids[(int)v.x, (int)v.z].localScale.y / 2 + 0.1f);
            transform.localPosition = v;

        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.gameObject.layer != Constants.LAYER_WATER && !collided) Destroy(this, Constants.DESPAWN_TIME_SHELL);
            else in_water = true;
        }

        private void OnCollisionExit(Collision collision)
        {
            if (collision.collider.gameObject.layer == Constants.LAYER_WATER) in_water = false;
        }
    }
}