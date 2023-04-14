using UnityEngine;
using Tobii.Gaming;
using System.Collections.Generic;

public class VIISManager : MonoBehaviour
{
    public GameObject dash_board;
    public float dash_board_max_h = -3.5f;
    public float dash_board_min_h = -5.5f;
    public float dash_board_speed = 10f;
    public GameObject up_panel;
    public float up_panel_max_h = 7.5f;
    public float up_panel_min_h = 5.5f;
    public float up_panel_speed = 10f;
    public GameObject b1;
    public GameObject b2;
    public GameObject b3;
    public GameObject b4;
    public GameObject b5;
    public Color b_color_ori;
    public Color b_color_selected;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GameObject focusedObject = TobiiAPI.GetFocusedObject();
        // dash_board
        var p = dash_board.transform.position;
        if (focusedObject == dash_board){
            p.y += Time.deltaTime*dash_board_speed;
            
        }else{
            p.y -= Time.deltaTime*dash_board_speed;
        }
        if (p.y > dash_board_max_h){
            p.y = dash_board_max_h;
        }else if(p.y < dash_board_min_h){
            p.y = dash_board_min_h;
        }
        dash_board.transform.position = p;

        // up_panel
        p = up_panel.transform.position;
        if(focusedObject == up_panel || focusedObject == b1|| focusedObject == b2|| focusedObject == b3|| focusedObject == b4|| focusedObject == b5){
            p.y -= Time.deltaTime*up_panel_speed;
            
        }else{
            p.y += Time.deltaTime*up_panel_speed;
        }
        if (p.y >up_panel_max_h){
            p.y = up_panel_max_h;
        }else if(p.y<up_panel_min_h){
            p.y = up_panel_min_h;
        }
        up_panel.transform.position = p;
        
        if(focusedObject == b1){
            focusedObject.GetComponent<Renderer>().material.color = b_color_selected;
        }else{
            b1.GetComponent<Renderer>().material.color=b_color_ori;
        }
        if(focusedObject == b2){
            focusedObject.GetComponent<Renderer>().material.color = b_color_selected;
        }else{
            b2.GetComponent<Renderer>().material.color=b_color_ori;
        }
        if(focusedObject == b3){
            focusedObject.GetComponent<Renderer>().material.color = b_color_selected;
        }else{
            b3.GetComponent<Renderer>().material.color=b_color_ori;
        }
        if(focusedObject == b4){
            focusedObject.GetComponent<Renderer>().material.color = b_color_selected;
        }else{
            b4.GetComponent<Renderer>().material.color=b_color_ori;
        }
        if(focusedObject == b5){
            focusedObject.GetComponent<Renderer>().material.color = b_color_selected;
        }else{
            b5.GetComponent<Renderer>().material.color=b_color_ori;
        }
    }
}
