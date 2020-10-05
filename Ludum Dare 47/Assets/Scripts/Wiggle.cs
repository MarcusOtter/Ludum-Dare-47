using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wiggle : MonoBehaviour
{
    [SerializeField] private float _speed, _randomSpeedVariation, _degrees, _randomDegreesVariation;
    private float _startRotation;
    private Vector3 _euler = new Vector3();

    private void Awake()
    {
        _startRotation = transform.localRotation.z;
        _speed += Random.Range(-_randomSpeedVariation, _randomSpeedVariation);
        _degrees += Random.Range(-_randomDegreesVariation, _randomDegreesVariation);
    }

    // Update is called once per frame
    void Update()
    {
        _euler.z = _startRotation + Mathf.Sin(Time.time * _speed) * _degrees;
        transform.localEulerAngles = _euler;
    }
}
