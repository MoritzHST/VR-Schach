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

    private void OnCollisionExit(Collision collision)
    {
        GameObject obstacle = collision.gameObject;
        GameManager gm = GameManager.instance;
        if (gm.DoesPieceBelongToCurrentPlayer(obstacle) && obstacle.GetComponent<Piece>() != null)
        {
            Destroy(obstacle);
        }
    }


    void FixedUpdate()
    {
        if (!piece.moving)
        {
            force = (target - piece.transform.position);
            previousDistance = (target - piece.transform.position).magnitude;
            rb.AddForce(force);
            BoxCollider collider  = piece.GetComponent<BoxCollider>();
            collider.enabled = true;
            piece.moving = true;
        } else
        {
            if ((target - piece.transform.position).magnitude <= 0.1f)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                BoxCollider collider  = piece.GetComponent<BoxCollider>();
                collider.enabled = false;
                piece.moving = false;
                piece.initialCollisionPosition = new Vector3(0,0,0);
                piece.collisionReady = false;

                this.gameObject.transform.position = target;

                Destroy(this);
            } else
            {
                previousDistance = (target - piece.transform.position).magnitude;
                Vector3 moveVector = target - piece.transform.position;
                float speed = Mathf.Abs(Mathf.Clamp(previousDistance, 0.1f, 0.4f));
                rb.AddForce(moveVector.normalized * speed, ForceMode.Force);
            }
        }
    }


}
