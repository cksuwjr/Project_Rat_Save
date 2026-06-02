using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    public void OnPlayButton ()
    {
        SceneManager.LoadScene("stage1_start");
    }

}