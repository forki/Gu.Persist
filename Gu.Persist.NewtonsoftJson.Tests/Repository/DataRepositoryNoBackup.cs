﻿namespace Gu.Persist.NewtonsoftJson.Tests.Repository
{
    using Gu.Persist.Core;
    using Gu.Persist.Core.Backup;

    using NUnit.Framework;

    public class DataRepositoryNoBackup : JsonRepositoryTests
    {
        [Test]
        public void BackuperIsNone()
        {
            Assert.AreSame(NullBackuper.Default, this.Repository.Backuper);
        }

        protected override IRepository Create()
        {
            var settings = new NewtonsoftJson.DataRepositorySettings(
                directory: this.TargetDirectory.FullName,
                jsonSerializerSettings: NewtonsoftJson.RepositorySettings.CreateDefaultJsonSettings(),
                isTrackingDirty: false,
                saveNullDeletesFile: false,
                backupSettings: null);

            return new DataRepository(settings);
        }
    }
}