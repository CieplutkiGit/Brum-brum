using UnityEngine;
using UnityEngine.SceneManagement;
public class Manager : MonoBehaviour
{
    public GameObject TutPanel;
    private void Start()
    {
        if (PlayerPrefs.HasKey("TutorialDone"))
        {
            //do nothing
        }
        else
        {
            TutPanel.SetActive(true);
        }
    }
    public void GotIt()
    {
        PlayerPrefs.SetString("TutorialDone", "TutorialDone");
    }
    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void Stop()
    {
        Time.timeScale = 0;
    }
    public void Resume()
    {
        Time.timeScale = 1;
    }
    public void Quit()
    {
        Application.Quit();
    }
}
