using System;
using System.ComponentModel;
using System.Collections.Generic;

namespace JetBrains.Annotations
{
    public enum UriScheme
    {
        /// <summary>None</summary>
        [Description("None")]
        None = default,

        /// <summary>Any</summary>
        [Description("Any")]
        Any,

        /// <summary>Pointer to a file</summary>
        [Description("Pointer to a file")]
        File,

        /// <summary>File Transfer Protocol (FTP)</summary>
        [Description("File Transfer Protocol (FTP)")]
        Ftp,

        /// <summary>Gopher protocol</summary>
        [Description("Gopher protocol")]
        Gopher,

        /// <summary>Hypertext Transfer Protocol (HTTP)</summary>
        [Description("Hypertext Transfer Protocol (HTTP)")]
        Http,

        /// <summary>Secure Hypertext Transfer Protocol (HTTPS)</summary>
        [Description("Secure Hypertext Transfer Protocol (HTTPS)")]
        Https,

        /// <summary>Simple Mail Transport Protocol (SMTP)</summary>
        [Description("Simple Mail Transport Protocol (SMTP)")]
        Mailto,

        /// <summary>Network News Transport Protocol (NNTP)</summary>
        [Description("Network News Transport Protocol (NNTP)")]
        News,

        /// <summary>Network News Transport Protocol (NNTP)</summary>
        [Description("Network News Transport Protocol (NNTP)")]
        Nntp,

        /// <summary>Windows Communication Foundation (WCF)</summary>
        [Description("Windows Communication Foundation (WCF)")]
        NetTcp,

        /// <summary>Windows Communication Foundation (WCF)</summary>
        [Description("Windows Communication Foundation (WCF)")]
        NetPipe
    }

    public static class UriSchemes
    {
        private const string Const_None = "None";
        private const string Const_Any = "Any";

        [NotNull] public static readonly IReadOnlyDictionary<UriScheme, string> Value2Name;
        [NotNull] public static readonly IReadOnlyDictionary<string, UriScheme> Name2Value;

        static UriSchemes()
        {
            Value2Name = new Dictionary<UriScheme, string> {
                [UriScheme.None] = Const_None,
                [UriScheme.Any] = Const_Any,
                [UriScheme.File] = Uri.UriSchemeFile,
                [UriScheme.Ftp] = Uri.UriSchemeFtp,
                [UriScheme.Gopher] = Uri.UriSchemeGopher,
                [UriScheme.Http] = Uri.UriSchemeHttp,
                [UriScheme.Https] = Uri.UriSchemeHttps,
                [UriScheme.Mailto] = Uri.UriSchemeMailto,
                [UriScheme.News] = Uri.UriSchemeNews,
                [UriScheme.Nntp] = Uri.UriSchemeNntp,
                [UriScheme.NetTcp] = Uri.UriSchemeNetTcp,
                [UriScheme.NetPipe] = Uri.UriSchemeNetPipe
            };

            Name2Value = new Dictionary<string, UriScheme>(StringComparer.InvariantCultureIgnoreCase) {
                [Const_None] = UriScheme.None,
                [Const_Any] = UriScheme.Any,
                [Uri.UriSchemeFile] = UriScheme.File,
                [Uri.UriSchemeFtp] = UriScheme.Ftp,
                [Uri.UriSchemeGopher] = UriScheme.Gopher,
                [Uri.UriSchemeHttp] = UriScheme.Http,
                [Uri.UriSchemeHttps] = UriScheme.Https,
                [Uri.UriSchemeMailto] = UriScheme.Mailto,
                [Uri.UriSchemeNews] = UriScheme.News,
                [Uri.UriSchemeNntp] = UriScheme.Nntp,
                [Uri.UriSchemeNetTcp] = UriScheme.NetTcp,
                [Uri.UriSchemeNetPipe] = UriScheme.NetPipe
            };
        }
    }
}
