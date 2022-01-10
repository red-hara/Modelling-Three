using System;

public class Joint : Command
{
    private Controllable controllable;
    private Context context;

    private Target4 start;
    private Target4 target;

    private float velocity;
    private float progress;

    public Joint(Target4 target, float velocity)
    {
        this.target = target;
        if (velocity > 1 || velocity <= 0)
        {
            throw new ArgumentException("Wrong velocity");
        }
        this.velocity = velocity;
    }

    public void Init(Controllable controllable, Context context)
    {
        this.controllable = controllable;
        this.context = context;
        start = controllable.GetCurrentPosition() * context.Tool;
    }
    public State Process(float delta)
    {
        Generalized4? generalizedStart = controllable.Inverse(
            start * context.Tool.Inverse()
        );
        Generalized4? generalizedEnd = controllable.Inverse(
            target * context.Tool.Inverse()
        );
        if (generalizedStart is null || generalizedEnd is null)
        {
            return State.Error;
        }
        Generalized4 generalizedDelta =
            generalizedEnd.Value - generalizedStart.Value;
        Generalized4 times = generalizedDelta.Abs() /
            controllable.MaximumJointVelocity() * (1 / velocity);
        float maximum = times.Max();

        progress += delta / maximum;

        if (progress >= 1)
        {
            controllable.SetJoints(generalizedEnd.Value);
            return State.Done;
        }
        Generalized4 current = generalizedStart.Value +
            progress * generalizedDelta;
        controllable.SetJoints(current);
        return State.Going;
    }
}