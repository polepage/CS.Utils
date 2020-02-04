using System.Collections.Generic;

namespace CS.Utils.Console
{
    public interface ICommandReader
    {
        IEnumerable<string[]> ReadCommands();
    }
}
