using Extendable.Abstraction;

namespace Extendable
{
    public class Configuration
    {
        #region Field Provider Configuration

        public static bool IsFieldProviderRegistered => Configuration.FieldProvider != null;

        public static IFieldProvider FieldProvider { set; get; }

        #endregion

        #region Cache Configuration

        public static bool CacheEnabled { get; set; } = true;

        public static long CacheSizeLimit { get; set; } =  1 * 1024 * 1024;

        #endregion
    }
}
