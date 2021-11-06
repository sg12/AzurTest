using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform floor;

	public float turnSpeed = 2.0f;      
	public float zoomSpeed = 1.0f;     

	private Vector3 mouseOrigin;  
	private bool isRotating;   
	private bool isZooming;  

	void Update()
	{
		//Debug.Log(Input.GetMouseButton(1));
        if (Input.mousePosition.y > Screen.height*0.75f)
			return;

		if (Input.GetMouseButtonDown(0))
		{
			mouseOrigin = Input.mousePosition;
            if (UIController.IsRotatingByDragOnScreen)
				isRotating = true;
			else
				isZooming = true;
		}


		if (!Input.GetMouseButton(0))
		{
			isRotating = false;
			isZooming = false;
		}


		if (isRotating)
		{
			Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - mouseOrigin);
			transform.RotateAround(transform.position,
								transform.right,
								-pos.y * turnSpeed);
        }

        if (transform.rotation.eulerAngles.x > 75f)
        {
			transform.rotation = Quaternion.Euler(75f,
				transform.rotation.eulerAngles.y,
				transform.rotation.eulerAngles.z);
		}
		if (transform.rotation.eulerAngles.x < 15f)
		{
			transform.rotation = Quaternion.Euler(15f,
				transform.rotation.eulerAngles.y,
				transform.rotation.eulerAngles.z);
		}
		bool needStopZoom = false;
        if (isZooming && Input.mousePosition.y < mouseOrigin.y)
        {
			Vector3 posLeftBorder = floor.position;
			posLeftBorder.x -= floor.localScale.x / 2f;
			posLeftBorder.z -= floor.localScale.z / 2f;
			Vector3 posLeftBorderOnScreen = Camera.main.WorldToScreenPoint(posLeftBorder, Camera.MonoOrStereoscopicEye.Mono);
            if (posLeftBorderOnScreen.x>0 && posLeftBorderOnScreen.x < Screen.width)
            {
				needStopZoom = true;
			}
		}

		if (isZooming
			&& (
			(transform.position.y >= 1.0f
			&& Input.mousePosition.y >= mouseOrigin.y)
			||
			(!needStopZoom
			&& Input.mousePosition.y < mouseOrigin.y)
			)
			)
		{
			Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - mouseOrigin);
			Vector3 move = pos.y * zoomSpeed * transform.forward;
			transform.Translate(move, Space.World);
        }
	}
}
