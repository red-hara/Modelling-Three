public class PositionHold : Command
{
    private Controllable controllable;
    private Context context;

    private Target4 target;

    public void Init(Controllable controllable, Context context)
    {
        this.controllable = controllable;
        this.context = context;
        target = controllable.GetCurrentPosition() * context.Tool;
    }
    public State Process(float delta)
    {
        Target4 flangeTarget = target * context.Tool.Inverse();
        if (controllable.SetPosition(flangeTarget) is null)
        {
            return State.Error;
        }
        return State.Going;
    }
}
