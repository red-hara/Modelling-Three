using System.Collections.Generic;
using Godot;

/// <summary>The robot execution context.</summary>
public class Context : Node
{
    /// <summary>List on enqueued commands.</summary>
    public Queue<Command> commands = new Queue<Command>();

    /// <summary>Path to the <c>Part</c> attached to the robot base.</summary>
    [Export]
    public NodePath part;
    /// <summary><c>Part</c> to work with.</summary>
    public Pose4 Part
    {
        // It is possible to get part, but not to set it.
        get => GetPartOrigin();
    }

    /// <summary>Path to the <c>Tool</c> attached to the robot.</summary>
    [Export]
    public NodePath tool;

    /// <summary><c>Tool</c> to work with.</summary>
    public Pose4 Tool
    {
        // It is possible to get tool, but not to set it.
        get => GetToolCenterPoint();
    }

    /// <summary>Flag to mark that the <c>Context</c> shall work using
    /// tool.<summary>
    public bool workWithTool;

    /// <summary>Add command to the command list.</summary>
    /// <param name="command">The <c>Command</c> to be enqueued.</param>
    public void AddCommand(Command command)
    {
        commands.Enqueue(command);
    }

    override public void _Process(float delta)
    {

    }

    /// <summary>Generate path by adding commands to the queue.</summary>
    public void GeneratePath()
    {
        InputWait(KeyList.Space);
        ContextCommand((context) => { context.workWithTool = false; });
        Joint(new Target4(new Pose4(new Vector3(2000, 0, 1000), 0), 0), 0.25f);
        InputWait(KeyList.Space);
        ContextCommand((context) => { context.workWithTool = true; });
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

    /// <summary>Get current TCP geometry.</summary>
    /// <returns>TCP in relation to the flange.</returns>
    public Pose4 GetToolCenterPoint()
    {
        // Return zero Pose4 if either
        // - we are working with flange
        // - no tool path is set
        // - the tool path is empty
        if (!workWithTool)
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

    /// <summary>Get current part origin geometry.</summary>
    /// <returns>Part origin in relation to the robot base.</summary>
    public Pose4 GetPartOrigin()
    {
        // Return zero Pose4 if either
        // - no part path is set
        // - the part path is empty
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

    /// <summary>Turn <c>SimpleConeTool</c> on.</summary>
    public void TurnToolOn()
    {
        GetNode<SimpleConeTool>(tool).TurnOn();
    }

    /// <summary>Turn <c>SimpleConeTool</c> off.</summary>
    public void TurnToolOff()
    {
        GetNode<SimpleConeTool>(tool).TurnOff();
    }

    /// <summary>Enqueue <c>Linear</c> command.</summary>
    /// <param name="target">Target <c>Pose4</c> for this motion.</param>
    /// <param name="linearVelocity">Maximum motion linear velocity, millimeter
    /// per second.</param>
    /// <param name="angularVelocity">Maximum motion angular velocity, degree
    /// per second.</param>
    public void Linear(
        Pose4 target,
        float linearVelocity,
        float angularVelocity
    )
    {
        AddCommand(new Linear(target, linearVelocity, angularVelocity));
    }

    /// <summary>Enqueue <c>Joint</c> command.</summary>
    /// <param name="target">Target <c>Target4</c> for this motion.</param>
    /// <param name="velocity">Motion velocity, fraction, in (1, 0]
    /// range.<param>
    public void Joint(
        Target4 target,
        float velocity
    )
    {
        AddCommand(new Joint(target, velocity));
    }

    /// <summary>Enqueue <c>ContextCommand</c> command.</summary>
    /// <param name="command">The <c>UpdateContext</c> delegate to be called on
    /// this <c>Context</c>.</param>
    public void ContextCommand(ContextCommand.UpdateContext command)
    {
        AddCommand(new ContextCommand(command));
    }

    /// <summary>Enqueue <c>InputWait</c> command.</summary>
    /// <param name="key">The key (from the <c>KeyList</c>) to wait for.</param>
    public void InputWait(KeyList key)
    {
        AddCommand(new InputWait(key));
    }
}
