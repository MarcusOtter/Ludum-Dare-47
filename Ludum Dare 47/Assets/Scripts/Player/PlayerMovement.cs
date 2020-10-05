using UnityEngine;
using System;
using System.Collections;

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
    private Transform _spawnPoint;

    private Action<int> _onRewindBeginAction;

    private bool _canMove;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _input = GetComponent<PlayerInput>();
        _playerDash = GetComponent<PlayerDash>();
    }


    private void OnEnable()
    {
        _onRewindBeginAction = rewindDuration => StartCoroutine(RewindToSpawnPoint(rewindDuration));

        GameManager.Instance.OnLevelStart += StartLevel;
        GameManager.Instance.OnRewindBegin += _onRewindBeginAction;
        GameManager.Instance.OnRewindEnd += UpdateInputMode;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnLevelStart -= StartLevel;
        GameManager.Instance.OnRewindBegin -= _onRewindBeginAction;
        GameManager.Instance.OnRewindEnd -= UpdateInputMode;
    }

    private void Update()
    {
        if (!_canMove) { return; }

        _rigidbody.velocity = transform.right * _movementSpeed;
        _rigidbody.angularVelocity = -GetCurrentRotationDelta();
        if (_input.GetDash()) { OnDash?.Invoke(); }
    }

    public BugType GetBugType()
    {
        return _bugType;
    }

    public bool GetCanMove()
    {
        return _canMove;
    }


    public void SetSpawnPoint(Transform spawnPoint)
    {
        if (_spawnPoint != null)
        {
            Debug.LogError("Why are we trying to set spawn point twice for a player??");
            return;
        }

        _spawnPoint = spawnPoint;
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

    private void StartLevel()
    {
        _canMove = true;
        gameObject.SetActive(true);
    }

    private void UpdateInputMode()
    {
        if (!(_input is ManualPlayerInput manualInput)) { return; }

        var playerInputs = manualInput.GetPlayerInputs();
        Destroy(manualInput);

        var autoPlayerInput = gameObject.AddComponent<AutomaticPlayerInput>();
        autoPlayerInput.SetInputs(playerInputs);
        _input = autoPlayerInput;

        var allChildren = GetComponentsInChildren<Transform>();
        foreach (var child in allChildren)
        {
            // Hard code is good code
            child.gameObject.layer = 9;
        }
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
            GameManager.Instance.TriggerGameOver();
        }
    }

    private async void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("DashBox"))
        {
            Die();
        }

        if (!collider.CompareTag("Finish")) { return; }

        foreach (var col in GetComponentsInChildren<Collider2D>())
        {
            col.enabled = false;
        }

        _canMove = false;
        _rigidbody.angularVelocity = 0f;
        _rigidbody.velocity *= 0.5f;

        if (_input is AutomaticPlayerInput) { return; }
        await GameManager.Instance.TriggerLevelFinish();
    }

    private IEnumerator RewindToSpawnPoint(int rewindDurationInMs)
    {
        var cachedTransform = transform;

        _canMove = false;
        _rigidbody.velocity = Vector2.zero;
        _rigidbody.angularVelocity = 0f;

        foreach (var p in GetComponentsInChildren<SpriteRenderer>())
        {
            p.color = new Color(255, 255, 255);
        }

        var startPosition = (Vector2) cachedTransform.position;
        var startRotation = cachedTransform.rotation;
        var rewindDurationInSeconds = rewindDurationInMs / 1000f;
        var timer = 0f;

        while (timer < rewindDurationInSeconds)
        {
            timer += 0.02f;
            var progress = timer / rewindDurationInSeconds;
            cachedTransform.position = Vector2.Lerp(startPosition, _spawnPoint.position, progress);
            cachedTransform.rotation = Quaternion.Lerp(startRotation, _spawnPoint.rotation, progress);

            yield return new WaitForSeconds(0.02f);
        }

        cachedTransform.position = _spawnPoint.position;
        cachedTransform.rotation = _spawnPoint.rotation;

        foreach (var collider in GetComponentsInChildren<Collider2D>())
        {
            collider.enabled = true;
        }
    }
}
