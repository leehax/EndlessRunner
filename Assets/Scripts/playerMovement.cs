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

    private float m_mouseSensitivity = 0.1f;
    private float m_touchSensitivity = 0.5f;
    private float m_previousOnCollisionZ;

    [SerializeField] private AnimationCurve m_movementOnY;
    private float m_curZ;


    private bool m_bounced = true;

    private float m_radius;
    private float m_playStartTime;
    
    private void Start ()
	{
	    m_rigidBody = GetComponent<Rigidbody>();
	    m_transform = GetComponent<Transform>();
	    m_camera = Camera.main;
	    m_initialMousePos = new Vector2();

	    m_radius = GetComponent<SphereCollider>().radius*m_transform.localScale.x; //scale the radius accordingly

        Assert.IsNotNull(m_camera, "No Main Camera");
	    m_cameraZOffset = m_camera.transform.position.z - m_transform.position.z;
	    m_movementOnY.AddKey(0f, 1f);
	    m_movementOnY.keys = new[] {new Keyframe(0f, 1f){outTangent = 8f}, new Keyframe(0.25f, 3f), new Keyframe(0.5f, 1f){inTangent = -8f}};
	    m_movementOnY.postWrapMode = WrapMode.Loop;

	    GameSettings.Instance().PauseGame();
	}

    private void FixedUpdate()
    {
        if (GameSettings.Instance().GameState() == GameSettings.GameStates.Paused)
        {
            if (!Input.GetMouseButton(0))
            {
                return;
            }

            m_playStartTime = Time.fixedTime;
            GameSettings.Instance().StartGame();

        }

        Vector3 newPos = m_rigidBody.position;
    
        if (GameSettings.Instance().GameState() == GameSettings.GameStates.GameOver)
        {
            newPos.y -= 4f * Time.deltaTime;
            newPos.z += 4f * Time.deltaTime;
            m_rigidBody.position = newPos;
        }
        else if(GameSettings.Instance().GameState() == GameSettings.GameStates.Playing)
        {
            float prevY = newPos.y;
            newPos.y = m_movementOnY.Evaluate(PlayTime()); //use animation curve for movement on Y axis, up and down in 1 second as defined in the curve

            float deltaY = newPos.y - prevY;
            if (deltaY > 0f && !m_bounced) //check when a bounce happened
            {
               if (Physics.OverlapSphere(m_rigidBody.position, m_radius).Length <= 1)  //if player is not colliding with anything, lose
                {
                    GameSettings.Instance().EndGame();
                    return;
                }

                m_bounced = true;
                GameSettings.Instance().AddScore(1);

            }
            else if (deltaY < 0f)
            {
                m_bounced = false;
            }

           
            newPos.z += (GameSettings.Instance().DistanceBetweenPlatforms()*2) * Time.deltaTime; //consistently move distance between tiles per second
            m_rigidBody.position = newPos;

        }

    }
  
    private void Update() //todo: split up this method to android and pc update
    {
        Vector3 newPos = m_rigidBody.position;
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            if (Input.GetMouseButtonDown(0))
            {
                m_initialMousePos.x = Input.mousePosition.x;
            }
            else if (Input.GetMouseButton(0) )
            {
                float delta = (Input.mousePosition.x - m_initialMousePos.x) * m_mouseSensitivity;
                newPos.x += delta * Time.deltaTime;
                float tileScale = GameSettings.Instance().TileScale().x;
                newPos.x = Mathf.Clamp(newPos.x, -2f * tileScale, 2f * tileScale);
            }
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.touchCount == 1)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Moved)
                {
                    float deltaX = touch.deltaPosition.x * Time.deltaTime * m_touchSensitivity;
                    newPos.x += deltaX;
                    float tileScale = GameSettings.Instance().TileScale().x;
                    newPos.x = Mathf.Clamp(newPos.x, -2f * tileScale, 2f * tileScale);
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
            GameSettings.Instance().EndGame();

        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Obstacle"))
        {
           if (other.gameObject.GetComponent<DecoyTile>() != null && other.gameObject.GetComponent<DecoyTile>().enabled)
            {
                other.gameObject.GetComponent<DecoyTile>().ExplodeMesh();
            }

            GameSettings.Instance().EndGame();
        }
    }
    private float PlayTime()
    {
        return Time.fixedTime - m_playStartTime;
    }
   

}
