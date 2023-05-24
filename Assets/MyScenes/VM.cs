using System.Security.Cryptography.X509Certificates;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Tobii.Gaming;

public class VM : MonoBehaviour
{
    public AudioSource audioSource;

    const float TEMP_RESET_CD = 0.6f;
    const float TEMP_CONFIRM_GAP = 0.45f;
    const int DEGREE_MAX = 30;
    const int DEGREE_MIN = 22;
    Vector2 gaze_point; // 目视位置
    public List<GameObject> tar_obj_list; // 所有可以被注视的对象
    public List<GameObject> menu_btn_list; // 所有的菜单按钮
    public List<GameObject> degree_text_list; // 所有可以被注视的对象
    public List<GameObject> temp_btn_list; // 所有的温度按钮
    Dictionary<String,Vector2> obj_ori_position = new Dictionary<string, Vector2>(); // 所有可以被注视的对象的原本位置
    public Color focused_color;
    public Color unfocused_color;
    GameObject last_tar; // 上一次注视的物体
    GameObject current_tar = null;
    float gaze_process = 0; // 专注的时间
    int degree = 25;
    bool temp_anim_playing = false; // 是否正在播放温度按钮动画
    float temp_anim_process = 0;// 温度按钮动画进程
    float temp_reset_cd = TEMP_RESET_CD;// 温度按钮重置CD
    bool temp_focusing = false; // 是否注视着温度按钮
    bool temp_activated = false; // 是否温度按钮处于激活状态
    void Start()
    {
        int count = 0;
        foreach (var g in tar_obj_list)
        {
            // 对每个注视目标检查组成
            if(check_tar_obj(g)){
                // Debug.Log(count.ToString()+" ready "+g.name);
                obj_ori_position.Add(g.name,g.GetComponent<RectTransform>().anchoredPosition);
                on_gaze_exit(g);
            }else{
                Debug.LogError(count.ToString()+" failed "+g.name);
            }
            count ++;
        }

        enable_temp_btns(false);
    }

    void Update() {
        // 更新温度
        foreach (var d in degree_text_list)
        {
            d.GetComponent<Text>().text = degree.ToString()+"C°";
        }
        // 温度按钮
        if (temp_anim_playing){
            if(temp_anim_process < 1){
                temp_anim_process += 4 * Time.deltaTime;
                foreach (var g in temp_btn_list)
                {
                    g.SetActive(true);
                    g.GetComponent<RectTransform>().anchoredPosition = obj_ori_position[g.name] * temp_anim_process + obj_ori_position["temp"]*(1f-temp_anim_process);
                    g.GetComponent<RectTransform>().localScale = new Vector2(1f,1f)*temp_anim_process + new Vector2(0.2f,0.2f)*(1-temp_anim_process);
                }
            }else{
                enable_temp_btns(true);
                temp_anim_playing = false;
                temp_reset_cd = TEMP_RESET_CD;
                temp_anim_process = 0;
                temp_activated = true;
            }
        }

        if(temp_activated){
            if (!temp_focusing){
                if(temp_reset_cd > 0){
                    temp_reset_cd -= Time.deltaTime;
                }else if(!temp_btn_list.Contains(current_tar)){
                    enable_temp_btns(false);
                    enable_menu_btns(true);
                    print("exit");
                    temp_activated = false;
                }
            }
        }

    }

    void FixedUpdate()
    {
        // 刷新注视点
        gaze_point = TobiiAPI.GetGazePoint().Screen;
        
        current_tar = null;

        foreach (var g in tar_obj_list)
        {
            // 对每个注视目标接近或离开
            if(g.GetComponent<BoxCollider2D>().enabled && check_focusing_obj(gaze_point,g.GetComponent<RectTransform>().anchoredPosition+g.GetComponent<BoxCollider2D>().offset,g.GetComponent<BoxCollider2D>().size)){
                current_tar = g;
            }
        }

        if(last_tar != current_tar){
            gaze_process = 0;
            on_gaze_exit(last_tar);
            on_gaze_enter(current_tar);
            play_sound();
        }

        if(current_tar!=null && last_tar == current_tar){
            gaze_process += Time.fixedDeltaTime;
            on_gazing(current_tar);
        }

        last_tar = current_tar;

    }
    // 检查注视目标的规范
    bool check_tar_obj(GameObject g){
        if(g && g.GetComponent<BoxCollider2D>() && g.GetComponent<RectTransform>()){
            return true;
        }
        return false;
    }

    // 判断是否注视一个物体
    bool check_focusing_obj(Vector2 point,Vector2 center,Vector2 rect){
        var d = point-center;
        d.x = Mathf.Abs(d.x);
        d.y = Mathf.Abs(d.y);
        return d.x <= rect.x && d.y <= rect.y;
    }

    void on_gaze_enter(GameObject g){
        if(!g) return;
        // print("gaze enter "+g.name);
        switch(g.name){
            case "temp":
                g.GetComponent<Image>().color = focused_color;
                break;
            case "temp_down":
                // g.GetComponent<RectTransform>().localScale=new Vector3(1.1f,1.1f);
                temp_focusing = true;
                break;
            case "temp_dis":
                g.GetComponent<RectTransform>().localScale=new Vector3(1.1f,1.1f);
                temp_focusing = true;
                break;
            case "temp_up":
                // g.GetComponent<RectTransform>().localScale=new Vector3(1.1f,1.1f);
                temp_focusing = true;
                break;
            case "music":
                g.GetComponent<Image>().color = focused_color;
                break;
            case "call":
                g.GetComponent<Image>().color = focused_color;
                break;
            case "map":
                g.GetComponent<Image>().color = focused_color;
                break;
            case "option":
                g.GetComponent<Image>().color = focused_color;
                break;
            case "dash_board":
                g.GetComponent<Image>().color = focused_color;
                g.GetComponentInChildren<Text>().color = focused_color;
                break;
            default:
                break;
        }
    }

