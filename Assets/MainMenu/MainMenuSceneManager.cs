using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuSceneManager : MonoBehaviour
{
    public void openSceneCoin()
    {
        SceneManager.LoadScene("CoinScene");
    }

    public void openSceneCar()
    {
        SceneManager.LoadScene("CarScene");
    }

    public void openHumanoidScene()
    {
        SceneManager.LoadScene("HumanoidScene");
    }
}
