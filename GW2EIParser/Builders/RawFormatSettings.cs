﻿using System;
using System.Configuration;
using System.IO;

namespace GW2EIParser.Builders
{
    public class RawFormatSettings
    {
        public bool RawFormatTimelineArrays { get; }

        public RawFormatSettings(bool rawFormatTimelineArrays)
        {
            RawFormatTimelineArrays = rawFormatTimelineArrays;
        }
    }
}
