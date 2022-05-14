using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private Image blackScreen;

    public void NewGame()
    {
        // Inventory.ClearSave();
        blackScreen.DOFade(1f, 0.5f).OnComplete( () => SceneManager.LoadScene("MapScene"));
    }

    public void LoadGame()
    {
        Inventory.LoadGame();
        blackScreen.DOFade(1f, 0.5f).OnComplete( () => SceneManager.LoadScene("MapScene"));
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game!!");
        Application.Quit(); // Doesn't work in Unity Editor
    }
}
