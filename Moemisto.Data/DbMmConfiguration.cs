using System.Data.Entity;
using System.Data.Entity.Core.Common;
using EFCache;

namespace Moemisto.Data
{
    //public class DbMmConfiguration : DbConfiguration
    //{
    //    public DbMmConfiguration()
    //    {
    //        var transactionHandler = new CacheTransactionHandler(new InMemoryCache());
    //
    //        AddInterceptor(transactionHandler);
    //
    //        Loaded +=
    //          (sender, args) => args.ReplaceService<DbProviderServices>(
    //            (s, _) => new CachingProviderServices(s, transactionHandler));
    //    }
    //}
}
