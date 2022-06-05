﻿using System;

namespace TauCode.Parsing.LexicalTokens
{
    public class UriToken : TextToken
    {
        public UriToken(int position, int consumedLength, Uri uri)
            : base(position, consumedLength, uri.ToString())
        {
            this.Uri = uri;
        }

        public Uri Uri { get; }
    }
}
