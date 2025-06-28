using UnityEngine;
using UnityEngine.SceneManagement;

public class ToNextScene : MonoBehaviour
{
    // �J�ڐ�̃V�[�����i�܂���Build Index�j
    public string nextSceneName = "NextScene"; // �C�ӂ̃V�[�����ɕύX

    void Update()
    {
        // �����̃L�[�������ꂽ��
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
