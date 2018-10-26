using nz.Rishaan.DynamicCuboidTerrain;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Starter : MonoBehaviour
{

    public AudioSource src;
    public Scene scene;
    // Use this for initialization
    void Start()
    {
        Run();

    }

    public void Run()
    {
        Object.DontDestroyOnLoad(src.gameObject);
        SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
    }
}
