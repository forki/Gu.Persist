namespace Gu.Persist.Core
{
    public interface IRepositorySettings : IFileSettings
    {
        /// <summary>
        /// Gets the settings specifying how backups are handled.
        /// If null no backups are made.
        /// </summary>
        BackupSettings BackupSettings { get;  }

        /// <summary>
        /// Gets the file extension used when saving files.
        /// On successful save the file extension is replaced.
        /// </summary>
        string TempExtension { get;  }

        /// <summary>
        /// Gets if the repository keeps a cache of last saved/read bytes to use for comparing if instance has changes.
        /// </summary>
        bool IsTrackingDirty { get;  }

        /// <summary>
        /// Gets if the repository keeps a cache of instances saved/read. 
        /// Default is true which means that , setting to false gives new instance for each read.
        /// </summary>
        bool IsCaching { get; }

        bool SaveNullDeletesFile { get; }
    }
}