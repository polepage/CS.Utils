using System.Collections.Generic;

namespace CS.Utils.Console
{
    public class SingleCommandReader : ICommandReader
    {
        private readonly string[] _args;

        public SingleCommandReader(string[] args)
        {
            _args = args;
        }

        public IEnumerable<string[]> ReadCommands()
        {
            yield return _args;
        }
    }
}
