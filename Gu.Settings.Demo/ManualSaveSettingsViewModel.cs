﻿namespace Gu.Settings.Demo
{
    using System.Windows.Input;
    using RemoveItemBox;

    public class ManualSaveSettingsViewModel
    {
        public ManualSaveSettingsViewModel()
        {
            this.ManualSaveSetting = RepositoryVm.Instance.ManualSaveSetting;
            this.SaveCommand = new RelayCommand(_ => RepositoryVm.Instance.Save(this.ManualSaveSetting),
                _ => RepositoryVm.Instance.Repository.IsDirty(this.ManualSaveSetting));
        }

        public ManualSaveSetting ManualSaveSetting { get; }

        public ICommand SaveCommand { get; }
    }
}