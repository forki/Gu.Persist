﻿#pragma warning disable GU0009 // Name the boolean parameter.
namespace Gu.Persist.Core.Tests.Backup
{
    using System;
    using System.IO;

    using Gu.Persist.Core.Backup;

    using NUnit.Framework;

    public class NullBackuperTests : BackupTests
    {
        private NullBackuper backuper;
        private DummySerializable dummy;

        [SetUp]
        public override void SetUp()
        {
            this.backuper = NullBackuper.Default;
            this.dummy = new DummySerializable(1);
        }

        [Test]
        public void BackupWhenNotExtsis()
        {
            AssertFile.Exists(false, this.File);
            AssertFile.Exists(false, this.Backup);

            this.backuper.BeforeSave(this.File);

            AssertFile.Exists(false, this.File);
            AssertFile.Exists(false, this.Backup);
        }

        [Test]
        public void TryRestoreWhenHasSoftDelete()
        {
            this.SoftDelete.Save(this.dummy);
            AssertFile.Exists(false, this.File);
            AssertFile.Exists(true, this.SoftDelete);

            Assert.IsTrue(this.backuper.CanRestore(this.File));
            Assert.IsTrue(this.backuper.TryRestore(this.File));

            Assert.AreEqual(this.dummy, this.File.Read<DummySerializable>());
            AssertFile.Exists(true, this.File);
            AssertFile.Exists(false, this.SoftDelete);
            AssertFile.Exists(false, this.Backup);
        }

        [Test]
        public void TryRestoreWhenHasSoftDeleteAndBackup()
        {
            this.Backup.Save(this.dummy);
            this.dummy.Value++;
            this.SoftDelete.Save(this.dummy);

            AssertFile.Exists(false, this.File);
            AssertFile.Exists(true, this.SoftDelete);
            AssertFile.Exists(true, this.Backup);

            Assert.IsTrue(this.backuper.CanRestore(this.File));
            Assert.IsTrue(this.backuper.TryRestore(this.File));

            Assert.AreEqual(this.dummy, this.File.Read<DummySerializable>());
            AssertFile.Exists(true, this.File);
            AssertFile.Exists(false, this.SoftDelete);
            AssertFile.Exists(true, this.Backup);
        }

        [Test]
        public void TryRestoreWhenHasBackupAndOriginal()
        {
            this.File.WriteAllText("File");
            this.SoftDelete.CreatePlaceHolder();
            this.Backup.CreatePlaceHolder();

            AssertFile.Exists(true, this.File);
            AssertFile.Exists(true, this.SoftDelete);
            AssertFile.Exists(true, this.Backup);

            Assert.Throws<InvalidOperationException>(() => this.backuper.TryRestore(this.File));

            AssertFile.Exists(true, this.File);
            AssertFile.Exists(true, this.SoftDelete);
            AssertFile.Exists(true, this.Backup);
        }

        [Test]
        public void TryRestoreWhenHasBackup()
        {
            this.Backup.Save(this.dummy);

            AssertFile.Exists(false, this.File);
            AssertFile.Exists(false, this.SoftDelete);
            AssertFile.Exists(true, this.Backup);

            Assert.IsFalse(this.backuper.CanRestore(this.File));
            Assert.IsFalse(this.backuper.TryRestore(this.File));

            AssertFile.Exists(false, this.File);
            AssertFile.Exists(false, this.SoftDelete);
            AssertFile.Exists(true, this.Backup);
        }

        [Test]
        public void TryRestoreWhenNoFiles()
        {
            AssertFile.Exists(false, this.File);
            AssertFile.Exists(false, this.SoftDelete);
            AssertFile.Exists(false, this.Backup);

            Assert.IsFalse(this.backuper.TryRestore(this.File));

            AssertFile.Exists(false, this.File);
            AssertFile.Exists(false, this.SoftDelete);
            AssertFile.Exists(false, this.Backup);
        }

        [Test]
        public void PurgeAll()
        {
            Assert.Inconclusive("do we want this?");
        }

        [Test]
        public void PurgeWhenNoFiles()
        {
            using (var lockedFile = this.LockedFile())
            {
                this.backuper.AfterSave(lockedFile);
            }

            AssertFile.Exists(false, this.BackupOneMinuteOld);
            AssertFile.Exists(false, this.BackupOneHourOld);
            AssertFile.Exists(false, this.BackupOneDayOld);
            AssertFile.Exists(false, this.BackupOneMonthOld);
            AssertFile.Exists(false, this.BackupOneYearOld);
            AssertFile.Exists(false, this.SoftDelete);
        }

        [Test]
        public void PurgeWhenHasSoftDelete()
        {
            this.File.CreatePlaceHolder();
            this.SoftDelete.CreatePlaceHolder();
            foreach (var backup in this.TimestampedBackups)
            {
                backup.CreatePlaceHolder();
            }

            using (var lockedFile = this.LockedFile())
            {
                this.backuper.AfterSave(lockedFile);
            }

            AssertFile.Exists(true, this.File);
            AssertFile.Exists(true, this.BackupOneMinuteOld);
            AssertFile.Exists(true, this.BackupOneHourOld);
            AssertFile.Exists(true, this.BackupOneDayOld);
            AssertFile.Exists(true, this.BackupOneMonthOld);
            AssertFile.Exists(true, this.BackupOneYearOld);
            AssertFile.Exists(false, this.SoftDelete);
        }

        [Test]
        public void PurgeDeletesSoftDeletesNoTimestamp()
        {
            this.File.CreatePlaceHolder();
            this.SoftDelete.CreatePlaceHolder();
            this.Backup.CreatePlaceHolder();
            using (var lockedFile = this.LockedFile())
            {
                this.backuper.AfterSave(lockedFile);
            }

            AssertFile.Exists(true, this.File);
            AssertFile.Exists(true, this.Backup);
            AssertFile.Exists(false, this.SoftDelete);
        }

        [Test]
        public void PurgeDeletesSoftDeletes()
        {
            foreach (var backup in this.TimestampedBackups)
            {
                backup.CreatePlaceHolder();
            }

            this.SoftDelete.CreatePlaceHolder();
            this.File.CreatePlaceHolder();
            this.Backup.CreatePlaceHolder();
            using (var lockedFile = this.LockedFile())
            {
                this.backuper.AfterSave(lockedFile);
            }

            AssertFile.Exists(true, this.File);
            AssertFile.Exists(true, this.Backup);
            AssertFile.Exists(false, this.SoftDelete);
        }

        [Test]
        public void Rename()
        {
            Assert.Inconclusive("Backups must be renamed when original file is renamed");
        }

        private LockedFile LockedFile() => Core.LockedFile.CreateIfExists(this.File, x => x.Open(FileMode.Open, FileAccess.Read, FileShare.Delete));
    }
}
