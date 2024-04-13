using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float turnSpeed = 40.0f;

    [SerializeField] private Rigidbody playerRb;
    [SerializeField] float horsePower;
    [SerializeField] GameObject centerOfMass;

    float horizontalInput;
    float forwardInput;

    [SerializeField] TextMeshProUGUI speedometerText;
    [SerializeField] TextMeshProUGUI gearText;
    int gear;
    int gearInterval = 26;
    [SerializeField] List<WheelCollider> allWheels;
    [SerializeField] int wheelsOnGround;        

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerRb.centerOfMass = centerOfMass.transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (IsOnGround())
        {
            // Axis setup
            horizontalInput = Input.GetAxis("Horizontal");
            forwardInput = Input.GetAxis("Vertical");

            // Move the vehicle forward
            //transform.Translate(Vector3.forward * Time.deltaTime * speed * forwardInput);
            playerRb.AddRelativeForce(Vector3.forward * forwardInput * horsePower);

            // Rotate the vehicle left and right
            transform.Rotate(Vector3.up, turnSpeed * horizontalInput * Time.deltaTime);

            // Display speed in kph
            speed = playerRb.velocity.magnitude;
            int kph = (int)(speed * 3.6f);
            speedometerText.text = "Speed: " + kph + "kph";

            // Display gear
            if (kph % gearInterval == 0)
            {
                gear = kph / gearInterval;
                gearText.SetText("Gear: " + gear);
            }
        }        
    }
    private bool IsOnGround()
    {
        wheelsOnGround = 0;
        foreach (WheelCollider wheel in allWheels)
        {
            if (wheel.isGrounded)
            {
                wheelsOnGround++;
            }
        }
        if (wheelsOnGround >= 2)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
