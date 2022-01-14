using System.Collections.Generic;
using Godot;

public class Context : Node
{
    public Queue<Command> commands = new Queue<Command>();

    public Pose4 origin;
    [Export]
    public NodePath part;
    public Pose4 Part
    {
        get => GetPartOrigin();
    }
    [Export]
    public NodePath tool;
    public Pose4 Tool
    {
        get => GetToolCenterPoint();
    }

    public bool workWithFlange;

    public void AddCommand(Command command)
    {
        commands.Enqueue(command);
    }

    override public void _Process(float delta)
    {

    }

    public void GeneratePath()
    {
        InputWait(KeyList.Space);
        ContextCommand((context) => { context.workWithFlange = true; });
        Joint(new Target4(new Pose4(new Vector3(2000, 0, 1000), 0), 0), 0.25f);
        InputWait(KeyList.Space);
        ContextCommand((context) => { context.workWithFlange = false; });
        Linear(new Pose4(new Vector3(1750, 250, 250), -90), 100, 90);
        ContextCommand((context) => { context.TurnToolOn(); });

        Linear(new Pose4(new Vector3(2250, 250, 250), -90), 100, 90);
        Linear(new Pose4(new Vector3(2250, 250, 250), 180), 100, 90);

        Linear(new Pose4(new Vector3(2250, -250, 250), 180), 100, 90);
        Linear(new Pose4(new Vector3(2250, -250, 250), 90), 100, 90);

        Linear(new Pose4(new Vector3(1750, -250, 250), 90), 100, 90);
        Linear(new Pose4(new Vector3(1750, -250, 250), 0), 100, 90);

        Linear(new Pose4(new Vector3(1750, 250, 250), 0), 100, 90);

        ContextCommand((context) => { context.TurnToolOff(); });
        Linear(new Pose4(new Vector3(1750, 250, 500), 0), 100, 90);
        ContextCommand((context) => { context.GeneratePath(); });
    }

    public Pose4 GetToolCenterPoint()
    {
        if (workWithFlange)
        {
            return new Pose4();
        }
        if (tool is null)
        {
            return new Pose4();
        }
        if (tool.IsEmpty())
        {
            return new Pose4();
        }
        Tool tcp = GetNode<Tool>(tool);
        return tcp.GetToolCenterPoint();
    }

    public Pose4 GetPartOrigin()
    {
        if (part is null)
        {
            return new Pose4();
        }
        if (part.IsEmpty())
        {
            return new Pose4();
        }
        Part pt = GetNode<Part>(part);
        return pt.GetOrigin();
    }

    public void TurnToolOn()
    {
        GetNode<SimpleCone>(tool).TurnOn();
    }

    public void TurnToolOff()
    {
        GetNode<SimpleCone>(tool).TurnOff();
    }

    public void Linear(
        Pose4 target,
        float linearVelocity,
        float angularVelocity
    )
    {
        AddCommand(new Linear(target, linearVelocity, angularVelocity));
    }

    public void Joint(
        Target4 target,
        float velocity
    )
    {
        AddCommand(new Joint(target, velocity));
    }

    public void ContextCommand(ContextCommand.UpdateContext command)
    {
        AddCommand(new ContextCommand(command));
    }

    public void InputWait(KeyList list)
    {
        AddCommand(new InputWait(KeyList.Space));
    }
}
