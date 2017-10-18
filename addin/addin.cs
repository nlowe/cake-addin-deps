using System;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.Diagnostics;

namespace addin
{
    [CakeAliasCategory("Dummy")]
    public static class addin
    {
        [CakeMethodAlias]
        public static void Foo(this ICakeContext ctx) => ctx.Log.Information("Addin Works: Sha256WithRsaEncryption={0}", Org.BouncyCastle.Asn1.Pkcs.PkcsObjectIdentifiers.Sha256WithRsaEncryption);
    }
}
