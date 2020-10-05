using System.Collections;
using UnityEngine;

public class BugAnimation : MonoBehaviour
{
    private Animator _animator;
    private PlayerDash _dash;
    private PlayerMovement _playerMovement;

    [SerializeField] private RuntimeAnimatorController _helmetBug, _noHelmetBug;
    [SerializeField] private Transform _helmetGraphics, _noHelmetGraphics;

    private int _dashingHash, _stoppedHash;

    private void Awake()
    {
        GameManager.Instance.OnLevelStarted += SetGraphicsInATinyBit;

        _dashingHash = Animator.StringToHash("Dashing");
        _stoppedHash = Animator.StringToHash("Stopped");
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnLevelStarted -= SetGraphicsInATinyBit;
    }

    private void Start()
    {
        _animator = GetComponentInParent<Animator>();
        _dash = GetComponentInParent<PlayerDash>();
        _playerMovement = GetComponentInParent<PlayerMovement>();
        //SetGraphics();
    }

    private void Update()
    {
        _animator.SetBool(_dashingHash, _dash.IsDashing());
        _animator.SetBool(_stoppedHash, _playerMovement.GetCanMove());
    }

    private void SetGraphicsInATinyBit() //Since the input has to be swapped first in order for it to work
    {
        //StartCoroutine(SetGraphicsDelayed(0.01f));
    }

    private IEnumerator SetGraphicsDelayed(float delayInSeconds)
    {
        yield return new WaitForSeconds(delayInSeconds);
        SetGraphics();
    }

    private void SetGraphics()
    {
        // No idea how this is gonna work with caterpillar which has like 1000 children but uh yea
        if (transform.childCount > 0) Destroy(transform.GetChild(0).gameObject); //https://youtu.be/ZjOAwBtRD54

        if (GetComponentInParent<PlayerInput>() is ManualPlayerInput)
        {
            Instantiate(_helmetGraphics, transform);
            _animator.runtimeAnimatorController = _helmetBug;
        }
        else
        {
            Instantiate(_noHelmetGraphics, transform);
            _animator.runtimeAnimatorController = _noHelmetBug;
        }
    }
}
