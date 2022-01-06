using System.Collections.Generic;
using Godot;

public class Controller : Node
{
    private Context context;
    private Queue<Command> commands = new Queue<Command>();
    private Command currentCommand;

    [Export]
    public NodePath controllable;

    public override void _Ready()
    {
        GeneratePath();
        context.tool.position.y = 1000;
        context.tool.position.z = 50;
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
                    if (commands.Count > 0)
                    {
                        currentCommand = commands.Dequeue();
                        currentCommand.Init(control, context);
                    }
                    break;
                case State.Error:
                    currentCommand = null;
                    commands.Clear();
                    GD.Print("Error!");
                    break;
            }
        }
        else if (commands.Count > 0)
        {
            currentCommand = commands.Dequeue();
            currentCommand.Init(control, context);
        }
        else
        {
            GeneratePath();
        }
    }

    public void SetTool(Pose4 tool)
    {
        context.tool = tool;
    }

    public void SetPart(Pose4 part)
    {
        context.part = part;
    }

    public void GeneratePath()
    {
        AddCommand(
            new Linear(new Pose4(new Vector3(2000, 0, 0), 0), 500, 90)
        );
        AddCommand(
            new Linear(new Pose4(new Vector3(2000, 0, 200), 90), 100, 90)
        );
        AddCommand(
            new Linear(new Pose4(new Vector3(2000, 0, 400), 180), 100, 90)
        );
        AddCommand(
            new Linear(new Pose4(new Vector3(2000, 0, 600), 270), 100, 90)
        );
        AddCommand(
            new Linear(new Pose4(new Vector3(2000, 0, 800), 360), 100, 90)
        );
    }

    public void AddCommand(Command command)
    {
        commands.Enqueue(command);
    }
}
