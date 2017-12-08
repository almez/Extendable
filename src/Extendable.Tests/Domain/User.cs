using Extendable.Abstraction;

namespace Extendable.Tests.Domain
{
    public class User : IExtendable
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
