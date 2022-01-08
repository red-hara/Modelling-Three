using Godot;

public class Controller : Node
{
    [Export]
    public NodePath context;
    private Context ctx;
    private Command currentCommand;

    [Export]
    public NodePath controllable;

    [Export]
    public NodePath tcpTrace;

    public override void _Ready()
    {
        ctx = GetNode<Context>(context);
        ctx.GeneratePath();
    }

    public override void _Process(float delta)
    {
        Controllable control = GetNode<Controllable>(controllable);
        if (!(currentCommand is null))
        {
            State state = currentCommand.Process(delta);
            switch (state)
            {
                case State.Going:
                    break;
                case State.Done:
                    currentCommand = null;
                    if (ctx.commands.Count > 0)
                    {
                        currentCommand = ctx.commands.Dequeue();
                        currentCommand.Init(control, ctx);
                    }
                    break;
                case State.Error:
                    currentCommand = null;
                    ctx.commands.Clear();
                    GD.Print("Error!");
                    break;
            }
        }
        else if (ctx.commands.Count > 0)
        {
            currentCommand = ctx.commands.Dequeue();
            currentCommand.Init(control, ctx);
        }
        // ctx.Update(delta);

        Trace trace = GetNode<Trace>(tcpTrace);
        Spatial controlSpatial = (Spatial)control;
        trace.GlobalTransform = controlSpatial.GlobalTransform *
            new Transform(
                Quat.Identity,
                (
                    control.GetCurrentPosition().pose *
                    ctx.Tool
                ).position
            );
    }
}
