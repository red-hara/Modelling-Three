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
        start = controllable.GetCurrentPosition();
        start.pose *= context.tool;
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
            if (controllable.SetPosition(
                    new Target4(target * context.tool.Inverse(), 0)
                ) is null)
            {
                return State.Error;
            }
            return State.Done;
        }

        Pose4 interpolated = start.pose.Interpolate(target, progress) *
            context.tool.Inverse();
        if (controllable.SetPosition(new Target4(interpolated, 0)) is null)
        {
            return State.Error;
        }
        return State.Going;
    }
}
