﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public string Name { get; private set; }

    /// <summary>
    /// The score that has been earned by the player
    /// </summary>
    public int Score { get; private set; }

    // Use this for initialization
    [SerializeField]private float speed;
    [SerializeField]private float jumpSpeed;
    [SerializeField]private float gravity;


    [SerializeField] private float minThrowPower;
    [SerializeField] private float maxThrowPower;

    /// <summary>
    /// The number of pillows carried by the player
    /// </summary
    private int numPillowHold;
    private List<Pillow> Pillows;
    private Pillow[] Ammo;
    private float emPower;

    private Vector3 moveDirection = Vector3.zero;
    private CharacterController controller;
    private bool oldTriggerHeldPick;
    void Start()
    {
        numPillowHold = 0;
        controller = GetComponent<CharacterController>();
        Ammo = new Pillow[2];
        Pillows = new List<Pillow>();
        emPower = 0.0f;
        // let the gameObject fall down
        gameObject.transform.position = new Vector3(0, 5, 0);
    }

    void Update()
    {
        // all input interface
        if (controller.isGrounded)
        {
            // We are grounded, so recalculate
            // move direction directly from axes

            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection = moveDirection * speed;

            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpSpeed;
            }
        }
        bool newTriggerHeldPick = Input.GetAxis("Pick") > 0f;
        if(!oldTriggerHeldPick == newTriggerHeldPick)
        {
            if (numPillowHold >= 2)
            {
                // exceed the number that one player can hold 
                ThrowDrop();
            }
            else
            {
                PickUp();
            }
        }
        oldTriggerHeldPick = newTriggerHeldPick;

        if (Input.GetAxis("Throw")>0.1f && emPower < 0.1f)
        {
            if (numPillowHold > 0) {
                // start to empower the throw
                emPower = minThrowPower;
            }
            // do nothing if no pillow
        }
        emPower = emPower < maxThrowPower? emPower + 0.01f : emPower;
        if (Input.GetKeyUp("joystick button 5") && emPower > 0.1f)
        {
            // key released throw
            emPower = 0.0f;
            // throw!!!!
            // TODO: pillow class
        }
        // Apply gravity
        moveDirection.y = moveDirection.y - (gravity * Time.deltaTime);

        // Move the controller
        controller.Move(moveDirection * Time.deltaTime);
    }
    public void ThrowDrop() {

    }

    public void PickUp() {
            for (int i = 0; i < Pillows.Count; i++) {
                var temp = Pillows[i];
                if (!temp.Picked) {
                    temp.Pick(gameObject);
                    numPillowHold++;
                    break;
                }
            }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Pillow") {
            print("pillow found");
            var temp = other.GetComponent<Pillow>();
            if (temp.Throwed){
                // doing dmg TODO

            }else{
                // sign into queue for pick up
                if (!temp.Picked) { Pillows.Add(temp);}
            }
        }   
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Pillow")
        {
            var temp = other.GetComponent<Pillow>();
            if (Pillows.Contains(temp)) { 
                // sign out queue
                Pillows.Remove(temp);
            }
        }
    }
}