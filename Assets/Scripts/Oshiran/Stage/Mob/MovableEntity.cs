using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class MovableEntity : ExecutableEntity
{
    [SerializeField] protected float detectExtents;
    [SerializeField] protected Vector3 forceVector;
    [SerializeField] protected float duration;

    protected Vector3 originPos;
    protected Quaternion originRot;

    protected Transform mainCamera;
    protected CancellationTokenSource cTokenSource;
    protected Rigidbody2D rig;

    bool defaultSimulated;


    void Start()
    {
        mainCamera = Camera.main.transform;
        originPos = transform.position;
        originRot = transform.rotation;
        rig = GetComponent<Rigidbody2D>();
        defaultSimulated = rig.simulated;
        cTokenSource = new CancellationTokenSource();

        ExecuteAsync(cTokenSource.Token).Forget();
    }


    protected virtual UniTaskVoid ExecuteAsync(CancellationToken cToken)
    {
        return new UniTaskVoid();
    }


    public override void Rewind()
    {
        cTokenSource.Cancel();

        gameObject.SetActive(true);

        rig.velocity = Vector2.zero;
        rig.simulated = defaultSimulated;
        transform.position = originPos;
        transform.rotation = originRot;

        cTokenSource?.Dispose();
        cTokenSource = new CancellationTokenSource();

        ExecuteAsync(cTokenSource.Token).Forget();
    }


    void OnDestroy()
    {
        cTokenSource.Cancel();
    }
}