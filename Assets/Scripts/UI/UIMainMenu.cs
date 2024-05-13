using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMainMenu : MonoBehaviour
{
    [SerializeField] private GameObject instructionPage;

    public void LoadInstructionPage()
    {
        instructionPage.SetActive(true);
    }

    public void BackToMainMenu()
    {
        instructionPage.SetActive(false);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
