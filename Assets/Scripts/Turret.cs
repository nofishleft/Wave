using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nz.Rishaan.DynamicCuboidTerrain;
using UnityEngine.UI;

public class Turret : MonoBehaviour {

    public GameObject barrel;
    public GameObject barrelEnd;
    Camera cam;
    public Camera OrthographicCamera;
    public Camera PerspectiveCamera;
    public static int currentAmmo = 2;
    public Text ammoText;
    public static bool orthoCamera = true;


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

    

	// Use this for initialization
	void Start () {
        currentAmmo = 0;
        ammoText.text = "Current Ammo: \nShell";
        if (orthoCamera) cam = OrthographicCamera;
        else cam = PerspectiveCamera;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.V)) {
            orthoCamera = !orthoCamera;
            OrthographicCamera.gameObject.SetActive(orthoCamera);
            PerspectiveCamera.gameObject.SetActive(!orthoCamera);
            if (orthoCamera) cam = OrthographicCamera;
            else cam = PerspectiveCamera;
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentAmmo = 0;
            ammoText.text = "Current Ammo: \nShell";
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentAmmo = 1;
            ammoText.text = "Current Ammo: \nThruster";
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentAmmo = 2;
            ammoText.text = "Current Ammo: \nTorpedo";
        }
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
                    Vector3 dest = hit.collider.gameObject.transform.position;
                    dest.y += hit.collider.gameObject.transform.localScale.y/2 + 1f;
                    Quaternion q = Quaternion.LookRotation(dest - barrel.transform.position, Vector3.up);
                    barrelRotationVert = Mathf.Clamp(-90f - q.eulerAngles.x,-105f,-61f);
                    barrelRotationHor = 180f + q.eulerAngles.y;
                    //Vector3.RotateTowards(barrel.transform.rotation.eulerAngles, hit.collider.gameObject.transform.position - barrel.transform.position);
                    break;
                default:
                    break;
            }
        }
    }

    public GameObject shell;
    public GameObject thruster;
    public GameObject torpedo;

    public void Fire() {
        GameObject o;
        switch (currentAmmo) {
            case 0:
                o = shell;
                break;
            case 1:
                o = thruster;
                break;
            case 2:
                o = torpedo;
                break;
            default:
                return;
        }
        float ang = transform.localEulerAngles.y;
        Vector3 rot = barrelEnd.transform.position - barrel.transform.position;
        rot.y = 0;
        if ((ang <= 45 && ang >= 0) || (ang <= 360 && ang >= 135)) Instantiate(o, barrelEnd.transform.position, Quaternion.LookRotation(rot));
        
    }
}
