public class Idle : Command
{
    private float counter;

    private float total;

    public Idle(float total)
    {
        this.total = total;
    }

    public void Init(Controllable controllable, Context context) { }
    public State Process(float delta)
    {
        counter += delta;
        if (counter >= total)
        {
            return State.Done;
        }
        return State.Going;
    }
}
