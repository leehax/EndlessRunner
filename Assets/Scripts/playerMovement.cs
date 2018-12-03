using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour {
    private enum PlayState
    {
        Alive,
        Dead,
    }

    private Rigidbody m_rigidBody;
    private Transform m_transform;
    private Camera m_camera;
  
    private float m_cameraZOffset;
    private Vector3 m_consistentVelocity;
    private Vector2 m_initialMousePos;

    private float m_inputSensitivity = 0.1f;
    private float m_previousOnCollisionZ;

    public AnimationCurve m_movementOnY;
    private float m_curZ;

    private GameObject m_killZone;
    private PlayState m_state = PlayState.Alive;
    private void Start ()
	{
	    m_rigidBody = GetComponent<Rigidbody>();
	    m_transform = GetComponent<Transform>();
	    m_camera = Camera.main;
	    m_initialMousePos = new Vector2();
	    m_killZone = new GameObject("KillZone");
	    m_killZone.AddComponent<BoxCollider>().isTrigger = true;
	    m_killZone.tag = "Obstacle";
        

        Assert.IsNotNull(m_camera, "No Main Camera");
	    m_cameraZOffset = m_camera.transform.position.z - m_transform.position.z;
	    m_movementOnY.postWrapMode = WrapMode.Loop;

	}

    private void FixedUpdate()
    {

       Vector3 killZonePos = m_rigidBody.position;
       killZonePos.y = 0.25f; //put the killzone collider on the same height as the platform colliders
       m_killZone.transform.position = killZonePos;

        Vector3 newPos = m_rigidBody.position;
           
        if (m_state == PlayState.Dead)
        {
            newPos.y -= 4f * Time.deltaTime;
            newPos.z += 4f * Time.deltaTime;
            m_rigidBody.position = newPos;
        }
        else
        {


            //save prev Y
            //get Y delta
            //when Y delt is positive raycast down 

          
            newPos.y = m_movementOnY.Evaluate(Time.time); //use animation curve for movement on Y axis, up and down in 1 second as defined in the curve
            newPos.z += 4f * Time.deltaTime; //consistently move 4 units per second on the Z axis
            m_rigidBody.position = newPos;
           

            if (m_rigidBody.position.y < 0.9f && m_rigidBody.position.z >= 1f) //player is below the y pos of tiles, and has moved from the starting position
            {
                print(m_rigidBody.position.y + "Dead");
                m_state = PlayState.Dead;
            }

        }


    }

    private void Update() //todo: clean up this method
    {
        Vector3 newPos = m_rigidBody.position;
        //todo: add touch input functionality
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {

            if (Input.GetMouseButtonDown(0))
            {
                m_initialMousePos.x = Input.mousePosition.x;
            }
            else if (Input.GetMouseButton(0) )
            {
                float delta = (Input.mousePosition.x - m_initialMousePos.x) * m_inputSensitivity;
                newPos.x += delta * Time.deltaTime;
                newPos.x = Mathf.Clamp(newPos.x, -3f, 3f);
            }
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.touchCount == 1)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Moved)
                {
                    float deltaX = touch.deltaPosition.x * Time.deltaTime * m_inputSensitivity;
                    newPos.x += deltaX;
                    newPos.x = Mathf.Clamp(newPos.x, -3f, 3f);
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
        if (other.gameObject.CompareTag("Obstacle"))
        {
           // m_rigidBody.velocity = Vector3.zero;
          //  m_state = PlayState.Dead;
            print("hit obstacle");
            if (other.gameObject.GetComponent<DecoyTile>() != null && other.gameObject.GetComponent<DecoyTile>().enabled)
            {
                other.gameObject.GetComponent<DecoyTile>().ExplodeMesh();
            }
        }
        else
        {
        
            print(m_rigidBody.position.y);
        }

    }

    private void OnTriggerEnter(Collider other)
    {

        //if we hit a trigger that is an obstacle, aka the killzone set state to dead, if we also hit another collider, keep state as alive
       //if (Physics.OverlapSphere(m_rigidBody.position, 0.5f).Length < 3)
       //{
       //    print(Physics.OverlapSphere(m_rigidBody.position, 0.5f).Length);
       //    if (other.gameObject.CompareTag("Obstacle") && m_rigidBody.position.z >= 1f)
       //    {
       //        m_state = PlayState.Dead;
       //    }
       //}
    }

    //todo: debugging functionality, remove
    private float CalcDistanceJumped(float curZ)
    {
        float distance = curZ - m_previousOnCollisionZ;
        m_previousOnCollisionZ = curZ;
        return distance;
    }

   

}
