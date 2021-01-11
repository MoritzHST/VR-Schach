using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBehaviour : MonoBehaviour
{
    public Vector3 target;

    private Piece piece;
    private Vector3 force;
    private float previousDistance;
    private Rigidbody rb;

    public void Start()
    {
        piece = this.gameObject.GetComponent<Piece>();
        rb = piece.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (!piece.moving)
        {
            force = (target - piece.transform.position);
            previousDistance = (target - piece.transform.position).magnitude;
            rb.AddForce(force);
            piece.moving = true;
        } else
        {
            if ((target - piece.transform.position).magnitude > previousDistance)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                piece.moving = false;

                this.gameObject.transform.position = target;

                Destroy(this);
            } else
            {
                previousDistance = (target - piece.transform.position).magnitude;
                rb.AddForce(force);
            }
        }
    }


}
