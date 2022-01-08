public class PositionHold : Command
{
    private Controllable controllable;
    private Context context;

    private Pose4 target;

    public void Init(Controllable controllable, Context context)
    {
        this.controllable = controllable;
        this.context = context;
        target = controllable.GetCurrentPosition().pose * context.Tool;
    }
    public State Process(float delta)
    {
        Pose4 flangeTarget = target * context.Tool.Inverse();
        if (controllable.SetPosition(new Target4(flangeTarget, 0)) is null)
        {
            return State.Error;
        }
        return State.Going;
    }
}
