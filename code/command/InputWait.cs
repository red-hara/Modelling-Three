using Godot;

public class InputWait : Command
{
    private KeyList button;

    public InputWait(KeyList button)
    {
        this.button = button;
    }
    public void Init(Controllable controllable, Context context) { }
    public State Process(float delta)
    {
        if (Input.IsPhysicalKeyPressed(((int)button)))
        {
            return State.Done;
        }
        return State.Going;
    }
}