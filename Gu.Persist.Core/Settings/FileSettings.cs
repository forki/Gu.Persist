﻿namespace Gu.Persist.Core
{
    using System;

    /// <summary>
    /// Metadata for reading and saving files.
    /// </summary>
    [Serializable]
    public class FileSettings : IFileSettings
    {
        public FileSettings(string directory, string extension)
        {
            Ensure.NotNull(directory, nameof(directory));
            Ensure.NotNull(extension, nameof(extension));
            this.Directory = directory;
            this.Extension = FileHelper.PrependDotIfMissing(extension);
        }

        public string Directory { get; }

        public string Extension { get; }
    }
}