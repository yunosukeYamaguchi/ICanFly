using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    // 遷移先のシーン名（またはBuild Index）
    public string nextSceneName = "NextScene"; // 任意のシーン名に変更

    void Update()
    {
        // 何かのキーが押されたら
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
