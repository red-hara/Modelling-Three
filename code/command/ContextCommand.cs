public class ContextCommand : Command
{
    private Context context;
    private UpdateContext updater;

    public ContextCommand(UpdateContext updater)
    {
        this.updater = updater;
    }

    public void Init(Controllable controllable, Context context)
    {
        this.context = context;
    }
    public State Process(float delta)
    {
        updater(context);
        return State.Done;
    }

    public delegate void UpdateContext(Context context);
}
