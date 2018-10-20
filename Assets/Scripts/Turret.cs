using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nz.Rishaan.Projectiles;

public class Turret : MonoBehaviour {

    public GameObject barrel;
    public GameObject barrelEnd;
    public Camera cam;

    public float barrelRotationVert {
        get {
            return barrel.transform.localRotation.eulerAngles.x;
        }
        set {
            barrel.transform.localRotation = Quaternion.Euler(value,0,0);
        }
    }

    public float barrelRotationHor
    {
        get
        {
            return transform.rotation.eulerAngles.y;
        }
        set
        {
            transform.rotation = Quaternion.Euler(0, value, 0);
        }
    }

    public GameObject shellPrefab;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //Modify local rotation so that .rotation returns rotation relative to world
        if (Input.GetMouseButtonDown(0)) Fire();

        //RayCast to terrain
        RaycastHit hit = new RaycastHit();
        int mask = 1 << 8;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hit, Mathf.Infinity);
        //Physics.Raycast(cam.transform.position, cam.transform.rotation.eulerAngles, out hit, Mathf.Infinity, mask);
        if (hit.collider != null) {
            switch (hit.collider.gameObject.layer) {
                case 0:
                //case gui:
                    
                    break;
                case 8:
                    Quaternion q = Quaternion.LookRotation(hit.collider.gameObject.transform.position - barrel.transform.position, Vector3.up);
                    barrelRotationVert = Mathf.Clamp(-90f - q.eulerAngles.x,-105f,-61f);
                    barrelRotationHor = 180f + q.eulerAngles.y;
                    //Vector3.RotateTowards(barrel.transform.rotation.eulerAngles, hit.collider.gameObject.transform.position - barrel.transform.position);
                    break;
                default:
                    break;
            }
        }
    }

    public void Fire() {
        GameObject obj = Instantiate(shellPrefab, barrelEnd.transform.position, Quaternion.LookRotation(barrelEnd.transform.position-barrel.transform.position));
        Shell shell = obj.GetComponent<Shell>();
        shell.GetComponent<Rigidbody>().velocity = transform.rotation * Vector3.forward * 1f;
    }
}
