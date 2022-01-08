using Godot;

public class InputWait : Command
{
    private PositionHold hold = new PositionHold();

    private KeyList button;

    public InputWait(KeyList button)
    {
        this.button = button;
    }
    public void Init(Controllable controllable, Context context)
    {
        hold.Init(controllable, context);
    }
    public State Process(float delta)
    {
        if (hold.Process(delta) == State.Error)
        {
            return State.Error;
        }
        if (Input.IsPhysicalKeyPressed(((int)button)))
        {
            return State.Done;
        }
        return State.Going;
    }
}
