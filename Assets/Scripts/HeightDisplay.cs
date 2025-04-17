using UnityEngine;
using TMPro;

public class HeightDisplay : MonoBehaviour
{
    public Transform player;
    public TextMeshProUGUI heightText;

    void Update()
    {
        float height = player.position.y;
        heightText.text = "高さ: " + height.ToString("F1") + " m";
    }
}
