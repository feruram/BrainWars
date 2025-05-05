using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveModel : MonoBehaviour
{

    public void Save()
    {
        ModelManager.SaveModel();
    }
    public void Load()
    {
        ModelManager.LoadModel();
    }
}
