using UnityEngine;
using Tobii.Gaming;

public class dash_board : MonoBehaviour
{
    public float min_h = -5.5f;
    public float max_h = -3.5f;
    public float rise_speed = 2f;

    private GazeAware _gazeAware;
    protected void Start()
    {
        _gazeAware = GetComponent<GazeAware>();
    }
    void Update()
    {
        var p = transform.position;
        if(_gazeAware.HasGazeFocus){
            p.y += Time.deltaTime*rise_speed;
        }else{
            p.y -= Time.deltaTime*rise_speed;
        }
        if (p.y>max_h){
            p.y=max_h;
        }else if(p.y<min_h){
            p.y=min_h;
        }

        transform.position = p;
    }
}