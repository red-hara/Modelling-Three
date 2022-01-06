public interface Command
{
    void Init(Controllable controllable, Context context);
    State Process(float delta);
}

public enum State
{
    Going,
    Done,
    Error,
}
