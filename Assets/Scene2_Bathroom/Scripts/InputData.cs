using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;
using System.Collections.Generic;
using System.Collections;
public class InputData : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public InputDevice leftController; // Reference to left controller device
    public InputDevice rightController; // Reference to right controller device
    public InputDevice HMD;

    void Update()
    {

        if (!leftController.isValid || !rightController.isValid) { InitializeInputDevices(); } // If controllers are not valid, exit


    }
    
    private void InitializeInputDevices()
    {
        if (!rightController.isValid) {
            InitializeInputDevice(InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Right, ref rightController);
        }
        if (!leftController.isValid) {
            InitializeInputDevice(InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Left, ref leftController);
        }
        if (!HMD.isValid) {
            InitializeInputDevice(InputDeviceCharacteristics.HeadMounted, ref HMD);
        }
    }

    private void InitializeInputDevice(InputDeviceCharacteristics characteristics, ref InputDevice device)
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(characteristics, devices);
        if (devices.Count > 0)
        {
            device = devices[0];
        }
    }
}
