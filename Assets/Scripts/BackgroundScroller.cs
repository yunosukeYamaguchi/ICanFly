using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    public Transform player;
    public float yOffset = 0f;

    void Update()
    {
        transform.position = new Vector3(0, player.position.y + yOffset, 10);
    }
}
