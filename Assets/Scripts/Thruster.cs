using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace nz.Rishaan.DynamicCuboidTerrain
{

    public class Thruster : MonoBehaviour
    {
        public GameObject Eff;

        public bool collided = false;
        public bool thruster = false;
        public bool thruster_enabled_in_water = true;
        public float thrust = 1f;
        public float water_decel = 1f;
        public bool in_water = false;
        public float speed = 20f;
        RFX4_TransformMotion rf;

        public float damage = 60f;

        public float offset = 0f;

        private void Start()
        {
            rf = Eff.GetComponentInChildren<RFX4_TransformMotion>();
            speed = 30f;
        }

        public Transform target;

        public void Update()
        {
            Vector3 v = transform.localPosition;
            if (target != null) transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(target.transform.position - transform.position), 30f*Time.deltaTime);
            if (!collided && (thruster || in_water))
            {
                if (in_water)
                    speed -= water_decel * Time.deltaTime;
                if (thruster && (thruster_enabled_in_water || !in_water))
                    speed += thrust * Time.deltaTime;

                v += transform.localRotation * Vector3.forward * speed * Time.deltaTime;
            }
            if (v.x < 0 || v.z < 0 || v.x >= TRenderer.render || v.z >= TRenderer.render) Destroy(this.gameObject);
            transform.localPosition = v;

        }
        /*
        private void OnTriggerEnter(Collider other)
        {
            Debug.Log(3);
        }

        private void OnTriggerStay(Collider other)
        {
            print(4);
        }
        /*
        //private void OnCollisionEnter(Collision collision)
        //{
            /*
            if (collision.gameObject.layer != Constants.LAYER_WATER && !collided) Destroy(this, Constants.DESPAWN_TIME_SHELL);
            else in_water = true;*/
        //}

        //private void OnCollisionExit(Collision collision)
        //{
        //    if (collision.collider.gameObject.layer == Constants.LAYER_WATER) in_water = false;
        //}
        
    }
}