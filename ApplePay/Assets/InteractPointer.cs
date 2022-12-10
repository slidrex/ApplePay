[UnityEngine.RequireComponent(typeof(UnityEngine.CircleCollider2D))]
public class InteractPointer : UnityEngine.MonoBehaviour
{
    public UnityEngine.Transform hintPosition;
    public InteractiveObject AttachedInteractive;
    public UnityEngine.CircleCollider2D rangeCollider;
    private void Start()
    {
        rangeCollider = GetComponent<UnityEngine.CircleCollider2D>();
    }
}
