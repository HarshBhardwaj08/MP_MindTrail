using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PanelMager : MonoBehaviour
{
    public List<GameObject> storyPanellist;
    public int currentPannelNum;
    private void Start()
    {
        storyPanellist[currentPannelNum].SetActive(true);
    }
    public void IncremenetPanel()
    { 
        currentPannelNum++;
        for (int i = 0; i < storyPanellist.Count; i++)
        {
            if(i==currentPannelNum)
            {
                storyPanellist[currentPannelNum].SetActive(true);
            }
            else
            {
                storyPanellist[i].SetActive(false);
            }
        }
       if(currentPannelNum >= storyPanellist.Count)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
        }
    }
    public void DecremenetPanel()
    {
        currentPannelNum--;
        for (int i = 0; i < storyPanellist.Count; i++)
        {
            if (i == currentPannelNum)
            {
                storyPanellist[currentPannelNum].SetActive(true);
            }
            else
            {
                storyPanellist[i].SetActive(false);
            }
        }
    }
}
