﻿namespace Gu.Persist.NewtonsoftJson
{
    using Gu.Persist.Core;

    /// <inheritdoc/>
    public sealed class JsonEqualsComparer<T> : SerializedEqualsComparer<T>
    {
        /// <summary>
        /// Returns the default instance.
        /// </summary>
        public new static readonly JsonEqualsComparer<T> Default = new JsonEqualsComparer<T>();

        /// <inheritdoc/>
        protected override IPooledStream GetStream(T item)
        {
            return File.ToStream(item);
        }
    }
}
