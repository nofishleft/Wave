using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CreditStart : MonoBehaviour {

    public Text Wave;
    public Text CreditText;
    string[] Credits;
    float aTime;
    int current;
	// Use this for initialization
	void Start () {
        aTime = 4f;
        current = -1;
        Wave.text = WaveText.Wave;
        Credits = new string[4];
        Credits[0] = "Made by Rishaan Gupta\nFor ProcJam 2018";
        Credits[1] = "Boat taken from Khalkeus' 3D Art Pack\nhttp://www.procjam.com/art/khalkeus.html\nhttp://khalkeus.tumblr.com/\n\nThis art is released under a Creative Commons Attribution-NonCommercial license.";
        Credits[2] = "Music\nBlind Love Dub by Jeris (c) copyright 2017 Licensed under a Creative Commons Attribution (3.0) license.\nhttp://dig.ccmixter.org/files/VJ_Memes/55416 Ft: Kara Square (mindmapthat)";
        Credits[3] = "The font used in this game is copyright (c) Jakob Fischer at www.pizzadude.dk,  all rights reserved.\nDo not distribute without the author's permission.\nUse this font for non - commercial use only!";
    }
	
	// Update is called once per frame
	void Update () {
        aTime += Time.deltaTime;
        if (aTime >= 5f) {
            aTime = 0;
            ++current;
            if (current >= Credits.Length) current = 0;
            CreditText.text = Credits[current];
        }

	}
}
