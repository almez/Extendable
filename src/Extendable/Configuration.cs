using Extendable.Abstraction;

namespace Extendable
{
    public class Configuration
    {
        #region Field Provider

        public static bool IsFieldProviderRegistered => Configuration.FieldProvider != null;

        public static IFieldProvider FieldProvider { set; get; }

        #endregion
    }
}
