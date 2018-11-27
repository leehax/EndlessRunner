using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour {


    enum Lanes
    {
        Left,
        Center,
        Right
    }

    private Lanes m_curLane = Lanes.Center;
    private float m_forwardSpeed = 400f;
    private Rigidbody m_rigidBody;
    private Transform m_transform;
    private Camera m_camera;
    private float m_cameraZOffset;


	
	void Start ()
	{
	    m_rigidBody = GetComponent<Rigidbody>();
	    m_transform = GetComponent<Transform>();
	    m_camera = Camera.main;
	    m_cameraZOffset = m_camera.transform.position.z - m_transform.position.z;

	}
	
	void Update ()
	{

	    Vector3 newVel = new Vector3(0,0, m_forwardSpeed * Time.deltaTime);
	    Vector3 newPos = m_transform.localPosition;
     
        //todo: add touch input functionality

	    if (Input.GetKeyDown(KeyCode.A) && m_curLane > Lanes.Left)
	    {
            //todo: play animation
	        newPos.x -= 1f;
	    }

	    if (Input.GetKeyDown(KeyCode.D) && m_curLane < Lanes.Right)
	    {
            //todo: play animation
	        newPos.x += 1f;
	    }

	    m_curLane = (Lanes)newPos.x + 1;
	    if (m_transform.localPosition != newPos)
	    {
	        m_transform.localPosition = newPos;
	    }

	    m_rigidBody.velocity = newVel;

	}

    void LateUpdate()
    {
        Vector3 newCamPos = m_camera.transform.position;
        newCamPos.z = m_transform.position.z + m_cameraZOffset;
        m_camera.transform.position = newCamPos;
    }
}
