using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class player : NetworkBehaviour
{
    private bool onFloor = true;
    private float jump_force = 7F;
    private Rigidbody body;
    public override void OnStartLocalPlayer()
    {
        Camera.main.transform.SetParent(transform);
        Camera.main.transform.localPosition = new Vector3(0, 0, 0);
        body = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!isLocalPlayer) { return; }
    
        float moveX = Input.GetAxis("Horizontal") * Time.deltaTime * 110.0f;
        float moveZ = Input.GetAxis("Vertical") * Time.deltaTime * 4f;

        transform.Rotate(0, moveX, 0);
        transform.Translate(0, 0, moveZ);

        if (Input.GetKeyDown(KeyCode.Space) && onFloor){
            body.AddForce(new Vector3(0, jump_force, 0), ForceMode.Impulse);
            onFloor = false;
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.tag == "Floor")
            onFloor = true;
    }
}