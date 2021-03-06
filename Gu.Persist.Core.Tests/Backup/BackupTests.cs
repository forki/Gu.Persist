namespace Gu.Persist.Core.Tests.Backup
{
    using System;
    using System.IO;

    using Gu.Persist.Core;

    using NUnit.Framework;

    public class BackupTests
    {
        protected readonly DirectoryInfo Directory;
        protected FileInfo File;
        protected FileInfo Backup;
        protected FileInfo BackupOneMinuteOld;
        protected FileInfo BackupOneHourOld;
        protected FileInfo BackupOneDayOld;
        protected FileInfo BackupOneMonthOld;
        protected FileInfo BackupOneYearOld;
        protected FileInfo[] TimestampedBackups;
        protected FileInfo SoftDelete;
        protected FileInfo OtherBackup;

        public BackupTests()
        {
            this.Directory = new DirectoryInfo(@"C:\Temp\Gu.Persist\" + this.GetType().Name);
            this.Directory.CreateIfNotExists();

            this.File = this.Directory.CreateFileInfoInDirectory("Meh.cfg");
            this.SoftDelete = this.File.WithAppendedExtension(FileHelper.SoftDeleteExtension);
            this.Backup = this.Directory.CreateFileInfoInDirectory("Meh.bak");
            var settings = new BackupSettings(this.Directory.FullName, BackupSettings.DefaultExtension, BackupSettings.DefaultTimeStampFormat, 3, 3);
            this.BackupOneMinuteOld = this.Backup.WithTimeStamp(DateTime.Now.AddMinutes(-1), settings);
            this.BackupOneHourOld = this.Backup.WithTimeStamp(DateTime.Now.AddHours(-1), settings);
            this.BackupOneDayOld = this.Backup.WithTimeStamp(DateTime.Now.AddDays(-1), settings);
            this.BackupOneMonthOld = this.Backup.WithTimeStamp(DateTime.Now.AddMonths(-1), settings);
            this.BackupOneYearOld = this.Backup.WithTimeStamp(DateTime.Now.AddYears(-1), settings);

            this.OtherBackup = this.Directory.CreateFileInfoInDirectory("Other.bak").WithTimeStamp(DateTime.Now.AddHours(1), settings);

            this.TimestampedBackups = new[]
                                      {
                                          this.BackupOneMinuteOld,
                                          this.BackupOneHourOld,
                                          this.BackupOneDayOld,
                                          this.BackupOneMonthOld,
                                          this.BackupOneYearOld
                                      };
        }

        [SetUp]
        public virtual void SetUp()
        {
            this.File.Delete();
            this.Backup.Delete();
            foreach (var backup in this.TimestampedBackups)
            {
                backup.Delete();
            }

            this.OtherBackup.Delete();
            this.OtherBackup.CreatePlaceHolder();
            this.SoftDelete.Delete();
        }

        [TearDown]
        public void TearDown()
        {
            this.File.Delete();
            this.Backup.Delete();

            foreach (var backup in this.TimestampedBackups)
            {
                backup.Delete();
            }

            this.OtherBackup.Delete();
            this.SoftDelete.Delete();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            this.Directory.Delete(recursive: true);
        }
    }
}