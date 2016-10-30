﻿namespace Gu.Persist.Core
{
    public interface IFileSettings
    {
        /// <summary>
        /// Gets the path to where backups are saved.
        /// </summary>
        PathAndSpecialFolder Directory { get; }

        /// <summary>
        /// Gets the extension
        /// </summary>
        string Extension { get; }
    }
}