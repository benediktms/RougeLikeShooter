using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField]
    protected Camera Camera;

    [SerializeField]
    protected Transform AttackPoint;

    [SerializeField]
    protected GameObject Round;

    [SerializeField]
    protected float Damage;
    [Header("Sway Settings")]
    [SerializeField] private float _smooth;
    [SerializeField] private float _multiplier;
    private float _mouseX;
    private float _mouseY;

    void OnStart()
    {
        PlayerController = GetComponentInParent<PlayerController>();
    }

    [SerializeField]
    protected float Range;

    [SerializeField]
    protected float ShootForce;

    protected abstract void Fire();
}
