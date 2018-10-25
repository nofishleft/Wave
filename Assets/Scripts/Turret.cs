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
    public LayerMask mask;
    public Transform target;

    public float barrelRotationVert {
        get {
            return barrel.transform.localRotation.eulerAngles.x;
        }
        set {
            barrel.transform.localRotation = Quaternion.RotateTowards(barrel.transform.localRotation, Quaternion.Euler(value, 0, 0), 360f * Time.deltaTime);
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
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, value, 0), 360f * Time.deltaTime);
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
        if (TRenderer.PAUSED) return;
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
            target = null;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentAmmo = 1;
            ammoText.text = "Current Ammo: \nHoming Missile";
            target = null;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentAmmo = 2;
            ammoText.text = "Current Ammo: \nTorpedo";
            target = null;
        }
        //Modify local rotation so that .rotation returns rotation relative to world
        
        //RayCast to terrain
        RaycastHit hit = new RaycastHit();
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (currentAmmo == 1)
        {
            Physics.Raycast(ray, out hit, Mathf.Infinity);
        }
        else if (currentAmmo == 0) Physics.Raycast(ray, out hit, Mathf.Infinity/*, ~mask.value*/);
        else Physics.Raycast(ray, out hit, Mathf.Infinity, ~mask.value);
        //Physics.Raycast(cam.transform.position, cam.transform.rotation.eulerAngles, out hit, Mathf.Infinity, mask);
        if (hit.collider != null) {
            Vector3 dest = hit.collider.gameObject.transform.position;
            //print(hit.collider.gameObject.layer);
            switch (hit.collider.gameObject.layer) {
                case 0:
                //case gui:
                    
                    break;
                case 8:
                    dest.y += hit.collider.gameObject.transform.localScale.y/2 + 1f;
                    //Vector3.RotateTowards(barrel.transform.rotation.eulerAngles, hit.collider.gameObject.transform.position - barrel.transform.position);
                    break;
                case 12:
                    target = hit.collider.transform;
                    break;
                default:
                    break;
            }
            Quaternion q = Quaternion.LookRotation(dest - barrel.transform.position, Vector3.up);
            
            //print(q.eulerAngles);
            if (q.eulerAngles.x > 200) barrelRotationVert = -90f - q.eulerAngles.x;
            else barrelRotationVert = Mathf.Clamp(-90f - q.eulerAngles.x, -105f, -61f);
            barrelRotationHor = 180f + q.eulerAngles.y;
            if (Input.GetMouseButtonDown(0)) Fire();
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
        if (o != thruster) rot.y = 0;
        if ((ang <= 45 && ang >= 0) || (ang <= 360 && ang >= 135))
        {
            GameObject ob = Instantiate(o, barrelEnd.transform.position, Quaternion.LookRotation(rot));
            ob.layer = 9;
            if (currentAmmo == 1) {
                ob.GetComponent<Thruster>().target = target;
            }
        }
    }
}
