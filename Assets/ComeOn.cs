using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.Gaming;

public class ComeOn : MonoBehaviour
{
    public GameObject welcome;
    public GameObject text;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!TobiiAPI.GetGazePoint().IsRecent())
        {
            welcome.SetActive(true);
        }
        else
        {
            welcome.SetActive(false);
        }
        var p = text.GetComponent<Transform>().position;
        p.y = 0.5f * Mathf.Sin(4 * Time.realtimeSinceStartup);
        text.GetComponent<Transform>().position = p;
    }
}
