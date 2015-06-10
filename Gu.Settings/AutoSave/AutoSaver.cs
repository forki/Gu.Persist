﻿namespace Gu.Settings
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Threading.Tasks;

    public class AutoSaver : IDisposable
    {
        private static readonly TaskFactory TaskFactory = new TaskFactory(new ConcurrentExclusiveSchedulerPair().ExclusiveScheduler);
        private readonly List<IDisposable> _subscriptions = new List<IDisposable>();
        private readonly IRepository _repository;
        private bool _disposed = false;

        public AutoSaver(IRepository repository)
        {
            _repository = repository;
        }

        public virtual IDisposable Add<T>(T item, IFileInfos fileInfos, IObservable<object> trigger)
            where T : class, INotifyPropertyChanged
        {
            var reference = new WeakReference<T>(item);
            var saver = new Saver(this, () => Save(reference, fileInfos));
            var subscription = trigger.Subscribe(saver);
            saver.Subscription = subscription;
            _subscriptions.Add(saver);
            return saver;
        }

        public event EventHandler<SaveEventArgs> Saving;

        public event EventHandler<SaveEventArgs> Saved;

        public event EventHandler<SaveErrorEventArgs> Error;

        /// <summary>
        /// Dispose(true); //I am calling you from Dispose, it's safe
        /// GC.SuppressFinalize(this); //Hey, GC: don't bother calling finalize later
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Protected implementation of Dispose pattern. 
        /// </summary>
        /// <param name="disposing">true: safe to free managed resources</param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                foreach (var subscription in _subscriptions.ToArray())
                {
                    if (subscription != null)
                    {
                        subscription.Dispose();
                    }
                }
            }

            // Free any unmanaged objects here. 
            _disposed = true;
        }

        protected virtual void Save<T>(WeakReference<T> itemReference, IFileInfos fileInfos)
            where T : class
        {
            T item;
            if (itemReference.TryGetTarget(out item))
            {
                OnSaving(new SaveEventArgs(item, fileInfos));
                TaskFactory.StartNew(() => Save(item, fileInfos));
            }
        }

        protected virtual void Save<T>(T item, IFileInfos fileInfos)
        {
            try
            {
                _repository.Save(item, fileInfos);
                OnSaved(new SaveEventArgs(item, fileInfos));
            }
            catch (Exception e)
            {
                OnError(new SaveErrorEventArgs(item, fileInfos, e));
            }
        }

        protected void VerifyDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().FullName);
            }
        }

        protected virtual void OnSaving(SaveEventArgs e)
        {
            var handler = Saving;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnSaved(SaveEventArgs e)
        {
            var handler = Saved;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnError(SaveErrorEventArgs e)
        {
            var handler = Error;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private sealed class Saver : IObserver<object>, IDisposable
        {
            private readonly AutoSaver _saver;
            private readonly Action _saveAction;

            public Saver(AutoSaver saver, Action saveAction)
            {
                _saver = saver;
                _saveAction = saveAction;
            }

            public IDisposable Subscription { get; internal set; }

            public void OnNext(object value)
            {
                _saveAction();
            }

            public void OnError(Exception error)
            {
                _saver._subscriptions.Remove(this);
            }

            public void OnCompleted()
            {
                _saver._subscriptions.Remove(this);
            }

            public void Dispose()
            {
                _saver._subscriptions.Remove(this);
                Subscription.Dispose();
            }
        }
    }
}