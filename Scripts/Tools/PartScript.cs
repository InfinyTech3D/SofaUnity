using UnityEngine;
using System.Collections;

/// <summary>
/// Small component script that will update a particule script spray area
/// </summary>
public class PartScript : MonoBehaviour
{

    public float engineRevs;
    public float exhaustRate;

    ParticleSystem exhaust;

    // Use this for initialization
    void Start()
    {
        exhaust = GetComponent<ParticleSystem>();
    }


    // Update is called once per frame
    void Update()
    {
        exhaust.emissionRate = engineRevs * exhaustRate;
    }

}