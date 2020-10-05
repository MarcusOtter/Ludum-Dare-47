using System.Collections;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    [SerializeField] private float _distance, _time, _lingerTime;
    private Rigidbody2D _rb;
    private bool _isDashing;
    private Transform _dashHitBox;
    private Vector2 _colliderChildPos;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _dashHitBox = transform.Find("DashHitBox");
        _colliderChildPos = _dashHitBox.localPosition;
    }

    private void OnEnable()
    {
        GetComponent<PlayerMovement>().OnDash += Dash;
        GameManager.Instance.OnLevelStarted += StopDashingImmediately;
    }

    private void OnDisable()
    {
        GetComponent<PlayerMovement>().OnDash -= Dash;
        GameManager.Instance.OnLevelStarted -= StopDashingImmediately;
    }

    public bool IsDashing()
    {
        return _isDashing;
    }

    private void Dash()
    {
        if(!_isDashing)StartCoroutine(DashRoutine());
    }

    public void StopDashingImmediately()
    {
        StopAllCoroutines();
        _dashHitBox.gameObject.SetActive(false);
        _dashHitBox.localPosition = _colliderChildPos;
        _isDashing = false;
    }

    private IEnumerator DashRoutine()
    {
        _isDashing = true;
        var speed = _distance / _time;
        var startTime = GameManager.Instance.GetTimeSinceLevelStart();
        var dashHitBoxTransform = _dashHitBox.transform; // For performance

        _dashHitBox.gameObject.SetActive(true);

        Vector2 startPos = _dashHitBox.position;

        while (GameManager.Instance.GetTimeSinceLevelStart() < startTime + _time)
        {
            var hitBoxPosition = (Vector2) dashHitBoxTransform.position; // For performance

            dashHitBoxTransform.right = startPos - hitBoxPosition;
            _dashHitBox.localScale = dashHitBoxTransform.localScale.With(x: -(startPos - hitBoxPosition).magnitude / transform.localScale.x);
            
            if (GameManager.Instance.GetTimeSinceLevelStart() < startTime) break; //Means we've restarted
            _rb.velocity = transform.right * speed;
            yield return null;
        }

        //keep it still without unchilding it so you don't collide with your own dash and die
        var colliderChildPos = _dashHitBox.localPosition;
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
        _dashHitBox.localPosition = colliderChildPos;
        _dashHitBox.rotation = Quaternion.identity;

        _rb.velocity = Vector2.zero;
        _dashHitBox.gameObject.SetActive(false);
        _isDashing = false;
    }
}
