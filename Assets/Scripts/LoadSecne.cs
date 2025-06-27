using UnityEngine;
using UnityEngine.SceneManagement; // ← シーン遷移に必要

public class LoadScene : MonoBehaviour
{
    public string nextScene;
    
    public void ToNextScene()
    {
        SceneManager.LoadScene(nextScene);
    }
}