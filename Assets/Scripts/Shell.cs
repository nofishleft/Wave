using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace nz.Rishaan.Projectiles
{

    public class Shell : MonoBehaviour
    {
        

        Rigidbody body;

        public bool collided = false;
        public bool thruster = true;
        public bool thruster_enabled_in_water = true;
        public float thrust = 1f;
        public float water_decel = 1f;
        public bool in_water = false;

        private void Start()
        {
            body = this.GetComponent<Rigidbody>();
        }

        public void FixedUpdate()
        {
            if (!collided && (thruster || in_water))
            {
                if (in_water)
                    body.AddForce(-body.velocity * water_decel);
                if (thruster && (thruster_enabled_in_water || !in_water))
                    body.AddForce(transform.rotation * Vector3.forward * thrust);
            }
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