using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonsController : MonoBehaviour
{
    public void PauseButton()
    {
        GameManager.Instance.OpenPauseMenu(false);
    }

    public void ResumeButton()
    {
        GameManager.Instance.ClosePauseMenu();
    }

    public void RestartButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
