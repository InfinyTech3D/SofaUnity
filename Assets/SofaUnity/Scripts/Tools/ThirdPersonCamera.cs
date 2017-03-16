using UnityEngine;
using System.Collections;

public class ThirdPersonCamera : MonoBehaviour 
{
	// camera min/max angles
	private const float Y_ANGLE_MIN = -50.0f;
	private const float Y_ANGLE_MAX = 50.0f;

	public Transform lookAt;
	private Vector3 m_lookAtStatic;

	public Transform camTransform;

	//private Camera cam;

	private float m_currentX = -5.0f;
	private float m_currentY = 0.0f;
	private float m_sensivityX = 10.0f;
	private float m_sensivityY = 10.0f;

	float m_cameraDistanceMax = 1000f;
	float m_cameraDistanceMin = 30f;
	float m_cameraDistance = 3000.0f;
	float m_scrollSpeed = 200.0f;

	float m_currentScale = 0.01f;

	private bool m_leftButtonHold = false;

	// Use this for initialization
	void Start () 
	{
		camTransform = transform;
		//cam = Camera.main;
		m_leftButtonHold = false;

		// init a static lookat for the moment.
		m_lookAtStatic = new Vector3 (0, 200*m_currentScale, 0);
		m_cameraDistance = m_cameraDistance * m_currentScale;
	}
	
	// Update is called once per frame
	void Update () 
	{		
		// check if left mouse button is clicked
		if (Input.GetMouseButtonDown (0)) {
			m_leftButtonHold = true;
		}

		// check if left mouse button is released
		if (Input.GetMouseButtonUp (0)) {
			m_leftButtonHold = false;
		}

		// if left button is hold compute new position
		if (m_leftButtonHold)			
		{
			m_currentX += Input.GetAxis ("Mouse X") * m_sensivityX;
			m_currentY += Input.GetAxis ("Mouse Y") * m_sensivityY;
			m_currentY = Mathf.Clamp (m_currentY, Y_ANGLE_MIN, Y_ANGLE_MAX);
		}

		m_cameraDistance += Input.GetAxis("Mouse ScrollWheel") * m_scrollSpeed*m_currentScale;
		m_cameraDistance = Mathf.Clamp(m_cameraDistance, m_cameraDistanceMin*m_currentScale, m_cameraDistanceMax*m_currentScale);
	}

	void LateUpdate()
	{
		//if (Input.GetMouseButtonDown (0)) 

		Vector3 dir = new Vector3 (0, 0, -m_cameraDistance);
		Quaternion rotation = Quaternion.Euler (-m_currentY, m_currentX, 0);

		//camTransform.position = lookAt.position + rotation * dir;
		//camTransform.LookAt (lookAt.position);

		camTransform.position = m_lookAtStatic + rotation * dir;
		camTransform.LookAt (m_lookAtStatic);
	}
}
