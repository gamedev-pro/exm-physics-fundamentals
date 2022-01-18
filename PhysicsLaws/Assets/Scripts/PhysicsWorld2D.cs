using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class PhysicsWorld2D : MonoBehaviour
{
    private List<SimpleRigidbody2D> rbs = new List<SimpleRigidbody2D>();
    public void Register(SimpleRigidbody2D rb)
    {
        Assert.IsFalse(rbs.Contains(rb));
        rbs.Add(rb);
    }

    public void Unregister(SimpleRigidbody2D rb)
    {
        rbs.Remove(rb);
    }

    private void FixedUpdate()
    {
        Simulate(Time.fixedDeltaTime);
    }

    private void Simulate(float dt)
    {
        foreach (var rb in rbs)
        {
            var frameAcc = (rb.NetForce * rb.InverseMass * dt) + rb.InstantNetForce * rb.InverseMass;
            rb.Velocity = (rb.Velocity + frameAcc) * (1 - dt * rb.LinearDrag);
            rb.Position += rb.Velocity * dt;

            var frameAngularAcc = rb.AngularAcceleration * dt;
            rb.AngularVelocity = (rb.AngularVelocity + frameAngularAcc) * (1 - dt * rb.AngularDrag);
            rb.Orientation += rb.AngularVelocity * dt;
            rb.ResetForces();
        }
    }
}
