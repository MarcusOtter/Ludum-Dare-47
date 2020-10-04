using System;
using UnityEngine;

public class AutomaticPlayerInput : PlayerInput
{
    private ReplayInputs _replayInputs;
    private int _inputIndex;

    private bool _left, _right, _dash;

    private void OnEnable()
    {
        GameManager.Instance.OnLevelStarted += ResetCurrentInput;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnLevelStarted -= ResetCurrentInput;
    }

    private void Update()
    {
        if (_inputIndex >= _replayInputs.InputEntries.Count) { return; }
        var inputEntry = _replayInputs.InputEntries[_inputIndex];

        if (GameManager.Instance.GetTimeSinceLevelStart() >= inputEntry.TimeOffset)
        {
            ApplyInput(inputEntry);
            _inputIndex++;
        }
    }

    public void SetInputs(ReplayInputs inputs)
    {
        _replayInputs = inputs;
    }

    public void ApplyInput(PlayerInputEntry replay)
    {
        var pressed = replay.WasPressedDown;
        switch (replay.InputType)
        {
            case PlayerInputType.Left:
                _left = pressed;
                break;
            case PlayerInputType.Right:
                _right = pressed;
                break;
            case PlayerInputType.Dash:
                _dash = pressed;
                break;
            default:
                throw new Exception("Somehow tried to process an input type which doesn't even exist, dude");
        }
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
        //only returns true for one frame in the replay as it does 
        if (_dash)
        {
            _dash = false;
            return true;
        }
        return false;
    }

    public override Vector3 GetStartPosition()
    {
        return _replayInputs.StartPosition;
    }

    private void ResetCurrentInput()
    {
        _inputIndex = 0;
        _right = false;
        _left = false;
        _dash = false;
    }
}