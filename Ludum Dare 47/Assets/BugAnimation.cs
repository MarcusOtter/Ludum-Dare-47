//using UnityEditor.Animations;
using UnityEngine;

public class BugAnimation : MonoBehaviour
{
    private Animator _animator;
    private PlayerDash _dash;
    private PlayerMovement _playerMovement;

    //Commenting cuz it's gicing me errors when I build and I need to test yo

    //[SerializeField] private AnimatorController _helmetBug, _noHelmetBug;
    //[SerializeField] private Transform _helmetGraphics, _noHelmetGraphics;

    //private void Awake()
    //{
    //    GameManager.Instance.OnLevelStarted += SetGraphicsInATinyBit;
    //}

    //private void Start()
    //{
    //    _animator = GetComponentInParent<Animator>();
    //    _dash = GetComponentInParent<PlayerDash>();
    //    _playerMovement = GetComponentInParent<PlayerMovement>();
    //    SetGraphics();
    //}

    //private void Update()
    //{
    //    _animator.SetBool("Dashing", _dash.IsDashing());
    //    _animator.SetBool("Stopped", _playerMovement.GetCanMove());
    //}

    //private void OnDestroy()
    //{
    //    GameManager.Instance.OnLevelStarted -= SetGraphicsInATinyBit;
    //}

    //private void SetGraphicsInATinyBit() //Since the input has to be swapped first in order for it to work
    //{
    //    Invoke("SetGraphics", 0.001f); //A new low, but it works
    //}

    //private void SetGraphics()
    //{
    //    if (transform.childCount > 0) Destroy(transform.GetChild(0).gameObject); //https://youtu.be/ZjOAwBtRD54
    //    if (GetComponentInParent<PlayerInput>() is ManualPlayerInput)
    //    {
    //        Instantiate(_helmetGraphics, transform);
    //        _animator.runtimeAnimatorController = _helmetBug;
    //    }
    //    else
    //    {
    //        Instantiate(_noHelmetGraphics, transform);
    //        _animator.runtimeAnimatorController = _noHelmetBug;
    //    }
    //}
}
