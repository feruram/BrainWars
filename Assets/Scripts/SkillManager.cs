using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public Camera vrCamera;
    public Weapons weapons;
    public float rayDistance = 2f;
    public float requiredGazeTime = 10.0f;
    public bool downwardGazeMode = false;
    private ControllerInputManager input;

    private Timer timer;
    private GameObject currentTarget;
    private GameObject hitObject;
    private BCIManager bciManager;
    private PlayerPoser poser;


    [SerializeField] private float nowGazeTime = 0;
    public string currentStimulus = "";

    void Start()
    {
        timer = new Timer(requiredGazeTime);
        bciManager = GameObject.Find("BCIManager").GetComponent<BCIManager>();
        input=GameObject.FindGameObjectWithTag("Input").GetComponent<ControllerInputManager>();
    }

    void Update()
    {
        //
        if(input.B){
            Mi();
        }
        //
        Vector3 gazeDirection = vrCamera.transform.forward;
        if (downwardGazeMode)
        {
            gazeDirection = Quaternion.Euler(15f, 0f, 0f) * gazeDirection;
        }

        Ray ray = new Ray(vrCamera.transform.position, gazeDirection);
        RaycastHit hit;
        bool isSameTarget = false;

        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            hitObject = hit.collider.gameObject;

            // 注視対象に対する処理
            if (System.Enum.TryParse(hitObject.name, out TargetName targetEnum))
            {
                if (hitObject == currentTarget)
                {
                    isSameTarget = true;
                }
                else
                {
                    if (currentTarget != null)
                    {
                        Debug.Log(currentTarget.name);
                        Debug.Log(hitObject.name);
                        Debug.Log("out");
                    }
                    currentTarget = hitObject;
                    timer.Reset();
                }

                nowGazeTime = timer.UpdateTimer(isSameTarget, Time.deltaTime);

                if (!string.IsNullOrEmpty(currentStimulus))
                {
                    if (System.Enum.TryParse(currentStimulus, out TargetName parsedTarget))
                    {
                        ExecuteFireFunction(parsedTarget); 
                    }
                    else
                    {
                        Debug.LogWarning($"Unknown stimulus: {currentStimulus}");
                    }
                    timer.Reset();
                    currentStimulus = "";
                }
                else if (timer.IsTimeReached())
                {
                    ExecuteFireFunction(targetEnum);       // 注視時間で発動
                    timer.Reset();
                }
            }
            else
            {
                if (currentTarget != null)
                {
                    Debug.Log(currentTarget.name);
                    Debug.Log(hitObject.name);
                    Debug.Log("out");
                }
                currentTarget = null;
                timer.Reset();
            }
        }
    }

    // 外部から刺激結果を受け取る
    public void SetStimulus(string stimulusName)
    {
        currentStimulus = stimulusName;
    }

    bool IsExternalInputActive(TargetName name)
    {
        return currentStimulus == name.ToString();
    }

    void ExecuteFireFunction(TargetName name)
    {
        switch (name)
        {
            case TargetName.ONE:
                FireONE();
                break;
            case TargetName.TWO:
                FireTWO();
                break;
            case TargetName.THREE:
                FireTHREE();
                break;
        }
    }
    void FireONE() => weapons.Equip(1);
    void FireTWO() => weapons.Equip(2);
    void FireTHREE() => weapons.Equip(3);
    void Mi()=> weapons.Burst();
}

public enum TargetName
{
    ONE,
    TWO,
    THREE
}
