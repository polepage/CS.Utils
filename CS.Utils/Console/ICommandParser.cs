namespace CS.Utils.Console
{
    public interface ICommandParser
    {
        bool ParseStartupCommand(string[] args, out int status);
        bool ParseCommand(string[] args, out int status);
    }
}
