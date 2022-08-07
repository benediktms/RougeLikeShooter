using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField]
    protected Transform AttackPoint;

    [SerializeField] 
    public Camera Camera;

    [SerializeField] 
    ParticleSystem MuzzleFlash;

    [SerializeField]
    protected GameObject Round;

    [SerializeField]
    protected float Damage;
    private float _movementCounter;

    [SerializeField]
    protected float Range;

    [SerializeField]
    protected float ShootForce;

    private void Start()
    {
    }

    private void Update()
    {
    }
}

