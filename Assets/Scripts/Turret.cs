using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nz.Rishaan.Projectiles;

public class Turret : MonoBehaviour {

    public GameObject barrel;
    public GameObject barrelEnd;
    public Camera cam;

    public float barrelRotation {
        get {
            return
        }
    }

    public GameObject shellPrefab;
    int currentAmmo = 0;
    Modifier[/*Ammo*/][/*Modifiers*/] modList;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //Modify local rotation so that .rotation returns rotation relative to world


        //RayCast to terrain
        RaycastHit hit = new RaycastHit();
        int mask = 1 << 8;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hit, Mathf.Infinity, mask);
        //Physics.Raycast(cam.transform.position, cam.transform.rotation.eulerAngles, out hit, Mathf.Infinity, mask);
        if (hit.collider != null) {
            switch (hit.collider.gameObject.layer) {
                case 0:
                //case gui:
                    
                    break;
                case 1:
                    Quaternion q = Quaternion.LookRotation(hit.collider.gameObject.transform.position - barrel.transform.position);
                    barrel.transform.rotation = q;
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
        for (int i = 0; i < modList.GetLength(1); ++i) {
            shell.AddModifer(modList[currentAmmo][i]);
        }
    }
}
