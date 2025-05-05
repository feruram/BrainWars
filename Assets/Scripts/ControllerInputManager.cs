using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.TestTools;
using Valve.VR;
public class ControllerInputManager : MonoBehaviour
{
    private SteamVR_Action_Boolean onTrigger = SteamVR_Actions._default.InteractUI;
    private SteamVR_Action_Vector2 joymove=SteamVR_Actions._default.Move;
    private SteamVR_Action_Boolean buttonX=SteamVR_Actions._default.GrabPinch;
    private SteamVR_Action_Vibration hapticTrigger = SteamVR_Actions._default.Haptic;
    private SteamVR_Input_Sources hands;
    private SteamVR_Action_Boolean AButton=SteamVR_Actions._default.A;
    private SteamVR_Action_Boolean BButton=SteamVR_Actions._default.B;
    [SerializeField]private bool hintOn=false;
    public bool leftTrigger;
    public bool rightTrigger;
    public bool trigger;
    public bool up;
    public bool right;
    public bool left;
    public bool down;
    public bool A;
    public bool B;
    public Vector2 move;
    public bool setTracker_r;
    public bool setTracker_l;
    public bool setTracker_n;
    public bool e;
    public bool q;
    public bool isSimulatorOn=false;
    public bool isOnlyMe=false;
    public int buttonCounter=0;
    public bool rightAsyncSimu=false;
    public bool leftAsyncSimu=false;
    public bool space=false;
    public bool back=false;
    public bool calibration_trigger;
    public bool calibration_space;
    public bool calibration_delete;
    public bool calibration_up;
    public bool calibration_down;

    private void Update()
    {
        leftTrigger=onTrigger.GetStateDown(SteamVR_Input_Sources.LeftHand);
        rightTrigger=onTrigger.GetStateDown(SteamVR_Input_Sources.RightHand);
        A=AButton.GetStateDown(SteamVR_Input_Sources.RightHand);
        B=BButton.GetStateDown(SteamVR_Input_Sources.RightHand);
        move=joymove.GetAxis(SteamVR_Input_Sources.LeftHand);
        if(leftTrigger||rightTrigger){
            trigger=true;
        }else{
            trigger=false;
        }
        space=Input.GetKeyDown(KeyCode.Space);
        back=Input.GetKeyDown(KeyCode.Delete);
        if(Input.GetKeyDown(KeyCode.O)){
            if(isOnlyMe){
                isOnlyMe=false;
            }else{
                isOnlyMe=true;
            }
        }
        if(Input.GetKeyDown(KeyCode.Return)){
            if(isSimulatorOn){
                isSimulatorOn=false;
            }else{
                isSimulatorOn=true;
            }
        }

            setTracker_r=Input.GetKeyDown(KeyCode.RightArrow);
            setTracker_l=Input.GetKeyDown(KeyCode.LeftArrow);
            setTracker_n=Input.GetKeyDown(KeyCode.DownArrow);
            up=Input.GetKey(KeyCode.W);
            right=Input.GetKey(KeyCode.D);
            left=Input.GetKey(KeyCode.A);
            down=Input.GetKey(KeyCode.S);
            e=Input.GetKey(KeyCode.E);
            q=Input.GetKey(KeyCode.Q);
    }
            
}