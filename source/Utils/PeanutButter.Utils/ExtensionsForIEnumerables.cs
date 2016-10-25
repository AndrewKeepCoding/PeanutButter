﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeanutButter.Utils
{
    public static class ExtensionsForIEnumerables
    {
        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> toRun)
        {
            foreach (var item in collection)
                toRun(item);
        }

        public static async Task ForEach<T>(this IEnumerable<T> collection, Func<T, Task> toRun)
        {
            foreach (var item in collection)
                await toRun(item);
        } 

        public static void ForEach<T>(this IEnumerable<T> collection, Action<T, int> toRunWithIndex)
        {
            var idx = 0;
            collection.ForEach(o =>
            {
                toRunWithIndex(o, idx++);
            });
        } 

        public static async Task ForEach<T>(this IEnumerable<T> collection, Func<T, int, Task> toRunWithIndex)
        {
            var idx = 0;
            await collection.ForEach(async (o) =>
            {
                await toRunWithIndex(o, idx++);
            });
        }

        public static bool IsSameAs<T>(this IEnumerable<T> collection, IEnumerable<T> otherCollection)
        {
            if (collection == null && otherCollection == null) return true;
            if (collection == null || otherCollection == null) return false;
            var source = collection.ToArray();
            var target = otherCollection.ToArray();
            if (source.Count() != target.Count()) return false;
            return source.Aggregate(true, (state, item) => state && target.Contains(item));
        }

        public static string JoinWith<T>(this IEnumerable<T> collection, string joinWith)
        {
            var stringArray = collection as string[];
            if (stringArray == null)
            {
                if (typeof(T) == typeof(string))
                    stringArray = collection.ToArray() as string[];
                else
                    stringArray = collection.Select(i => i.ToString()).ToArray();
            }
            return string.Join(joinWith, stringArray);
        }

        public static bool IsEmpty<T>(this IEnumerable<T> collection)
        {
            if (collection == null) return true;
            return !collection.Any();
        }

        public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> collection)
        {
            return collection ?? new List<T>();
        }

        public static T[] And<T>(this IEnumerable<T> source, params T[] toAdd)
        {
            return source.Concat(toAdd).ToArray();
        }

        public static T[] ButNot<T>(this IEnumerable<T> source, params T[] toRemove)
        {
            return source.Except(toRemove).ToArray();
        }

        public static IEnumerable<T> Flatten<T>(this IEnumerable<IEnumerable<T>> collection)
        {
            return collection.SelectMany(o => o);
        }

        public static IEnumerable<TResult> SelectNonNull<TCollection,TResult>(this IEnumerable<TCollection> collection, 
                                                                              Func<TCollection, TResult?> grabber = null) where TResult: struct
        {
            return collection
                        .Select(grabber)
                        .Where(i => i.HasValue)
                        .Select(i => i.Value);
        }

        public static IEnumerable<TResult> SelectNonNull<TCollection,TResult>(this IEnumerable<TCollection> collection, 
                                                                              Func<TCollection, TResult> grabber) where TResult: class
        {
            return collection
                        .Select(grabber)
                        .Where(i => i != null)
                        .Select(i => i);
        }

        public static string AsText<T>(this IEnumerable<T> input, string delimiter = null)
        {
            return input.JoinWith(delimiter ?? Environment.NewLine);
        }

        public static bool HasUnique<T>(this IEnumerable<T> input, Func<T, bool> matcher)
        {
            var matches = input.Where(matcher);
            return matches.Count() == 1;
        }

        public static void TimesDo(this int howMany, Action toRun)
        {
            howMany.TimesDo(i => toRun());
        }

        public static void TimesDo(this int howMany, Action<int> toRun)
        {
            if (howMany < 0)
                throw new ArgumentException("TimesDo must be called on positive integer", nameof(howMany));
            for (var i = 0; i < howMany; i++)
                toRun(i);
        }

        public static T Second<T>(this IEnumerable<T> src)
        {
            return src.FirstAfter(1);
        } 

        public static T Third<T>(this IEnumerable<T> src)
        {
            return src.FirstAfter(2);
        } 

        public static T FirstAfter<T>(this IEnumerable<T> src, int toSkip)
        {
            return src.Skip(toSkip).First();
        }

        public static T FirstOrDefaultAfter<T>(this IEnumerable<T> src, int toSkip)
        {
            return src.Skip(toSkip).FirstOrDefault();
        } 

    }
}
