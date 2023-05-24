using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using AOT;
using Tobii.EyeX.Client;
using Tobii.EyeX.Framework;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class my_cb : MonoBehaviour
{
    private static Context _context;
    private static Context Context
        {
            get
            {
                if (_context == null)
                {
                    Tobii.EyeX.Client.Environment.Initialize(LogTarget.Trace);
                    _context = new Tobii.EyeX.Client.Context(true);
                }

                return _context;
            }
        }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void calibration(){
        Task.Run(() =>
            {
                Context.LaunchConfigurationTool(ConfigurationTool.GuestCalibration, CompletionHandler);
            });
    }

    private void CompletionHandler(AsyncData asyncdata)
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
            {
                print("!");
                calibration();
            }
    }
}
