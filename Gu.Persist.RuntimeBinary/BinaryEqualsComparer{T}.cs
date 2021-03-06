﻿namespace Gu.Persist.RuntimeBinary
{
    using Gu.Persist.Core;

    /// <inheritdoc/>
    public class BinaryEqualsComparer<T> : SerializedEqualsComparer<T>
    {
        /// <summary> The default instance. </summary>
        public new static readonly BinaryEqualsComparer<T> Default = new BinaryEqualsComparer<T>();

        /// <inheritdoc/>
        protected override IPooledStream GetStream(T item)
        {
            return BinaryFile.ToStream(item);
        }
    }
}
