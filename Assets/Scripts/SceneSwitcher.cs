using nz.Rishaan.DynamicCuboidTerrain;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneSwitcher : MonoBehaviour
{
    public static SceneSwitcher ss;
    // Use this for initialization
    void Start()
    {
        ss = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator LoadCredits()
    {
        yield return new WaitForSeconds(4);
        SceneManager.LoadSceneAsync(2, LoadSceneMode.Single);
    }

    public static void Credits()
    {
        ss.StartCoroutine("LoadCredits");
    }
}
