using Assets.Helpers;
using UnityEngine;

public class LookSway : MonoBehaviour
{
    [Header("Mouse Look Sway Settings")]
    [SerializeField]
    private float _smooth = 5;

    [SerializeField]
    private float _swayMultiplier = 10;

    // Update is called once per frame
    void Update()
    {
        ApplyRotationSway();
    }

    public virtual void ApplyRotationSway()
    {
        var inputX = InputHelper.MouseXAxisRaw;
        var inputY = InputHelper.MouseYAxisRaw;

        float mouseX = inputX * _swayMultiplier;
        float mouseY = inputY * _swayMultiplier;

        Quaternion rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);

        Quaternion targetRotation = rotationX * rotationY;

        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, _smooth * Time.deltaTime);
    }
}
