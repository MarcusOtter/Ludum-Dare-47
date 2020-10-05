using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private bool _kill = true, _scaleDown = false;
    [SerializeField] private Vector2 _pushVector = new Vector2();
    [SerializeField] private CustomAudioClip _clip;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var p = collision.GetComponent<PlayerMovement>();
        if (p != null)
        {
            if(_kill) p.Die();
            p.GetComponent<Rigidbody2D>().AddForce(_pushVector);
            if(_scaleDown)
            {
                LeanTween.scale(p.gameObject, Vector2.zero, 0.5f);
            }
            if (_clip != null) AudioPlayerSpawner.Instance.PlaySoundEffect(_clip);
        }
    }
}
