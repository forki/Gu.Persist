namespace Gu.Persist.NewtonsoftJson.Tests.Repository
{
    using Gu.Persist.Core;

    using NUnit.Framework;

    public class DataRepositoryWithDefaultSettings : JsonRepositoryTests
    {
        [Test]
        public void DefaultSettings()
        {
            Assert.AreEqual(".cfg", this.Settings.Extension);
            Assert.AreEqual(this.Directory.FullName, this.Settings.Directory);
            Assert.AreEqual(false, this.Settings.IsTrackingDirty);
            Assert.AreEqual(".bak", this.Settings.BackupSettings.Extension);
            Assert.AreEqual(int.MaxValue, this.Settings.BackupSettings.MaxAgeInDays);
            Assert.AreEqual(1, this.Settings.BackupSettings.NumberOfBackups);
            Assert.AreEqual(null, this.Settings.BackupSettings.TimeStampFormat);
        }

        protected override IRepository Create()
        {
            return new DataRepository(this.TargetDirectory);
        }
    }
}