using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class random_speed : MonoBehaviour
{
    public Text text; 
    float current_speed = 37;
    float CD = 0;
    float target_speed = 37;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var random = new System.Random();
        text.text = ((int)current_speed).ToString() +"km/h";

        if(current_speed>target_speed+1){
            current_speed-=Time.deltaTime;
        }else if(current_speed<target_speed-1)
        {
            current_speed+=Time.deltaTime;
        }

        if(CD<=0){
            target_speed = random.Next(20,60);
            CD = 5;
        }else{
            CD -= Time.deltaTime;
        }
    }
}
