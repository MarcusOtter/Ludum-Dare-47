using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    public BugType BugType;
    
    [SerializeField] private float _movementSpeed = 2f;
    [SerializeField] private float _turningSpeed = 90f;

    private Rigidbody2D _rigidbody;
    private PlayerInput _input;
    private bool _canMove = true;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _input = GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {
        GameManager.Instance.OnLevelStarted += ResetPosition;
    }
    private void OnDisable()
    {
        GameManager.Instance.OnLevelStarted -= ResetPosition;
    }

    private void FixedUpdate()
    {
        if (!_canMove) { return; } 

        _rigidbody.velocity = transform.right * GetSpeed();
        _rigidbody.angularVelocity = -GetCurrentRotationDelta();
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

    private void OnTriggerEnter2D(Collider2D collider)
    {
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
