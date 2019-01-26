﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pillow : MonoBehaviour {
    public bool Picked;
    public bool Throwed; // if the pillow is throwed, then do damage

    private GameObject holder;
	// Use this for initialization
	void Start () {
        Picked = false;
    }
	
	// Update is called once per frame
	void FixedUpdate (){
        // pillow follow
        if (Picked && Vector3.Distance(transform.position, holder.transform.position) > 2f) {
            //print(Vector3.Distance(transform.position, holder.transform.position));
            transform.position = Vector3.Lerp(transform.position, holder.transform.position, 0.1f);
        }
    }

    public void Pick(GameObject player){
        Picked = true;
        holder = player;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (Throwed)
            {
                // doing dmg TODO
                Throwed = false;
            }
        }
    }
}
