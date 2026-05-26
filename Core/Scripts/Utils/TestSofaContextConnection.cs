using UnityEngine;
using SofaUnityAPI;

public class TestSofaContextConnection : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SofaConnexionTestAPI test = new SofaConnexionTestAPI();
        test.testConnexion();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
