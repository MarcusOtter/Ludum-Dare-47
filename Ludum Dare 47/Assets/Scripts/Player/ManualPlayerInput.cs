using System.Collections.Generic;
using UnityEngine;

public class ManualPlayerInput : PlayerInput
{
    private bool _left, _right, _dash;
    private List<PlayerInputEntry> _playerInputs = new List<PlayerInputEntry>(64);

    private bool _isLogging;
    private Vector3 _startPosition;


    private void Start()
    {
        StartLogging();
    }

    //private void OnEnable()
    //{
    //    GameManager.Instance.OnLevelStarted += StartLogging;
    //    GameManager.Instance.OnLevelCompleted += StopLogging;
    //}

    //private void OnDisable()
    //{
    //    GameManager.Instance.OnLevelStarted -= StartLogging;
    //    GameManager.Instance.OnLevelCompleted -= StopLogging;
    //}

    private void Update()
    {
        if (!_isLogging) { return; }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            LogKeyPress(PlayerInputType.Dash, true);
            _dash = true;
        }
        else
        {
            _dash = false;
        }

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            LogKeyPress(PlayerInputType.Left, true);
            _left = true;
        }
        else if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow))
        {
            LogKeyPress(PlayerInputType.Left, false);
            _left = false;
        }

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            LogKeyPress(PlayerInputType.Right, true);
            _right = true;
        }
        else if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            LogKeyPress(PlayerInputType.Right, false);
            _right = false;
        }
    }

    public ReplayInputs GetReplayInputs()
    {
        ReplayInputs replay = new ReplayInputs()
        {
            InputEntries = _playerInputs,
            StartPosition = _startPosition,
            BugType = BugType.Ant
        };

        return replay;
    }

    private void StartLogging()
    {
        _isLogging = true;
        _startPosition = transform.position;
    }

    private void StopLogging()
    {
        _isLogging = false;
    }

    private void LogKeyPress(PlayerInputType inputType, bool pressedDown)
    {
        var inputEntry = new PlayerInputEntry
        {
            InputType = inputType,
            TimeOffset = GameManager.Instance.GetTimeSinceLevelStart(),
            WasPressedDown = pressedDown
        };

        _playerInputs.Add(inputEntry);
    }

    public override bool GetLeft()
    {
        return _left;
    }

    public override bool GetRight()
    {
        return _right;
    }

    public override bool GetDash()
    {
        return _dash;
    }

    public override Vector3 GetStartPosition()
    {
        return _startPosition;
    }
}
