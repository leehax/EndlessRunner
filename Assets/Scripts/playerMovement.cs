using System;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerMovement : MonoBehaviour {


 

    private Rigidbody m_rigidBody;
    private Transform m_transform;
    private Camera m_camera;
  
    private float m_cameraZOffset;
    private Vector3 m_consistentVelocity;
    private Vector2 m_initialMousePos;


    private float m_previousOnCollisionZ;

	private void Start ()
	{
	    m_rigidBody = GetComponent<Rigidbody>();
	    m_transform = GetComponent<Transform>();
	    m_camera = Camera.main;
	    m_initialMousePos = new Vector2();

        Assert.IsNotNull(m_camera, "No Main Camera");
	    m_cameraZOffset = m_camera.transform.position.z - m_transform.position.z;
        m_consistentVelocity = new Vector3(0,5,4);
	    m_rigidBody.velocity = m_consistentVelocity;

	}

    private void FixedUpdate()
    {
        Vector3 decreasedVelocity = m_rigidBody.velocity;
        decreasedVelocity.y -= Time.deltaTime * 10f;
        m_rigidBody.velocity = decreasedVelocity;

    }

    private void Update() //todo: clean up this method
    {

        Vector3 newPos = m_rigidBody.position;

        //todo: add touch input functionality
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {

            if (Input.GetMouseButtonDown(0))
            {
                m_initialMousePos.x = Input.GetAxis("Mouse X");
            }
            else if (Input.GetMouseButton(0) )
            {
                float delta = Input.GetAxis("Mouse X") - m_initialMousePos.x;
                newPos.x += delta*Time.deltaTime;
                newPos.x = Mathf.Clamp(newPos.x, -1f, 1f);
            }
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.touchCount == 1)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Moved)
                {
                    float deltaX = touch.deltaPosition.x * Time.deltaTime;
                    newPos.x += deltaX;
                    newPos.x = Mathf.Clamp(newPos.x, -1f, 1f);
                }
            }
        }
        if (Math.Abs(m_rigidBody.position.x - newPos.x) > 0f)
        {
            m_rigidBody.position = newPos;
        }
    }

    private void LateUpdate()
    {
        Vector3 newCamPos = m_camera.transform.position;
        newCamPos.z = m_transform.position.z + m_cameraZOffset;
        m_camera.transform.position = newCamPos;
    }

    private void OnCollisionEnter(Collision other)
    {
        //todo: check if other is an obstacle or platform etc.

        print("Jump Distance" + CalcDistanceJumped(m_transform.position.z));
        Vector3 downwardConsistentVel = m_consistentVelocity;
        downwardConsistentVel.y = -downwardConsistentVel.y;
        m_rigidBody.velocity = Vector3.Reflect(downwardConsistentVel, Vector3.up);

    }

    //todo: debugging functionality, remove
    private float CalcDistanceJumped(float curZ)
    {
        float distance = curZ - m_previousOnCollisionZ;
        m_previousOnCollisionZ = curZ;
        return distance;
    }

   

}
