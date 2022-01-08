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

    public bool movingTool;
    public float counter;


    public void AddCommand(Command command)
    {
        commands.Enqueue(command);
    }

    override public void _Process(float delta)
    {
        if (movingTool)
        {
            counter += delta;
            SimpleCone simpleCone = GetNode<SimpleCone>(tool);
            simpleCone.angle = 45 * Mathf.Sin(counter * Mathf.Pi / 4);
        }
    }

    public void GeneratePath()
    {
        AddCommand(
            new Joint(
                new Target4(new Pose4(new Vector3(2000, 0, 100), 0), 0),
                0.25f
            )
        );
        AddCommand(
            new Linear(new Pose4(new Vector3(2000, 0, 100), 0), 500, 90)
        );

        AddCommand(
            new ContextCommand(
                (context) => { context.movingTool = true; }
            )
        );
        AddCommand(new InputWait(KeyList.Space));
        AddCommand(
            new Linear(new Pose4(new Vector3(2000, 0, 300), 90), 100, 45)
        );
        AddCommand(
            new Linear(new Pose4(new Vector3(2000, 0, 500), 180), 100, 45)
        );
        AddCommand(
            new Linear(new Pose4(new Vector3(2000, 0, 700), 270), 100, 45)
        );
        AddCommand(
            new Linear(new Pose4(new Vector3(2000, 0, 900), 360), 100, 45)
        );
        AddCommand(
            new ContextCommand(
                (context) => { context.movingTool = false; }
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
