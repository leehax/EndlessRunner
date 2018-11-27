using System;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerMovement : MonoBehaviour {


 

    private Rigidbody m_rigidBody;
    private Transform m_transform;
    private Camera m_camera;
  

    private float m_cameraZOffset;
    private Vector3 m_consistentVelocity;

    private float m_minX = -1f;
    private float m_maxX = 1f;

    private int m_centerX;
    private float m_previousOnCollisionZ;

	private void Start ()
	{
	    m_rigidBody = GetComponent<Rigidbody>();
	    m_transform = GetComponent<Transform>();
	    m_camera = Camera.main;
	   
	    
	    Assert.IsNotNull(m_camera, "No Main Camera");
	    m_cameraZOffset = m_camera.transform.position.z - m_transform.position.z;
	    m_centerX = Screen.width / 2;
        m_consistentVelocity = new Vector3(0,5,4);
	    m_rigidBody.velocity = m_consistentVelocity;

	}

    private void FixedUpdate()
    {
        Vector3 decreasedVelocity = m_rigidBody.velocity;
        decreasedVelocity.y -= Time.deltaTime * 10f;
        m_rigidBody.velocity = decreasedVelocity;
    }

    private void Update()
    {

        Vector3 newPos = m_transform.localPosition;

        //todo: add touch input functionality
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            print("Updating input on windows");

            if (Input.GetMouseButton(0))
            {

                newPos.x += Input.GetAxis("Mouse X") * Time.deltaTime;
                newPos.x = Mathf.Clamp(newPos.x, m_minX, m_maxX);
            }
        }

        else if (Application.platform == RuntimePlatform.Android)
        {
            print("Updating input on android");

            Vector2 initialTouchPos = new Vector2();
            if (Input.touchCount == 1)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    initialTouchPos = touch.position;
                }

                else if (touch.phase == TouchPhase.Moved)
                {
                    float deltaX = (touch.position.x - initialTouchPos.x)*Time.deltaTime;
                    deltaX = Mathf.Clamp(deltaX, -1f, 1f);

                    newPos.x = deltaX;
                }

               // float diffToCenter = touch.position.x - m_centerX;

                //deltaX= Mathf.Clamp(deltaX, -1f, 1f);

                //newPos.x += diffToCenter;

            }

            print(newPos.x);
        }

        if (Math.Abs(m_transform.localPosition.x - newPos.x) > 0f)
        {
            m_transform.localPosition = newPos;
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
