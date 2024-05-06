using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    // singleton pattern -> similar to module pattern in js 
    public static GameManager instance;

    public int MaxNumberOfShots = 3;

    private int _usedNumberOfShots;


    private void Awake()
    {
        if(instance == null)
        {
            instance = this; 
        }
    }
    public void UseShot()
    {
        _usedNumberOfShots++;
    }

    public bool HasEnoughShots()
    {
        if(_usedNumberOfShots < MaxNumberOfShots)
        {
            return true;
        }
        return false;
    }

}
