﻿namespace Gu.Settings.Core
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public class DirectoryInfoComparer : IEqualityComparer<DirectoryInfo>
    {
        private static readonly char[] TrimEnd = { '\\' };
        public static readonly DirectoryInfoComparer Default = new DirectoryInfoComparer();
        private static readonly StringComparer OrdinalIgnoreCaseComparer = StringComparer.OrdinalIgnoreCase;

        private DirectoryInfoComparer()
        {
        }

        public bool Equals(DirectoryInfo x, DirectoryInfo y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            return OrdinalIgnoreCaseComparer.Equals(x.FullName.TrimEnd(TrimEnd), y.FullName.TrimEnd(TrimEnd));
        }

        public int GetHashCode(DirectoryInfo obj)
        {
            Ensure.NotNull(obj, nameof(obj));
            return OrdinalIgnoreCaseComparer.GetHashCode(obj.FullName.TrimEnd(TrimEnd));
        }
    }
}