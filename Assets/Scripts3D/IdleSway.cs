using Assets.Helpers;
using UnityEngine;

public class IdleSway : MonoBehaviour
{
    [Header("Idle Bob Settings")]
    [SerializeField] 
    private float _amount;

    [SerializeField] 
    private float _speed;

    [SerializeField] 
    private float _transitionSpeed;

    private Vector3 _origin;
    private float _idleCounter; // Used to determine the position of the swayee in X-Y

    // Start is called before the first frame update
    void Start()
    {
        _origin = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        ApplyIdleSway();
    }

    private void ApplyIdleSway()
    {
        var playerIsStationary = InputHelper.VerticalAxis == 0 && InputHelper.HorizontalAxis == 0;

        if (playerIsStationary)
        {
            var targetPosition = HeadBob(_amount, _amount);
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, Time.deltaTime * _transitionSpeed);
        }

        Vector3 HeadBob(float horizontalIntensity, float verticalIntensity)
        {
            _idleCounter += Time.deltaTime * _speed;
            return _origin + new Vector3(Mathf.Cos(_idleCounter) * horizontalIntensity, Mathf.Sin(_idleCounter * 2) * verticalIntensity, 0);
        }
    }
}
