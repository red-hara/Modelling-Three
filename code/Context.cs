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
        get => GetPart();
    }
    [Export]
    public NodePath tool;
    public Pose4 Tool
    {
        get => GetTool();
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
            new Joint(
                new Target4(new Pose4(new Vector3(0, 2100, 100), 0), 0),
                0.25f
            )
        );
        AddCommand(new InputWait(KeyList.Space));
        AddCommand(
            new Linear(new Pose4(new Vector3(0, 2100, 300), 120), 100, 45)
        );
        AddCommand(
            new Linear(new Pose4(new Vector3(0, 2100, 500), 240), 100, 45)
        );
        AddCommand(
            new Linear(new Pose4(new Vector3(0, 2100, 700), 0), 100, 45)
        );
        AddCommand(
            new Linear(new Pose4(new Vector3(0, 2100, 900), 120), 100, 45)
        );
        AddCommand(
            new ContextCommand(
                (context) =>
                {
                    context.workWithFlange = !context.workWithFlange;
                }
            )
        );
        AddCommand(
            new ContextCommand(
                (context) => { context.GeneratePath(); }
            )
        );
    }

    public Pose4 GetTool()
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

    public Pose4 GetPart()
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
