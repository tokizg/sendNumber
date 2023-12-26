using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class addForce : MonoBehaviour
{
    [SerializeField]
    Rigidbody2D rb;
    [SerializeField]
    Vector2 vec;
    [SerializeField]
    float r =1f;
    // Start is called before the first frame update
    void Start()
    {
        rb.AddForce(vec*Random.Range(1f,r));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
