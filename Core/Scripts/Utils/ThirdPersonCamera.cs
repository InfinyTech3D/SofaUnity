using UnityEngine;
using System.Collections;

/// <summary>
/// Script to Add to a Camera to control it using the mouse.
/// </summary>
public class ThirdPersonCamera : MonoBehaviour 
{
	// camera min/max angles
	private const float Y_ANGLE_MIN = -50.0f;
	private const float Y_ANGLE_MAX = 50.0f;

	public GameObject target;	
    public float m_currentScale = 0.001f;
	public float m_spanSpeed = 0.001f;
	public Vector3 m_lookAtStatic;
    public float m_cameraDistance = 100.0f;
	public float heartBeat = 0.0f;

	private float m_currentX = 0.0f;
	private float m_currentY = 0.0f;
	private float m_sensivityX = 10.0f;
	private float m_sensivityY = 10.0f;

	float m_cameraDistanceMax = 20000f;
	float m_cameraDistanceMin = 10f;
	
	float m_scrollSpeed = 2000.0f;

	private bool m_leftButtonHold = false;
    private bool m_rightButtonHold = false;

    // Use this for initialization
    void Start () 
	{
		m_leftButtonHold = false;
        m_rightButtonHold = false;

        if (target != null)
            m_lookAtStatic = target.transform.position;

    }
	
	// Update is called once per frame
	void Update () 
	{
        // check if left mouse button is clicked
        if (Input.GetMouseButtonDown(0))
            m_leftButtonHold = true;
        else if (Input.GetMouseButtonDown(1))
            m_rightButtonHold = true;

        // check if left mouse button is released
        if (Input.GetMouseButtonUp (0)) 
			m_leftButtonHold = false;
        else if (Input.GetMouseButtonUp(1))
            m_rightButtonHold = false;

        // if left button is hold compute new position
        if (m_leftButtonHold)			
		{
			m_currentX += Input.GetAxis ("Mouse X") * m_sensivityX;
			m_currentY += Input.GetAxis ("Mouse Y") * m_sensivityY;
			m_currentY = Mathf.Clamp (m_currentY, Y_ANGLE_MIN, Y_ANGLE_MAX);
		}

        if (m_rightButtonHold)
        {
            m_lookAtStatic.x += Input.GetAxis("Mouse X") * m_cameraDistance * m_spanSpeed;
            m_lookAtStatic.y -= Input.GetAxis("Mouse Y") * m_cameraDistance * m_spanSpeed;
        }

		m_cameraDistance -= Input.GetAxis("Mouse ScrollWheel") * m_scrollSpeed*m_currentScale;
		m_cameraDistance = Mathf.Clamp(m_cameraDistance, m_cameraDistanceMin*m_currentScale, m_cameraDistanceMax*m_currentScale);		
	}

	void LateUpdate()
	{	
		Vector3 dir = new Vector3 (0, 0, -m_cameraDistance);
		Quaternion rotation = Quaternion.Euler (-m_currentY, m_currentX, 0);

		Vector3 newPos = m_lookAtStatic + rotation * dir;
		Vector3 newlook = m_lookAtStatic;
		if (heartBeat != 0.0f)
        {
            newPos.y += heartBeat * Mathf.Sin(Time.time);
            newlook.y += heartBeat * Mathf.Sin(Time.time);
        }

        transform.position = newPos;
		transform.LookAt (newlook);
	}
}
