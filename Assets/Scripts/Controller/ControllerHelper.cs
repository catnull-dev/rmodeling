namespace RModeling.Controller
{
    public interface ICommand
    {
        void Init();
        CommandStatus Execute(float deltaTime);
    }

    public enum CommandStatus
    {
        Done,
        Failed,
        Executing
    }

    public enum ControllerStatus
    {
        Stop,
        Executing,
    }
}