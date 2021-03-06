﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yak : MonoBehaviour {

    [SerializeField] private float migrateSpeed;
    [SerializeField] private float bounceForce;

    private Vector3 migrateDirection;
    [SerializeField] private int currentRouteIndex;

    private MigrateEvent me;

    // Use this for initialization
    void Start()
    {
        currentRouteIndex = 2;

        me = LevelEventManager.Instance.GetComponent<MigrateEvent>();
        transform.LookAt(me.MigratePoints[currentRouteIndex].position);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeSelf)
        {
            transform.Translate(migrateDirection * migrateSpeed * Time.deltaTime, Space.World);
        }
    }

    public void StartMigrate(Vector3 direction)
    {

    }

    public void StopMigrate()
    {
        OnDestroy();
    }

    public void SetMigrateDirection(Vector3 direction)
    {
        migrateDirection = direction;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Boarder")
            StopMigrate();

        if (other.transform.GetComponent<Player>() != null)
        {
            Vector3 orientation = other.transform.position - transform.position;
            orientation.y = 0;

            other.transform.GetComponent<Player>().PushBack(orientation.normalized, 1f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        //if (other.gameObject.tag == "Player")
        //{
        //    Rigidbody rigidbody = other.transform.parent.GetComponent<Rigidbody>();
        //    //Bounce back the player
        //    rigidbody.isKinematic = false;
        //    rigidbody.AddForce(-other.transform.right * bounceForce);
        //    Debug.Log(-other.transform.right * bounceForce);
        //}

        if (other.gameObject.tag == "RoutePoint")
        {         
            if (currentRouteIndex < me.MigratePoints.Length - 1) {
                if (other.gameObject.name == me.MigratePoints[currentRouteIndex].name)
                {
                    currentRouteIndex++;
                    migrateDirection = (me.MigratePoints[currentRouteIndex].position - transform.position).normalized;
                    transform.LookAt(me.MigratePoints[currentRouteIndex].position);
                }               
            }
        }

    }

    private void OnDestroy()
    {
        gameObject.SetActive(false);
    }

}
