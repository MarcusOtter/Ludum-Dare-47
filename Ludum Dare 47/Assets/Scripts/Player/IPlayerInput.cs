using UnityEngine;

public abstract class PlayerInput : MonoBehaviour
{
    public abstract bool GetLeft();
    public abstract bool GetRight();
    public abstract bool GetDash();
}
