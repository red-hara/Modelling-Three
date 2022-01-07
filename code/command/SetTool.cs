public class SetTool : Command
{
    private Context context;
    private Pose4 newTool;

    public SetTool(Pose4 newTool)
    {
        this.newTool = newTool;
    }

    public void Init(Controllable controllable, Context context)
    {
        this.context = context;
    }
    public State Process(float delta)
    {
        context.tool = newTool;
        return State.Done;
    }
}