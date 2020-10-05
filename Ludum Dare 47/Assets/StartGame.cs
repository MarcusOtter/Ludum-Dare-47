using UnityEngine;

public class StartGame : MonoBehaviour
{
    private async void Start()
    {
        await GameManager.Instance.StartMainLoop();
    }
}
