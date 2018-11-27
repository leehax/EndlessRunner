using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camMovement : MonoBehaviour
{
    private Rigidbody m_rigidbody;

    private Vector3 m_velocity = new Vector3(0,0,3);

	// Use this for initialization
	void Start ()
	{
	    m_rigidbody = GetComponent<Rigidbody>();
	    m_rigidbody.velocity = m_velocity;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
