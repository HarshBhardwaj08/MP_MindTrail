using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public GameObject PausePanel;
    public GameObject HelpPanel;
    public GameObject PauseButton;
    public void EnablePausePanel()
    {
        Time.timeScale = 0;
        PausePanel.SetActive(true);
        PauseButton.SetActive(false);
    }

    public void DisablePausePanel()
    {
        Time.timeScale = 1;
        PausePanel.SetActive(false);
        PauseButton.SetActive(true);
    }
    public void EnableHelpPanel()
    {
        Time.timeScale = 0;
        HelpPanel.SetActive(true);
    }
    public void DisableHelpPanel()
    {
        Time.timeScale = 1;
        HelpPanel.SetActive(false);
        EnablePausePanel();
    }
}
