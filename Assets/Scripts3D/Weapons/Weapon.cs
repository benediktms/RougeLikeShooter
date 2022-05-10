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

    [SerializeField]
    protected float Range;

    [SerializeField]
    protected float ShootForce;

    protected abstract void Fire();
    [Header("Bob Settings")]
    private Vector3 _weaponOrigin;

    private void Start()
    {
        _weaponOrigin = transform.localPosition;
    }

    private void Update()
    {
        RotationSway();
    }

    public void PlayMuzzleFlash()
    {
        MuzzleFlash.Play();
    }

    void RotationSway()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * _swayMultiplier;
        float mouseY = Input.GetAxisRaw("Mouse Y") * _swayMultiplier;

        Quaternion rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);

        Quaternion targetRotation = rotationX * rotationY;

        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, _smooth * Time.deltaTime);
    }

    void HeadBob()
    {
    }
}
