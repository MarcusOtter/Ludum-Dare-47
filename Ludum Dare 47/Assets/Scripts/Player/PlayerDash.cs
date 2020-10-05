using System.Collections;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    [SerializeField] private float _distance, _time, _lingerTime;
    private Rigidbody2D _rb;
    private bool _hasDashHitbox, _isDashing;
    private Transform _dashHitBox;
    private Vector2 _colliderChildPos;
    private PlayerMovement _playerMovement;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _dashHitBox = transform.Find("DashHitBox");
        _colliderChildPos = _dashHitBox.localPosition;
    }

    private void OnEnable()
    {
        GetComponent<PlayerMovement>().OnDash += Dash;
        GameManager.Instance.OnLevelStart += StopDashingImmediately;
    }

    private void OnDisable()
    {
        GetComponent<PlayerMovement>().OnDash -= Dash;
        GameManager.Instance.OnLevelStart -= StopDashingImmediately;
    }

    public bool IsDashing()
    {
        return _isDashing;
    }

    private void Dash()
    {
        if(!_hasDashHitbox)StartCoroutine(DashRoutine());
    }

    public void StopDashingImmediately()
    {
        _dashHitBox.gameObject.SetActive(false);
        _dashHitBox.localPosition = _colliderChildPos;
        _dashHitBox.localRotation = Quaternion.identity;
        _hasDashHitbox = false;
        _isDashing = false;
        StopAllCoroutines();
    }

    private IEnumerator DashRoutine()
    {
        _hasDashHitbox = true;
        _isDashing = true;
        var speed = _distance / _time;
        var startTime = GameManager.Instance.GetTimeSinceLevelStart();
        var dashHitBoxTransform = _dashHitBox.transform; // For performance

        _dashHitBox.gameObject.SetActive(true);
        _dashHitBox.GetComponentInChildren<SpriteRenderer>().flipY = _rb.velocity.x > 0f;

        Vector2 startPos = _dashHitBox.position;

        while (GameManager.Instance.GetTimeSinceLevelStart() < startTime + _time)
        {
            var hitBoxPosition = (Vector2) dashHitBoxTransform.position; // For performance

            dashHitBoxTransform.right = startPos - hitBoxPosition;
            _dashHitBox.localScale = dashHitBoxTransform.localScale.With(x: -(startPos - hitBoxPosition).magnitude / transform.localScale.x);

            if (GameManager.Instance.GetTimeSinceLevelStart() < startTime) //Means we've restarted
            {
                StopDashingImmediately();
                yield break;
            }
            _rb.velocity = transform.right * speed;
            yield return null;
        }
        _isDashing = false;
        //keep it still without unchilding it so you don't collide with your own dash and die
        //var colliderChildPos = _dashHitBox.localPosition;
        var colliderChildRot = _dashHitBox.rotation;

        float startLingerTime = GameManager.Instance.GetTimeSinceLevelStart();
        Vector2 startLingerPosition = _dashHitBox.position;

        while (GameManager.Instance.GetTimeSinceLevelStart() < startLingerTime + _lingerTime)
        {
            _dashHitBox.position = startLingerPosition;
            _dashHitBox.rotation = colliderChildRot;
            yield return null;
        }

        //set it back to where it was
        _dashHitBox.localPosition = _colliderChildPos;
        _dashHitBox.rotation = Quaternion.identity;

        _rb.velocity = Vector2.zero;
        _dashHitBox.gameObject.SetActive(false);
        _hasDashHitbox = false;
    }
}
