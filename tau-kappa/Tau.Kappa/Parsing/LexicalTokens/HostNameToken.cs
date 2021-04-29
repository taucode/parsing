using System;
using System.Collections.Generic;
using System.Text;
using TauCode.Data;

namespace Tau.Kappa.Parsing.LexicalTokens
{
    public class HostNameToken : TextToken
    {
        public HostNameToken(
            int position,
            int consumedLength,
            HostName hostName)
            : base(
                position,
                consumedLength,
                hostName.ToString())
        {
            this.HostName = hostName;
        }

        public HostName HostName { get; }
    }
}
