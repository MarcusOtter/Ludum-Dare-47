using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIUtilities : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private bool _easeIn = true, _expandOnHover;
    [SerializeField] private float _growthFactor = 1.3f;
    private List<Transform> _popups = new List<Transform>(1);

    private void Start()
    {
        if (GetComponent<UIButton>() != null)
        {
            _expandOnHover = true;
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        foreach(Transform t in _popups)
        {
            t.gameObject.SetActive(true);
            Vector3 scale = new Vector2(1f,1f);
            t.localScale = Vector3.zero;
            LeanTween.scale(t.gameObject, scale, 0.3f).setEase(LeanTweenType.easeInOutSine);
        }
        if(_expandOnHover)
        {
            transform.localScale = new Vector2(1f,1f);
            LeanTween.scale(gameObject, transform.localScale * _growthFactor, 0.3f).setEase(LeanTweenType.easeInOutSine);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        foreach (Transform t in _popups)
        {
            LeanTween.scale(t.gameObject, Vector2.zero, 0.3f).setEase(LeanTweenType.easeInOutSine);
        }
        if (_expandOnHover)
        {
            LeanTween.scale(gameObject, new Vector2(1f, 1f), 0.3f).setEase(LeanTweenType.easeInOutSine);
        }
    }

    private void OnEnable()
    {
        if (_easeIn)
        {
            Vector3 scale = transform.localScale;
            transform.localScale = Vector3.zero;
            LeanTween.scale(gameObject, scale, 0.3f).setEase(LeanTweenType.easeInOutSine);
        }
        for(int i = 0; i < transform.childCount; i++)
        {
            if(transform.GetChild(i).CompareTag("Popup"))
            {
                _popups.Add(transform.GetChild(i));
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}
