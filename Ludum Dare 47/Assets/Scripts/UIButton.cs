using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(UIUtilities))]
public class UIButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private AudioClip _onHoverSound, _onExitSound, _onClickDownSound, _onClickUpSound;
    private bool _hovering;
    [SerializeField] private UnityEvent _clickEvent;
    [SerializeField] private float _onClickScale = 0.9f;
    private Vector2 _baseScale;

    private void Awake()
    {
        _baseScale = transform.localScale;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        _hovering = true;
        PlaySound(_onHoverSound);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        LeanTween.cancel(gameObject);
        LeanTween.scale(gameObject, _baseScale, 0.3f).setEase(LeanTweenType.easeOutSine);
        PlaySound(_onExitSound);
        _hovering = false;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        PlaySound(_onClickDownSound);
        LeanTween.cancel(gameObject);
        LeanTween.scale(gameObject, _baseScale * _onClickScale, 0.3f).setEase(LeanTweenType.easeOutSine);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (_hovering)
        {
            PlaySound(_onClickUpSound);
            _clickEvent?.Invoke();
        }
        LeanTween.cancel(gameObject);
        LeanTween.scale(gameObject, _baseScale, 0.3f).setEase(LeanTweenType.easeOutSine);
    }

    private void PlaySound(AudioClip clip)
    {
        //insert actually playing the sound with whatever system we have here
    }
}
