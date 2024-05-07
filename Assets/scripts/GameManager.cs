using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

//for reloading the scene
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    // singleton pattern -> similar to module pattern in js 
    public static GameManager instance;

    public int MaxNumberOfShots = 3;
    private int _usedNumberOfShots;
    [SerializeField] private float _maxWaitTime = 4f;
    [SerializeField] private GameObject _restartSceneObject;
    [SerializeField] private SlingShotHandler _slingShotHandler; 

    // like importing a module
    [SerializeField] private IconHandler _iconHandler;

    private List<Piggy> _piggies = new List<Piggy>();  


    private void Awake()
    {
        if(instance == null)
        {
            instance = this; 
        }

        // basically finds game objects with the component/script Piggy.cs (We are connecting the number of piggy to the gameManager)
        Piggy[] piggies = FindObjectsOfType<Piggy>();
        for (int i = 0; i < piggies.Length; i++)
        {
            // copying the array into the list
            _piggies.Add(piggies[i]);
        }
    }
    public void UseShot()
    {
        _usedNumberOfShots++;
        _iconHandler.UseShot(_usedNumberOfShots);

        CheckForLastShot();
    }

    public bool HasEnoughShots()
    {
        if(_usedNumberOfShots < MaxNumberOfShots)
        {
            return true;
        }
        return false;
    }

    // checking if all the piggies are death after the use of 3 angry birds
    public void CheckForLastShot()
    {
        if(_usedNumberOfShots == MaxNumberOfShots)
        {
            StartCoroutine(CheckAfterWaitTime());  
        }
    }

    private IEnumerator CheckAfterWaitTime() {
        yield return new WaitForSeconds(_maxWaitTime);

        if(_piggies.Count == 0)
        {
            WinGame();
        }
        else
        {
            RestartGame(); 
        }
    }

    // this method is from Piggy.cs 
    public void RemovePiggy(Piggy piggy)
    {
        _piggies.Remove(piggy);
        CheckForAllDeathPiggies();
    }
    
    // if all the piggies are death before the use of 3 angry bird
    private void CheckForAllDeathPiggies()
    {
        if(_piggies.Count == 0)
        {
            WinGame();
        }
    }


    #region Win/Lose

    private void WinGame()
    {
        // activating the game objects
        _restartSceneObject.SetActive(true);
        // turning off the script 
        _slingShotHandler.enabled = false;
    }

    // this function is attached to the restart image in the game
    public void RestartGame()
    {
        // reloads the scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
        

    #endregion  
}