    void on_gaze_exit(GameObject g){
        if(!g) return;
        // print("gaze exit "+g.name);
        switch(g.name){
            case "temp":
                g.GetComponent<Image>().color = unfocused_color;
                g.GetComponent<RectTransform>().anchoredPosition = obj_ori_position[g.name];
                g.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
                break;
            case "temp_down":
                g.GetComponent<RectTransform>().localScale = new Vector3(1.0f,1.0f);
                temp_focusing = false;
                temp_reset_cd = TEMP_RESET_CD;
                break;
            case "temp_dis":
                g.GetComponent<RectTransform>().localScale = new Vector3(1.0f,1.0f);
                temp_focusing = false;
                temp_reset_cd = TEMP_RESET_CD;
                break;
            case "temp_up":
                g.GetComponent<RectTransform>().localScale = new Vector3(1.0f,1.0f);
                temp_focusing = false;
                temp_reset_cd = TEMP_RESET_CD;
                break;
            case "music":
                g.GetComponent<Image>().color = unfocused_color;
                break;
            case "call":
                g.GetComponent<Image>().color = unfocused_color;
                break;
            case "map":
                g.GetComponent<Image>().color = unfocused_color;
                break;
            case "option":
                g.GetComponent<Image>().color = unfocused_color;
                break;
            case "dash_board":
                g.GetComponent<Image>().color = unfocused_color;
                g.GetComponentInChildren<Text>().color = unfocused_color;
                break;
            default:
                break;
        }
    }

    void on_gazing(GameObject g){
        if(!g) return;
        // print("gazing " + g.name);
        switch(g.name){
            case "temp":
                if (gaze_process >= 1f){
                    gaze_process = 0;
                    temp_anim_playing = true;
                    enable_menu_btns(false);
                }else{
                    twitch_obj(g, gaze_process);    
                }
                break;
            
            case "temp_down":
                g.GetComponent<RectTransform>().localScale = new Vector3(1.0f,1.0f)*(gaze_process/TEMP_CONFIRM_GAP*0.1f+1);
                if(gaze_process > TEMP_CONFIRM_GAP){
                    if(degree>DEGREE_MIN){
                        gaze_process = 0;
                        degree -= 1;
                        play_sound();
                    }else{
                        gaze_process = TEMP_CONFIRM_GAP;
                    }
                }
                break;
            case "temp_up":
                g.GetComponent<RectTransform>().localScale = new Vector3(1.0f,1.0f)*(gaze_process/TEMP_CONFIRM_GAP*0.1f+1);
                if(gaze_process > TEMP_CONFIRM_GAP){
                    if(degree < DEGREE_MAX){
                        gaze_process = 0;
                        degree += 1;
                        play_sound();
                    }else{
                        gaze_process = TEMP_CONFIRM_GAP;
                    }
                }
                break;
            case "music":
                if (gaze_process >= 1f){
                    gaze_process = 0;
                }else{
                    twitch_obj(g, gaze_process);    
                }
                break;
            case "call":
                if (gaze_process >= 1f){
                    gaze_process = 0;
                }else{
                    twitch_obj(g, gaze_process);    
                }
                break;
            case "map":
                if (gaze_process >= 1f){
                    gaze_process = 0;
                }else{
                    twitch_obj(g, gaze_process);    
                }
                break;
            case "option":
                if (gaze_process >= 1f){
                    gaze_process = 0;
                }else{
                    twitch_obj(g, gaze_process);    
                }
                break;
            case "dash_board":
                break;
            default:
                break;
        }
    }

    void enable_temp_btns(bool b){
        // print("enable_temp_btns");
        foreach (var g in temp_btn_list)
        {
            g.GetComponent<BoxCollider2D>().enabled = b;
            g.SetActive(b);
        }
    }

    void enable_menu_btns(bool b){
        foreach (var g in menu_btn_list)
        {
            g.GetComponent<BoxCollider2D>().enabled = b;
            g.SetActive(b);
        }
    }

    void twitch_obj(GameObject g,float t){
        g.GetComponent<RectTransform>().anchoredPosition = obj_ori_position[g.name] + twitching_position(t);
        g.GetComponent<RectTransform>().localScale = twitching_scale(t);
        g.GetComponent<RectTransform>().rotation = Quaternion.Euler(0,0,twitching_angle(t));
    }

    Vector2 twitching_position(float t){
        Vector2 d = new Vector2(1.5f*Mathf.Sin(51*t*(t+3)),1.5f*Mathf.Cos(49*t*t));
        return d;
    }

    Vector2 twitching_scale(float t){
        Vector2 d = new Vector3(1-0.3f*t*t,1-0.3f*t*t);
        return d;
    }

    float twitching_angle(float t){
        return 5*Mathf.Sin(60*t*t);
    }

    void play_sound(){
        audioSource.Play();
    }
}
