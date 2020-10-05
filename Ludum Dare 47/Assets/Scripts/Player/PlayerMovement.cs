using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    public Action OnDash;

    [SerializeField] private BugType _bugType;
    [SerializeField] private float _movementSpeed = 2f;
    [SerializeField] private float _turningSpeed = 90f;

    private Rigidbody2D _rigidbody;
    private PlayerInput _input;
    private PlayerDash _playerDash;
    private bool _canMove = true;

    // Since we want to listen to the events even if the ghost is inactive,
    // we subscribe to events in Awake and OnDestroy instead of OnEnable and OnDisable
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _input = GetComponent<PlayerInput>();
        _playerDash = GetComponent<PlayerDash>();

        GameManager.Instance.OnLevelStarted += ResetPosition;
        GameManager.Instance.OnLevelStarted += Reactivate;
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnLevelStarted -= Reactivate;
        GameManager.Instance.OnLevelStarted -= ResetPosition;
    }

    private void Reactivate()
    {
        gameObject.SetActive(true);
        foreach (var p in GetComponentsInChildren<SpriteRenderer>())
        {
            p.color = new Color(255, 255, 255);
        }
        _canMove = true;
    }

    public bool GetCanMove()
    {
        return _canMove;
    }

    private void Update()
    {
        if (!_canMove) { return; } 

        _rigidbody.velocity = transform.right * GetSpeed();
        _rigidbody.angularVelocity = -GetCurrentRotationDelta();

        if (_input.GetDash()) OnDash?.Invoke();
    }

    public void SetNewInput(PlayerInput playerInput)
    {
        _input = playerInput;
    }

    private float GetTurningSpeed()
    {
        return _turningSpeed;
    }

    private float GetCurrentRotationDelta()
    {
        float delta = 0;

        if (_input.GetLeft())
        {
            delta -= GetTurningSpeed();
        }

        if (_input.GetRight())
        {
            delta += GetTurningSpeed();
        }

        return delta;
    }

    private void ResetPosition()
    {
        _canMove = true;
        if (_input is ManualPlayerInput) return;
        transform.position = _input.GetStartPosition();
        transform.rotation = Quaternion.Euler(0,0,0);
    }

    private float GetSpeed()
    {
        return _movementSpeed;
        //add thing for different insects here
    }

    private void Die()
    {
        _canMove = false;

        _rigidbody.angularVelocity = 0f;
        _rigidbody.velocity = Vector2.zero;

        if (_playerDash != null && _playerDash.IsDashing())
        {
            _playerDash.StopDashingImmediately();
        }

        foreach(var p in GetComponentsInChildren<SpriteRenderer>())
        {
            p.color = new Color(.15f, .15f, .15f);
        }

        if (_input is ManualPlayerInput)
        {
            GameManager.Instance.EndGame();
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("DashBox"))
        {
            Die();
        }

        if (!collider.CompareTag("Finish")) { return; }

        if (!(_input is ManualPlayerInput manualInput))
        {
            _canMove = false;
            return;
        }

        ResetPosition();

        var autoPlayerInput = gameObject.AddComponent<AutomaticPlayerInput>();
        var replayInputs = manualInput.GetReplayInputs();

        autoPlayerInput.SetInputs(replayInputs);

        SetNewInput(autoPlayerInput);

        Destroy(manualInput);

        // Hard code is good code
        // Also this needs to happen on all the layers of the children (e.g for caterpillar)
        gameObject.layer = 9;

        GameManager.Instance.EndLevel(replayInputs);
    }
}
