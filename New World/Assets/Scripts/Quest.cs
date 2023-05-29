using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "Quest")]
public class Quest : ScriptableObject
{
    public string title;
    public string objectives;
    public string rewards;
    public bool isCompleted;

    public void Complete()
    {
        isCompleted = true;
    }
}
