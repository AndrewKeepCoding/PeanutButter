using System;

namespace PeanutButter.TestUtils.AspNetCore.Builders;

/// <summary>
/// A builder which is also capable of making randomized subjects
/// </summary>
/// <typeparam name="TBuilder"></typeparam>
/// <typeparam name="TSubject"></typeparam>
public abstract class RandomizableBuilder<TBuilder, TSubject> : Builder<TBuilder, TSubject>
    where TBuilder : RandomizableBuilder<TBuilder, TSubject>
{
    internal RandomizableBuilder(
        params Action<TSubject>[] actualizers
    ) : base(actualizers)
    {
    }

    /// <summary>
    /// Builds a random variant output artifact
    /// </summary>
    /// <returns></returns>
    public static TSubject BuildRandom()
    {
        return Create().Randomize().Build();
    }

    /// <summary>
    /// Derivatives must implement this so that BuildRandom can work
    /// </summary>
    /// <returns></returns>
    public abstract TBuilder Randomize();
}