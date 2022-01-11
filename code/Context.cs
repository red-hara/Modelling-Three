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
        AddCommand(new InputWait(KeyList.Space));
        AddCommand(
            new ContextCommand(
                (context) => { context.workWithFlange = true; }
            )
        );
        AddCommand(
            new Joint(
                new Target4(new Pose4(new Vector3(2000, 0, 1000), 0), 0),
                0.25f
            )
        );
        AddCommand(new InputWait(KeyList.Space));

        AddCommand(
            new ContextCommand(
                (context) => { context.workWithFlange = false; }
            )
        );
        AddCommand(
            new Linear(
                new Pose4(new Vector3(1750, 250, 250), -45), 100, 90
            )
        );
        AddCommand(
            new Linear(
                new Pose4(new Vector3(2250, 250, 250), -135), 100, 90
            )
        );
        AddCommand(
            new Linear(
                new Pose4(new Vector3(2250, -250, 250), 135), 100, 90
            )
        );
        AddCommand(
            new Linear(
                new Pose4(new Vector3(1750, -250, 250), 45), 100, 90
            )
        );
        AddCommand(
            new Linear(
                new Pose4(new Vector3(1750, 250, 250), -45), 100, 90
            )
        );
        AddCommand(
            new Linear(
                new Pose4(new Vector3(1750, 250, 500), -45), 100, 90
            )
        );

        AddCommand(
            new ContextCommand(
                (context) => { context.GeneratePath(); }
            )
        );
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
}
