using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public abstract class Shape2D : MonoBehaviour
{
    [SerializeField] private float mass = 1;
    public float InverseMass { get; private set; }

    public float Mass => mass;

    private void Awake()
    {
        UpdateInverseMass();
    }

    private void OnValidate()
    {
        UpdateInverseMass();
    }

    private void UpdateInverseMass()
    {
        Assert.IsFalse(Mathf.Approximately(mass, 0), "0 mass not accepted");
        InverseMass = Mathf.Approximately(mass, 0) ? 0 : 1.0f / mass;
    }
}
