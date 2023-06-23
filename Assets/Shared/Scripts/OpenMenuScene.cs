using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenMenuScene : MonoBehaviour
{
    public void openMainMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
