using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazePointCollision : MonoBehaviour
{
    BoxCollider c2d;
    // Start is called before the first frame update
    void Start()
    {
        c2d = GetComponent<BoxCollider>();
        print(c2d);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other) {
        print(other);    
    }
}
