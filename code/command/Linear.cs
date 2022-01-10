using Godot;

public class Linear : Command
{
    private Controllable controllable;
    private Context context;

    private Target4 start;
    private Pose4 target;

    private float linearVelocity;
    private float angularVelocity;

    private float progress;

    public Linear(Pose4 target, float vLin, float vAng)
    {
        this.target = target;
        linearVelocity = vLin;
        angularVelocity = vAng;
    }

    public void Init(Controllable controllable, Context context)
    {
        this.controllable = controllable;
        this.context = context;
        start = controllable.GetCurrentPosition() * context.Tool;
    }

    public State Process(float delta)
    {
        Pose4 deltaPose = start.pose.Inverse() * target;
        float length = linearVelocity / deltaPose.position.Length();
        float angle = angularVelocity / Mathf.Abs(
            Mathf.Wrap(deltaPose.rotation, -180, 180)
        );

        progress += delta * Mathf.Min(length, angle);
        if (progress >= 1)
        {
            Pose4 tmp = deltaPose * context.Tool.Inverse();
            Target4 result = start * tmp;
            if (controllable.SetPosition(result) is null)
            {
                return State.Error;
            }
            return State.Done;
        }
        Pose4 interpolated = new Pose4().Interpolate(deltaPose, progress);
        Pose4 withTool = interpolated * context.Tool.Inverse();
        Target4 resultTarget = start * withTool;

        if (controllable.SetPosition(resultTarget) is null)
        {
            return State.Error;
        }
        return State.Going;
    }
}
