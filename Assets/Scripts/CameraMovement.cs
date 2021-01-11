using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
  public float turnSpeed = 1.0f;
  public float dragSpeed = 5.0f;
  public float zoomSpeed = 0.5f;

  private Vector3 previous;
  private Vector3 dragOrigin;

  // Update is called once per frame
  void Update()
  {
      // Rotate camera
      if (Input.GetMouseButton(0))
      {
        float rotateHorizontal = Input.GetAxis ("Mouse X");
        float rotateVertical = Input.GetAxis ("Mouse Y");
        transform.RotateAround (Camera.main.transform.position, -Vector3.up, rotateHorizontal * turnSpeed); //use transform.Rotate(-transform.up * rotateHorizontal * sensitivity) instead if you dont want the camera to rotate around the player
        transform.RotateAround (Camera.main.transform.position, transform.right, rotateVertical * turnSpeed); // again, use transform.Rotate(transform.right * rotateVertical * sensitivity) if you don't want the camera to rotate around the player
      }

      // Move the camera
      if (Input.GetMouseButtonDown(1))
        {
            dragOrigin = Input.mousePosition;
            return;
        }
      if (Input.GetMouseButton(1))
      {
        Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - previous);
        Vector3 move = new Vector3(pos.x * dragSpeed, pos.y * dragSpeed, 0);

        transform.Translate(move, Space.Self);
      }

      // Zoom
      if (Input.mouseScrollDelta.y != 0)
      {
          float dir = 0;
          if (Input.mouseScrollDelta.y < 0){
            dir = -1;
          }
          else {
            dir = 1;
          }
          Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - previous);

          Vector3 move = dir  * zoomSpeed * transform.forward;
          transform.Translate(move, Space.World);
      }

      previous = Input.mousePosition;
  }

}
