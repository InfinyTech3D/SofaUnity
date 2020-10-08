using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SofaUnity;

public class GripperController : MonoBehaviour
{
    //public SofaDAGNode m_gripperNode = null;

    public List<SofaDAGNode> m_fingers = new List<SofaDAGNode>();
    protected List<SofaMesh> m_fingerDofs;
    protected List<SofaConstraint> m_fingerConstraints;

    protected bool m_isReady = false;

    // Start is called before the first frame update
    void Start()
    {
        m_fingerDofs = new List<SofaMesh>();
        m_fingerConstraints = new List<SofaConstraint>();

        foreach (SofaDAGNode node in m_fingers)
        {
            SofaMesh mesh = node.FindSofaMesh();
            if (mesh == null)
            {
                Debug.LogError("No mesh found in node: " + node.name);
                continue;
            }

            //Debug.Log("mesh: " + mesh.name + " nbrV: " + mesh.NbVertices());
            mesh.SofaMeshTopology.CreateRestVertexBuffer();
            m_fingerDofs.Add(mesh);

            foreach (Transform child in node.transform)
            {                
                if (child.name.Contains("PullingCable"))
                {
                    foreach (Transform component in child.transform)
                    {
                        SofaConstraint sofaC = component.GetComponent<SofaConstraint>();                        
                        if (sofaC != null)
                        {
                            //Debug.Log("SofaConstraint: " + sofaC.name);
                            m_fingerConstraints.Add(sofaC);
                            break;
                        }
                    }
                }
            }
        }


        m_isReady = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            bool moveGripper = false;            
            Vector3 direction = Vector3.zero;
            if (Input.GetKey(KeyCode.UpArrow))
            {
                direction.y = 1.0f;
                moveGripper = true;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                direction.x = 1.0f;
                moveGripper = true;
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                direction.x = -1.0f;
                moveGripper = true;
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                direction.y = -1.0f;
                moveGripper = true;
            }
            else if (Input.GetKey(KeyCode.KeypadPlus))
            {
                MoveFinger(0.1f);
            }
            else if (Input.GetKey(KeyCode.KeypadMinus))
            {
                MoveFinger(-0.1f);
            }

            if (moveGripper)
            {
                MoveGripper(direction);
            }
        }
        
    }


    protected void MoveGripper(Vector3 direction)
    {
        if (!m_isReady)
            return;

        foreach (SofaMesh mesh in m_fingerDofs)
        {
            int nbrV = mesh.NbVertices();

            float[] sofaVertices = mesh.SofaMeshTopology.m_restVertexBuffer;

            // assume we deal with 3d vertices for the moment.
            for (int i = 0; i < nbrV; i++)
            {
                sofaVertices[i * 3] += direction.x;
                sofaVertices[i * 3 + 1] += direction.y;
                sofaVertices[i * 3 + 2] += direction.z;
            }

            mesh.SetRestPositions(sofaVertices);
        }

        foreach (SofaConstraint constraint in m_fingerConstraints)
        {
            SofaVec3Data pullPointData = constraint.m_dataArchiver.GetSofaVec3Data("pullPoint");
            pullPointData.Value += direction;
        }
    }

    bool m_firstTime = true;
    List<float> m_values = null;
    protected void MoveFinger(float value)
    {
        if (m_firstTime)
        {
            m_values = new List<float>();
            foreach (SofaConstraint constraint in m_fingerConstraints)
            {
                int size = constraint.m_impl.GetVectordSize("value");
                if (size == 1)
                {
                    float[] val = new float[1];
                    int res = constraint.m_impl.GetVectordValue("value", 1, val);

                    if (res == 0)
                    {
                        m_values.Add(val[0]);
                    }
                }
            }

            if (m_values.Count != m_fingerConstraints.Count)
            {
                Debug.Log("Error getting access to Data value of the constraints. Will abort.");
                m_values = null;
            }
            m_firstTime = false;
        }

        if (m_values == null)
            return;

        int counter = 0;
        foreach (SofaConstraint constraint in m_fingerConstraints)
        {
            m_values[counter] += value;

            if (m_values[counter] < 0.0f)
                m_values[counter] = 0.0f;

            float[] val = new float[1];
            val[0] = m_values[counter];

            constraint.m_impl.SetVectordValue("value", 1, val);
        }
    }

}
