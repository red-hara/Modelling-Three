using System.Collections.Generic;
using Godot;

public class Context : Node
{
    public Queue<Command> commands = new Queue<Command>();

    public Pose4 origin;
    public Pose4 part;
    public Pose4 tool;

    public bool movingTool;
    public float counter;


    public void AddCommand(Command command)
    {
        commands.Enqueue(command);
    }

    public void Update(float delta)
    {
        if (movingTool)
        {
            counter += delta;
            tool.position.z = -50 + 100 * Mathf.Sin(counter * Mathf.Pi);
        }
    }

    public void GeneratePath()
    {
        AddCommand(new SetTool(new Pose4(new Vector3(0, 1000, -50), 0)));
        AddCommand(
            new Joint(
                new Target4(new Pose4(new Vector3(2000, 0, 100), 0), 0),
                0.25f
            )
        );
        AddCommand(
            new Linear(new Pose4(new Vector3(2000, 0, 100), 0), 500, 90)
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
                (context) => { context.movingTool = !context.movingTool; }
            )
        );
        AddCommand(
            new ContextCommand(
                (context) => { context.GeneratePath(); }
            )
        );
    }
}
