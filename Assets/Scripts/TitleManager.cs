using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    // �J�ڐ�̃V�[�����i�܂���Build Index�j
    public string nextSceneName = "NextScene"; // �C�ӂ̃V�[�����ɕύX

    void Update()
    {
        // �����̃L�[�������ꂽ��
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
