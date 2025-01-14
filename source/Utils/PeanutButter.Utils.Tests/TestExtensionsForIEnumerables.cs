﻿using System;
using System.Collections.Generic;
using System.Linq;
using NExpect;
using NUnit.Framework;
using static PeanutButter.RandomGenerators.RandomValueGen;
using static NExpect.Expectations;

// ReSharper disable ExpressionIsAlwaysNull
// ReSharper disable CollectionNeverUpdated.Local
// ReSharper disable PossibleMultipleEnumeration
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace PeanutButter.Utils.Tests
{
    [TestFixture]
    public class TestExtensionsForIEnumerables
    {
        [Test]
        public void ForEach_OperatingOnNullCollection_ShouldThrowArgumentNullException()
        {
            // This may seem pointless, but bear with me:
            //  I actually wanted ForEach to just do nothing when given a null collection,
            //  however System.Collections.Generic.List<T> has its own ForEach signature
            //  which doesn't even throw a valid ArgumentNullException -- it just barfs
            //  with a NullReferenceException. So if I made mine different, it would be
            //  inconsistent and surprising. And extension methods can't override actual
            //  methods. So, poo. This test then serves as documentation that I thought
            //  about it and chose consistency over function. You can, however, use
            //  the helper EmptyIfNull() to get the nicer behaviour:
            //  int[] foo = null;
            //  foo.EmptyIfNull().ForEach(i => {});
            //---------------Set up test pack-------------------
            int[] src = null;

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Expect(() => src.ForEach(i =>
            {
            })).To.Throw<NullReferenceException>();

            //---------------Test Result -----------------------
        }

        [Test]
        public void ForEach_OperatingOnCollection_ShouldRunActionOnEachElement()
        {
            //---------------Set up test pack-------------------
            var result = new List<int>();
            var src = new[]
            {
                GetRandomInt(),
                GetRandomInt(),
                GetRandomInt()
            };

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            src.ForEach(item => result.Add(item));

            //---------------Test Result -----------------------
            Expect(result)
                .To.Equal(src);
        }

        [Test]
        public void IsSameAs_OperatingOnCollection_WhenBothAreNull_ShouldReturnTrue()
        {
            //---------------Set up test pack-------------------
            List<int> first = null;
            List<int> second = null;

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = first.IsSameAs(second);

            //---------------Test Result -----------------------
            Expect(result)
                .To.Be.True();
        }

        [Test]
        public void IsSameAs_OperatingOnCollection_WhenOneIsNull_ShouldReturnFalse()
        {
            //---------------Set up test pack-------------------
            List<int> first = null;
            var second = new List<int>();

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = first.IsSameAs(second);

            //---------------Test Result -----------------------
            Expect(result)
                .To.Be.False();
        }

        [Test]
        public void IsSameAs_OperatingOnCollection_WhenBothAreEmpty_ShouldReturnTrue()
        {
            //---------------Set up test pack-------------------
            var first = new int[] { };
            var second = new List<int>();

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = first.IsSameAs(second);

            //---------------Test Result -----------------------
            Expect(result)
                .To.Be.True();
        }

        [Test]
        public void IsSameAs_OperatingOnCollection_WhenBothContainSameElements_ShouldReturnTrue()
        {
            //---------------Set up test pack-------------------
            var first = new[] { 1 };
            var second = new List<int>(new[] { 1 });

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = first.IsSameAs(second);

            //---------------Test Result -----------------------
            Expect(result)
                .To.Be.True();
        }

        [Test]
        public void IsSameAs_OperatingOnCollection_WhenBothContainSameElementsInDifferentOrder_ShouldReturnTrue()
        {
            //---------------Set up test pack-------------------
            var first = new[] { 1, 2 };
            var second = new List<int>(new[] { 2, 1 });

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = first.IsSameAs(second);

            //---------------Test Result -----------------------
            Expect(result)
                .To.Be.True();
        }

        [Test]
        public void IsSameAs_OperatingOnCollection_WhenBothContainDifferentElements_ShouldReturnFalse()
        {
            //---------------Set up test pack-------------------
            var first = new[] { 1 };
            var second = new List<int>(new[] { 2, 1 });

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = first.IsSameAs(second);

            //---------------Test Result -----------------------
            Expect(result)
                .To.Be.False();
        }

        [TestFixture]
        public class JoinWith
        {
            [Test]
            public void ShouldReturnEmptyStringForNullCollection()
            {
                // Arrange
                var collection = null as int[];
                // Act
                var result = collection.JoinWith("");
                // Assert
                Expect(result)
                    .To.Equal("");
            }

            [TestFixture]
            public class OperatingOnStringCollection
            {
                [TestFixture]
                public class WhenCollectionIsEmpty
                {
                    [Test]
                    public void ShouldReturnEmptyString()
                    {
                        //---------------Set up test pack-------------------
                        var src = new List<string>();

                        //---------------Assert Precondition----------------

                        //---------------Execute Test ----------------------
                        var result = src.JoinWith(",");

                        //---------------Test Result -----------------------
                        Expect(result)
                            .To.Be.Empty();
                    }
                }

                [TestFixture]
                public class WhenCollectionHasItems
                {
                    [Test]
                    public void ShouldReturnJoinedString()
                    {
                        // Arrange
                        var src = new[] { "a", "b", "c" };
                        var delimiter = ";";
                        var expected = "a;b;c";

                        // Act
                        var result = src.JoinWith(delimiter);
                        // Assert
                        Expect(result)
                            .To.Equal(expected);
                    }
                }
            }

            [TestFixture]
            public class OperatingOnNumericCollection
            {
                [TestFixture]
                public class WhenCollectionIsEmpty
                {
                    [Test]
                    public void ShouldReturnEmptyString()
                    {
                        //---------------Set up test pack-------------------
                        var src = new List<int>();

                        //---------------Assert Precondition----------------

                        //---------------Execute Test ----------------------
                        var result = src.JoinWith(",");

                        //---------------Test Result -----------------------
                        Expect(result)
                            .To.Be.Empty();
                    }
                }

                [TestFixture]
                public class WhenCollectionIsNotEmpty
                {
                    [Test]
                    public void ShouldReturnCollectionJoinedWithGivenDelimiter()
                    {
                        //---------------Set up test pack-------------------
                        var src = new[] { 1, 2, 3 };
                        var delimiter = GetRandomString(2, 3);
                        var expected = "1" + delimiter + "2" + delimiter + "3";

                        //---------------Assert Precondition----------------

                        //---------------Execute Test ----------------------
                        var result = src.JoinWith(delimiter);

                        //---------------Test Result -----------------------
                        Expect(result)
                            .To.Equal(expected);
                    }
                }
            }

            [TestFixture]
            public class OperatingOnObjects
            {
                [Test]
                public void ShouldReturnValuesConvertedToStringsJoinedWithDelimiter()
                {
                    // Arrange
                    var items = GetRandomArray<Person>(2, 3);
                    var delimiter = ";;";
                    var expected = "";
                    foreach (var item in items)
                    {
                        if (expected.Length > 0)
                        {
                            expected += delimiter;
                        }

                        expected += item.ToString();
                    }

                    // Act
                    var result = items.JoinWith(delimiter);
                    // Assert
                    Expect(result)
                        .To.Equal(expected);
                }

                [Test]
                public void ShouldNotBreakOnNullValues()
                {
                    // Arrange
                    var items = GetRandomArray<Person>(3, 4);
                    items[1] = null;
                    var delimiter = ";;";
                    var expected = "";
                    foreach (var item in items)
                    {
                        if (expected.Length > 0)
                        {
                            expected += delimiter;
                        }

                        expected += $"{item}";
                    }

                    // Act
                    var result = items.JoinWith(delimiter);
                    // Assert
                    Expect(result)
                        .To.Equal(expected);
                }

                public class Person
                {
                    public int Id { get; set; }
                    public string Name { get; set; }

                    public override string ToString()
                    {
                        return $"{Id}::{Name}";
                    }
                }
            }
        }

        [TestFixture]
        public class EmptyIfNull
        {
            [TestFixture]
            public class WhenCollectionIsNull
            {
                [Test]
                public void ShouldReturnNewEmptyCollection()
                {
                    //---------------Set up test pack-------------------
                    List<int> src = null;
                    //---------------Assert Precondition----------------

                    //---------------Execute Test ----------------------
                    var result1 = src.EmptyIfNull();
                    var result2 = src.EmptyIfNull();

                    //---------------Test Result -----------------------
                    Expect(result1)
                        .Not.To.Be.Null();
                    Expect(result1)
                        .To.Be.An.Instance.Of<int[]>();
                    Expect(result1)
                        .To.Be.Empty();
                    Expect(result2)
                        .To.Be.Empty();
                    Expect(result2)
                        .Not.To.Be(result1);
                }
            }

            [TestFixture]
            public class WhenCollectionIsNotNull
            {
                [Test]
                public void ShouldReturnOriginalCollection()
                {
                    // Arrange
                    var collection = GetRandomCollection<int>();
                    // Act
                    var result = collection.EmptyIfNull();
                    // Assert
                    Expect(result)
                        .To.Be(collection);
                }
            }
        }

        [TestFixture]
        public class And
        {
            [Test]
            public void ShouldReturnNewArrayWithAddedItems()
            {
                //---------------Set up test pack-------------------
                var src = new[] { 1, 2, 3 };
                var expected = new[] { 1, 2, 3, 4, 5 };

                //---------------Assert Precondition----------------

                //---------------Execute Test ----------------------
                var result = src.And(4, 5);

                //---------------Test Result -----------------------
                Expect(result)
                    .To.Equal(expected);
            }

            [Test]
            public void ShouldBeAbleToAcceptIEnumerable()
            {
                // Arrange
                var src = new[] { 1, 2, 3 };
                // Act
                var result = src.And(Generator());
                // Assert
                Expect(result)
                    .To.Equal(new[] { 1, 2, 3, 4, 5 });

                IEnumerable<int> Generator()
                {
                    yield return 4;
                    yield return 5;
                }
            }

            [Test]
            public void ShouldReturnNewArrayWithAllAddedItems()
            {
                //---------------Set up test pack-------------------
                var src = new[] { 1, 2, 3, 4, 5 };
                var expected = new[] { 1, 2, 3, 4, 5, 4, 5 };

                //---------------Assert Precondition----------------

                //---------------Execute Test ----------------------
                var result = src.And(4, 5);

                //---------------Test Result -----------------------
                Expect(result)
                    .To.Equal(expected);
            }

            [TestFixture]
            public class OperatingOnList
            {
                [Test]
                public void ShouldReturnTheList()
                {
                    // Arrange
                    var list = new List<string>();
                    // Act
                    var next = list.And("foo");
                    // Assert
                    Expect(next)
                        .Not.To.Be(list);
                    Expect(next)
                        .To.Equal(new[] { "foo" });
                }

                [Test]
                public void ShouldAcceptIEnumerable()
                {
                    // Arrange
                    var src = new List<int>(new[] { 1, 2, 3 });
                    // Act
                    var result = src.And(Generator());
                    // Assert
                    Expect(result)
                        .To.Equal(new[] { 1, 2, 3, 4, 5 });

                    IEnumerable<int> Generator()
                    {
                        yield return 4;
                        yield return 5;
                    }
                }
            }

            [TestFixture]
            public class OperatingOnListInterface
            {
                [Test]
                public void ShouldReturnTheList()
                {
                    // Arrange
                    var list = new List<string>() as IList<string>;
                    // Act
                    var next = list.And("foo");
                    // Assert
                    Expect(next)
                        .To.Be(list);
                    Expect(next)
                        .To.Equal(new[] { "foo" });
                }
            }

            [TestFixture]
            public class OperatingOnIEnumerable
            {
                IEnumerable<string> MakeEnumerable()
                {
                    yield return "foo";
                }

                [Test]
                public void ShouldReturnArray()
                {
                    // Arrange
                    var list = MakeEnumerable();
                    // Act
                    var result = list.And("another");
                    // Assert
                    Expect(result)
                        .To.Equal(new[] { "foo", "another" });
                    Expect(result)
                        .To.Be.An.Instance.Of<string[]>();
                }
            }
        }

        [Test]
        public void ButNot_OperatingOnArrayOfType_ShouldReturnNewArrayWithAddedItems()
        {
            //---------------Set up test pack-------------------
            var src = new[] { 1, 2, 3 };

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = src.ButNot(2);

            //---------------Test Result -----------------------
            Expect(result)
                .To.Equal(new[] { 1, 3 });
        }

        [Test]
        public void Flatten_GivenEmptyCollection_ShouldReturnEmptyCollection()
        {
            //---------------Set up test pack-------------------
            var input = new List<List<int>>();

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = input.Flatten();

            //---------------Test Result -----------------------
            Expect(result)
                .To.Be.Empty();
        }

        [Test]
        public void Flatten_GivenCollectionWithOneItemInSubCollection_ShouldReturnFlattened()
        {
            //---------------Set up test pack-------------------
            var input = new List<IEnumerable<int>>();
            var expected = GetRandomArray<int>(1, 1);
            input.Add(expected);

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = input.Flatten();

            //---------------Test Result -----------------------
            Expect(result)
                .To.Equal(expected);
        }

        [Test]
        public void Flatten_GivenCollectionWithMultipleItemsInMultipleSubCollections_ShouldReturnFlattened()
        {
            //---------------Set up test pack-------------------
            var input = new List<IEnumerable<int>>();
            var part1 = GetRandomArray<int>();
            var part2 = GetRandomArray<int>();
            var part3 = GetRandomArray<int>();
            var expected = new List<int>();
            expected.AddRange(part1);
            expected.AddRange(part2);
            expected.AddRange(part3);
            input.AddRange(new[] { part1, part2, part3 });

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = input.Flatten();

            //---------------Test Result -----------------------
            Expect(result)
                .To.Be.Equivalent.To(expected);
        }

        [Test]
        public void Second_WhenOnlyHaveOneItemInCollection_ShouldThrow()
        {
            //---------------Set up test pack-------------------
            var input = new[] { 1 };
            var expectedMessage = GetOutOfRangeMessage();

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Expect(() => input.Second())
                .To.Throw<InvalidOperationException>()
                .With.Message.Equal.To(expectedMessage);

            //---------------Test Result -----------------------
        }

        [Test]
        public void Second_WhenHave2OrMoreItemsInCollection_ShouldReturnSecond()
        {
            //---------------Set up test pack-------------------
            var collection = GetRandomArray<string>(2);
            var expected = collection.ToArray()[1];

            //---------------Assert Precondition----------------
            Assert.That(collection.Count(), Is.GreaterThanOrEqualTo(2));

            //---------------Execute Test ----------------------
            var item = collection.Second();

            //---------------Test Result -----------------------
            Expect(item)
                .To.Equal(expected);
        }


        [Test]
        public void Third_WhenOnlyHaveOneItemInCollection_ShouldThrow()
        {
            //---------------Set up test pack-------------------
            var input = new[] { 1 };
            var expectedMessage = GetOutOfRangeMessage();

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Expect(() => input.Second())
                .To.Throw<InvalidOperationException>()
                .With.Message.Equal.To(expectedMessage);

            //---------------Test Result -----------------------
        }

        [Test]
        public void Third_WhenHave2OrMoreItemsInCollection_ShouldReturnThird()
        {
            //---------------Set up test pack-------------------
            var collection = GetRandomArray<string>(3);
            var expected = collection.ToArray()[2];

            //---------------Assert Precondition----------------
            Expect(collection)
                .To.Contain.At.Least(3)
                .Items();

            //---------------Execute Test ----------------------
            var item = collection.Third();

            //---------------Test Result -----------------------
            Expect(item)
                .To.Equal(expected);
        }

        [Test]
        public void FirstAfter_OperatingOnInsufficientCollection_ShouldThrow()
        {
            //---------------Set up test pack-------------------
            var collection = GetRandomArray<int>(2, 5);
            var skip = GetRandomInt(6, 100);
            var expectedMessage = GetOutOfRangeMessage();

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Expect(() => collection.FirstAfter(skip))
                .To.Throw<InvalidOperationException>()
                .With.Message.Equal.To(expectedMessage);

            //---------------Test Result -----------------------
        }

        [Test]
        public void FirstAfter_GivenSkipZero_ShouldReturnFirstElement()
        {
            //---------------Set up test pack-------------------
            var collection = GetRandomCollection<int>(10, 20).ToArray();
            var expected = collection[0];

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = collection.FirstAfter(0);

            //---------------Test Result -----------------------
            Expect(result)
                .To.Equal(expected);
        }

        [Test]
        public void FirstAfter_OperatingOnSufficientCollection_ShouldReturnRequestedElement()
        {
            //---------------Set up test pack-------------------
            var collection = GetRandomCollection<int>(10, 20).ToArray();
            var skip = GetRandomInt(2, 8);
            var expected = collection[skip];

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = collection.FirstAfter(skip);

            //---------------Test Result -----------------------
            Expect(result)
                .To.Equal(expected);
        }

        [Test]
        public void FirstOrDefaultAfter_OperatingOnInsufficientCollectionOfInt_ShouldReturnDefaultForType()
        {
            //---------------Set up test pack-------------------
            var collection = GetRandomCollection<int>(2, 5);
            var skip = GetRandomInt(6, 100);
            var expected = default(int);

            //---------------Assert Precondition----------------
            Assert.That(skip, Is.GreaterThan(collection.Count()));

            //---------------Execute Test ----------------------
            var result = collection.FirstOrDefaultAfter(skip);

            //---------------Test Result -----------------------
            Expect(result)
                .To.Equal(expected);
        }

        public class SomeType
        {
        }

        [Test]
        public void FirstOrDefaultAfter_OperatingOnInsufficientCollectionOfComplexType_ShouldReturnDefaultForType()
        {
            //---------------Set up test pack-------------------
            var collection = GetRandomCollection<SomeType>(2, 5);
            var skip = GetRandomInt(6, 100);
            var expected = default(SomeType);

            //---------------Assert Precondition----------------
            Assert.That(skip, Is.GreaterThan(collection.Count()));

            //---------------Execute Test ----------------------
            var result = collection.FirstOrDefaultAfter(skip);

            //---------------Test Result -----------------------
            Expect(result)
                .To.Equal(expected);
        }


        [Test]
        public void FirstOrDefaultAfter_OperatingOnSufficientCollection_ShouldReturnRequestedElement()
        {
            //---------------Set up test pack-------------------
            var collection = GetRandomArray<int>(10, 20);
            var skip = GetRandomInt(2, 8);
            var expected = collection[skip];

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = collection.FirstOrDefaultAfter(skip);

            //---------------Test Result -----------------------
            Expect(result)
                .To.Equal(expected);
        }

        private string GetOutOfRangeMessage()
        {
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            var reference = Assert.Throws<InvalidOperationException>(() => new int[0].First());
            return reference.Message;
        }


        private class ItemWithNullableId
        {
            // ReSharper disable once MemberCanBePrivate.Local
            public int? Id { get; set; }

            public static ItemWithNullableId For(int? value)
            {
                return new ItemWithNullableId() { Id = value };
            }
        }

        [Test]
        public void SelectNonNull_GivenCollectionOfObjectsWithNullableInts_ShouldReturnOnlyNonNullValues()
        {
            //---------------Set up test pack-------------------
            var id1 = GetRandomInt();
            var id2 = GetRandomInt();
            var expected = new[] { id1, id2 };
            var input = new[]
            {
                ItemWithNullableId.For(id1),
                ItemWithNullableId.For(null),
                ItemWithNullableId.For(id2),
                ItemWithNullableId.For(null)
            };

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = input.SelectNonNull(o => o.Id);

            //---------------Test Result -----------------------
            CollectionAssert.AreEqual(expected, result);
            Expect(result)
                .To.Equal(expected);
        }


        public class Thing
        {
        }

        public class ItemWithNullableThing
        {
            public Thing Thing { get; set; }

            public static ItemWithNullableThing For(Thing thing)
            {
                return new ItemWithNullableThing() { Thing = thing };
            }
        }

        [Test]
        public void SelectNonNull_GivenCollectionOfObjectsWithNullableThings_ShouldReturnOnlyNonNullValues()
        {
            //---------------Set up test pack-------------------
            var id1 = GetRandom<Thing>();
            var id2 = GetRandom<Thing>();
            var expected = new[] { id1, id2 };
            var input = new[]
            {
                ItemWithNullableThing.For(id1),
                ItemWithNullableThing.For(null),
                ItemWithNullableThing.For(id2),
                ItemWithNullableThing.For(null)
            };

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = input.SelectNonNull(o => o.Thing);

            //---------------Test Result -----------------------
            Expect(result)
                .To.Equal(expected);
        }

        [Test]
        public void AsText_OperatingOnStringArray_ShouldReturnTextBlockWithEnvironmentNewlines()
        {
            //---------------Set up test pack-------------------
            var input = GetRandomArray<string>(2, 4);
            var expected = string.Join(Environment.NewLine, input);

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = input.AsText();

            //---------------Test Result -----------------------
            Expect(result)
                .To.Equal(expected);
        }

        public class SomethingWithNiceToString
        {
            public int Id { get; set; }
            public string Name { get; set; }

            public override string ToString()
            {
                return $"{Id} :: {Name}";
            }
        }

        [Test]
        public void AsText_OperatingOnArrayOfObjects_ShouldReturnTextBlockWithStringRepresentations()
        {
            //---------------Set up test pack-------------------
            var input = GetRandomList<SomethingWithNiceToString>(2, 4);
            var expected = string.Join(Environment.NewLine, input);

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = input.AsText();

            //---------------Test Result -----------------------
            Expect(result)
                .To.Equal(expected);
        }

        [Test]
        public void AsText_GivenAlternativeDelimiter_ShouldUseIt()
        {
            //---------------Set up test pack-------------------
            var input = GetRandomArray<int>(3, 6);
            var delimiter = GetRandomString(1);
            var expected = string.Join(delimiter, input);

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = input.AsText(delimiter);

            //---------------Test Result -----------------------
            Expect(result)
                .To.Equal(expected);
        }

        [Test]
        public void HasUnique_WhenNoMatchesForLambda_ShouldReturnFalse()
        {
            //---------------Set up test pack-------------------
            var input = new[] { 1, 2, 3 };

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = input.HasUnique(i => i == 4);

            //---------------Test Result -----------------------
            Expect(result)
                .To.Be.False();
        }

        [Test]
        public void HasUnique_WhenMultipleMatchesForLambda_ShouldReturnFalse()
        {
            //---------------Set up test pack-------------------
            var input = new[] { "a", "a", "b", "c" };

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = input.HasUnique(i => i == "a");

            //---------------Test Result -----------------------
            Expect(result)
                .To.Be.False();
        }

        [Test]
        public void HasUnique_WhenOneMatchesForLambda_ShouldReturnTrue()
        {
            //---------------Set up test pack-------------------
            var input = new[] { "a", "a", "b", "c" };

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var result = input.HasUnique(i => i == "b");

            //---------------Test Result -----------------------
            Expect(result)
                .To.Be.True();
        }


        [Test]
        public void TimesDo_OperatingOnZero_ShouldNotRunAction()
        {
            //---------------Set up test pack-------------------
            var calls = 0;
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            0.TimesDo(() => calls++);

            //---------------Test Result -----------------------
            Expect(calls)
                .To.Equal(0);
        }

        [Test]
        public void TimesDo_OperatingOnPositiveInteger_ShouldRunActionThatManyTimes()
        {
            //---------------Set up test pack-------------------
            var calls = 0;
            var howMany = GetRandomInt(1, 20);

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            howMany.TimesDo(() => calls++);

            //---------------Test Result -----------------------
            Expect(calls)
                .To.Equal(howMany);
        }

        [Test]
        public void TimesDo_OperatingOnNegativeInteger_ShouldThrowArgumentException()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var ex = Assert.Throws<ArgumentException>(() => (-1).TimesDo(() =>
            {
            }));

            //---------------Test Result -----------------------
            Expect(ex.Message)
                .To.Contain("positive integer");
            Expect(ex.ParamName)
                .To.Equal("howMany");
        }

        [Test]
        public void TimesDo_OperatingOnNegativeIntegerWithActionAcceptingIndex_ShouldThrowArgumentException()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var ex = Assert.Throws<ArgumentException>(() => (-1).TimesDo(i =>
            {
            }));

            //---------------Test Result -----------------------
            Expect(ex.Message)
                .To.Contain("positive integer");
            Expect(ex.ParamName)
                .To.Equal("howMany");
        }

        [Test]
        public void TimesDo_GivenActionAcceptingInteger_ShouldFeedIndex()
        {
            //---------------Set up test pack-------------------
            var howMany = GetRandomInt();
            var result = new List<int>();
            var expected = new List<int>();
            for (var i = 0; i < howMany; i++)
                expected.Add(i);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            howMany.TimesDo(i => result.Add(i));

            //---------------Test Result -----------------------
            Expect(result)
                .To.Equal(expected);
        }


        [Test]
        public void ForEach_GivenCollectionAndActionWithTwoParameters_ShouldPopulateSecondParameterWithIndex()
        {
            //---------------Set up test pack-------------------
            var input = GetRandomArray<int>(5, 15);
            var collectedIndexes = new List<int>();
            var collectedItems = new List<int>();
            var expectedIndexes = input.Select((o, i) => i).ToList();

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            input.ForEach((item, idx) =>
            {
                collectedItems.Add(item);
                collectedIndexes.Add(idx);
            });

            //---------------Test Result -----------------------
            Expect(collectedItems)
                .To.Equal(input);
            Expect(collectedIndexes)
                .To.Equal(expectedIndexes);
        }


        [TestFixture]
        public class FindDuplicates
        {
            [TestFixture]
            public class WithImpliedDiscriminator
            {
                [Test]
                public void OperatingOnEmptyCollection_ShouldReturnEmptCollection()
                {
                    // Arrange
                    var input = new int[0];
                    // Pre-Assert
                    // Act
                    var result = input.FindDuplicates();
                    // Assert
                    Expect(result)
                        .To.Be.Empty();
                }

                [Test]
                public void OperatingOnCollectionOfOne_ShouldReturnEmptCollection()
                {
                    // Arrange
                    var input = new[] { GetRandomInt() };
                    // Pre-Assert
                    // Act
                    var result = input.FindDuplicates();
                    // Assert
                    Expect(result)
                        .To.Be.Empty();
                }

                [Test]
                public void OperatingOnUniqueCollection_ShouldReturnEmptCollection()
                {
                    // Arrange
                    var input = GetRandomCollection<string>(10, 20).Distinct();
                    // Pre-Assert
                    // Act
                    var result = input.FindDuplicates();
                    // Assert
                    Expect(result)
                        .To.Be.Empty();
                }

                [Test]
                public void OperatingOnCollectionWithDuplicates_ShouldReturnDuplicatesOnceEach()
                {
                    // Arrange
                    var input = new[]
                    {
                        1, 2, 3, 3, 3, 2
                    };
                    // Pre-Assert
                    // Act
                    var result = input.FindDuplicates();
                    // Assert
                    Expect(result)
                        .To.Contain.Exactly(2).Items();
                    Expect(result)
                        .To.Be.Equivalent.To(new[] { 2, 3 });
                }
            }

            [TestFixture]
            public class None
            {
                [TestFixture]
                public class WithTestFn
                {
                    [Test]
                    public void ShouldReturnTrueForNullCollection()
                    {
                        // Arrange
                        var input = null as IEnumerable<int>;
                        // Pre-assert
                        // Act
                        var result = input.None(i => i == 1);
                        // Assert
                        Expect(result)
                            .To.Be.True();
                    }

                    [Test]
                    public void ShouldReturnTrueForEmptyCollection()
                    {
                        // Arrange
                        var input = new int[0];
                        // Pre-assert
                        // Act
                        var result = input.None(i => i == GetRandomInt());
                        // Assert
                        Expect(result)
                            .To.Be.True();
                    }

                    [Test]
                    public void ShouldReturnTrueWhenAllFail()
                    {
                        // Arrange
                        var input = new[] { 1, 2, 3 };
                        // Pre-assert
                        // Act
                        var result = input.None(i => i > 4);
                        // Assert
                        Expect(result)
                            .To.Be.True();
                    }

                    [Test]
                    public void ShouldReturnFalseWhenAnyOnePasses()
                    {
                        // Arrange
                        var input = new[] { 2, 4, 6, 7 };
                        // Pre-assert
                        // Act
                        var result = input.None(i => i % 2 == 1);
                        // Assert
                        Expect(result)
                            .To.Be.False();
                    }
                }

                [TestFixture]
                public class WithNoTestFn
                {
                    [Test]
                    public void ShouldReturnTrueForNullCollection()
                    {
                        // Arrange
                        var input = null as IEnumerable<int>;
                        // Act
                        var result = input.None();
                        // Assert
                        Expect(result)
                            .To.Be.True();
                    }

                    [Test]
                    public void ShouldBeTrueForEmptyCollection()
                    {
                        // Arrange
                        var input = new int[0];
                        // Act
                        var result = input.None();
                        // Assert
                        Expect(result)
                            .To.Be.True();
                    }

                    [Test]
                    public void ShouldBeFalseForAnySizeCollection()
                    {
                        // Arrange
                        var input = GetRandomArray<int>(1);
                        // Act
                        var result = input.None();
                        // Assert
                        Expect(result).To.Be.False();
                    }
                }
            }

            [TestFixture]
            public class WithProvidedDiscriminator
            {
                [Test]
                public void OperatingOnEmptyCollection_ShouldReturnEmptCollection()
                {
                    // Arrange
                    var input = new object[0];
                    // Pre-Assert
                    // Act
                    var result = input.FindDuplicates(o => o.GetHashCode());
                    // Assert
                    Expect(result).To.Be.Empty();
                }

                [Test]
                public void OperatingOnCollectionOfOne_ShouldReturnEmptCollection()
                {
                    // Arrange
                    var input = new[] { new { id = 1, name = "bob" } };
                    // Pre-Assert
                    // Act
                    var result = input.FindDuplicates(o => o.id);
                    // Assert
                    Expect(result).To.Be.Empty();
                }

                [Test]
                public void OperatingOnUniqueCollection_ShouldReturnEmptCollection()
                {
                    // Arrange
                    var idx = 0;
                    var input = GetRandomCollection(() => new
                    {
                        name = $"snowflake #{idx}",
                        id = idx++
                    });
                    // Pre-Assert
                    // Act
                    var result = input.FindDuplicates(o => o.id);
                    // Assert
                    Expect(result).To.Be.Empty();
                }

                [Test]
                public void OperatingOnCollectionWithDuplicates_ShouldReturnDuplicatesOnceEach()
                {
                    // Arrange
                    var input = new[]
                    {
                        new { id = 1, name = "bob" },
                        new { id = 2, name = "andrew" },
                        new { id = 3, name = "posh" },
                        new { id = 3, name = "scary" },
                        new { id = 3, name = "baby" },
                        new { id = 3, name = "sporty" },
                        new { id = 3, name = "ginger" },
                        new { id = 2, name = "dave" }
                    };
                    // Pre-Assert
                    // Act
                    var result = input.FindDuplicates(o => o.id);
                    // Assert
                    Expect(result).To.Contain.Exactly(2).Items();
                    var spiceGirls = result.Where(o => o.Key == 3).Select(o => o.Items).Single().Select(o => o.name);
                    Expect(spiceGirls).To.Contain.Exactly(5).Items();
                    Expect(spiceGirls)
                        .To.Be.Equivalent.To(new[]
                        {
                            "posh",
                            "scary",
                            "baby",
                            "sporty",
                            "ginger"
                        });
                    var adoringFans = result.Where(r => r.Key == 2).Select(r => r.Items).Single();
                    Expect(adoringFans).To.Contain.Exactly(2).Items();
                }
            }
        }

        [TestFixture]
        public class IsEmpty
        {
            [TestFixture]
            public class WhenCollectionIsNull
            {
                [Test]
                public void ShouldReturnTrue()
                {
                    // Arrange
                    var collection = null as int[];
                    // Act
                    var result = collection.IsEmpty();
                    // Assert
                    Expect(result).To.Be.True();
                }
            }

            [TestFixture]
            public class WhenCollectionIsEmpty
            {
                [Test]
                public void ShouldReturnTrue()
                {
                    // Arrange
                    var collection = new int[0];
                    // Act
                    var result = collection.IsEmpty();
                    // Assert
                    Expect(result)
                        .To.Be.True();
                }
            }

            [TestFixture]
            public class WhenCollectionIsNotEmpty
            {
                [Test]
                public void ShouldReturnFalse()
                {
                    // Arrange
                    var collection = GetRandomCollection<int>(1);
                    // Act
                    var result = collection.IsEmpty();
                    // Assert
                    Expect(result)
                        .To.Be.False();
                }
            }
        }

        [TestFixture]
        public class TrimmingStringCollections
        {
            [Test]
            public void TrimAll()
            {
                // Arrange
                var input = new[] { " one", "two ", " three " };
                var expected = new[] { "one", "two", "three" };
                // Act
                var result = input.Trim();
                // Assert
                Expect(result)
                    .To.Equal(expected);
            }

            [Test]
            public void TrimStartAll()
            {
                // Arrange
                var input = new[] { " one", "two ", " three " };
                var expected = new[] { "one", "two ", "three " };
                // Act
                var result = input.TrimStart();
                // Assert
                Expect(result)
                    .To.Equal(expected);
            }

            [Test]
            public void TrimEndAll()
            {
                // Arrange
                var input = new[] { " one", "two ", " three " };
                var expected = new[] { " one", "two", " three" };
                // Act
                var result = input.TrimEnd();
                // Assert
                Expect(result)
                    .To.Equal(expected);
            }
        }

        [TestFixture]
        public class Padding
        {
            [Test]
            public void PadLeftAll()
            {
                // Arrange
                var input = new[] { "one", "two", "three" };
                var expected = new[] { "   one", "   two", " three" };
                // Act
                var result = input.PadLeft(6);
                // Assert
                Expect(result)
                    .To.Equal(expected);
            }

            [Test]
            public void PadLeftAllDefaultsToPaddingToLongestString()
            {
                // Arrange
                var input = new[] { "one", "two", "three" };
                var expected = new[] { "  one", "  two", "three" };
                // Act
                var result = input.PadLeft();
                // Assert
                Expect(result)
                    .To.Equal(expected);
            }

            [Test]
            public void ShouldPadLeftWithProvidedChar()
            {
                // Arrange
                var input = new[] { "one", "two", "three" };
                var expected = new[] { "%%one", "%%two", "three" };
                // Act
                var result = input.PadLeft('%');
                // Assert
                Expect(result)
                    .To.Equal(expected);
            }

            [Test]
            public void PadRightAll()
            {
                // Arrange
                var input = new[] { "one", "two", "three" };
                var expected = new[] { "one   ", "two   ", "three " };
                // Act
                var result = input.PadRight(6);
                // Assert
                Expect(result)
                    .To.Equal(expected);
            }

            [Test]
            public void PadRightAllDefaultsToPaddingToLongestString()
            {
                // Arrange
                var input = new[] { "one", "two", "three" };
                var expected = new[] { "one  ", "two  ", "three" };
                // Act
                var result = input.PadRight();
                // Assert
                Expect(result)
                    .To.Equal(expected);
            }

            [Test]
            public void ShouldPadRightWithProvidedChar()
            {
                // Arrange
                var input = new[] { "one", "two", "three" };
                var expected = new[] { "one^^", "two^^", "three" };
                // Act
                var result = input.PadRight('^');
                // Assert
                Expect(result)
                    .To.Equal(expected);
            }

            [TestFixture]
            public class IsEqualTo
            {
                [Test]
                public void ShouldReturnTrueForTwoEmptyCollections()
                {
                    // Arrange
                    var a = new int[0];
                    var b = new int[0];
                    // Act
                    var result = a.IsEqualTo(b);
                    // Assert
                    Expect(result)
                        .To.Be.True();
                }

                [Test]
                public void ShouldReturnTrueForTwoEqualCollectionsOfSameType()
                {
                    // Arrange
                    var a = new[] { 1 };
                    var b = new[] { 1 };
                    // Act
                    var result = a.IsEqualTo(b);
                    // Assert
                    Expect(result)
                        .To.Be.True();
                }

                [Test]
                public void ShouldReturnFalseForMismatchedSize()
                {
                    // Arrange
                    var a = new[] { 1 };
                    var b = new[] { 1, 2 };
                    // Act
                    var result = a.IsEqualTo(b);
                    // Assert
                    Expect(result)
                        .To.Be.False();
                }

                [Test]
                public void ShouldReturnFalseForSameSizeDifferentValues()
                {
                    // Arrange
                    var a = new[] { 1 };
                    var b = new[] { 2 };
                    // Act
                    var result = a.IsEqualTo(b);
                    // Assert
                    Expect(result)
                        .To.Be.False();
                }
            }

            [TestFixture]
            public class IsEquivalentTo
            {
                [Test]
                public void ShouldReturnTrueWhenBothCollectionsAreNull()
                {
                    // Arrange
                    var left = null as int[];
                    var right = null as int[];
                    // Act
                    var result = left.IsEquivalentTo(right);
                    // Assert
                    Expect(result)
                        .To.Be.True();
                }

                [Test]
                public void ShouldReturnFalseWhenLeftIsNullAndRightIsNot()
                {
                    // Arrange
                    var left = null as int[];
                    var right = new[] { 1 };
                    // Act
                    var result = left.IsEquivalentTo(right);
                    // Assert
                    Expect(result)
                        .To.Be.False();
                }

                [Test]
                public void ShouldReturnFalseWhenLeftIsNotNullAndRightIsNull()
                {
                    // Arrange
                    var left = new[] { GetRandomInt() };
                    var right = null as int[];
                    // Act
                    var result = left.IsEquivalentTo(right);
                    // Assert
                    Expect(result)
                        .To.Be.False();
                }

                [Test]
                public void ShouldReturnTrueForTwoEmptyCollections()
                {
                    // Arrange
                    var a = new int[0];
                    var b = new int[0];
                    // Act
                    var result = a.IsEquivalentTo(b);
                    // Assert
                    Expect(result)
                        .To.Be.True();
                }

                [Test]
                public void ShouldReturnTrueForEqualCollections()
                {
                    // Arrange
                    var a = new[] { 1 };
                    var b = new[] { 1 };
                    // Act
                    var result = a.IsEquivalentTo(b);
                    // Assert
                    Expect(result)
                        .To.Be.True();
                }

                [Test]
                public void ShouldReturnTrueForOutOfOrderEquivalentCollections()
                {
                    // Arrange
                    var a = new[] { 1, 2 };
                    var b = new[] { 2, 1 };
                    // Act
                    var result = a.IsEquivalentTo(b);
                    // Assert
                    Expect(result)
                        .To.Be.True();
                }

                [Test]
                public void ShouldReturnTrueForOutOfOrderEquivalenceWithRepeatedValues()
                {
                    // Arrange
                    var a = new[] { 1, 2, 1 };
                    var b = new[] { 2, 1, 1 };
                    // Act
                    var result = a.IsEquivalentTo(b);
                    // Assert
                    Expect(result)
                        .To.Be.True();
                }

                [Test]
                public void ShouldReturnFalseForRepeatedValuesWithNoEquivalence()
                {
                    // Arrange
                    var a = new[] { 1, 2, 1, 1, 1 };
                    var b = new[] { 2, 1, 1, 2, 1 };
                    // Act
                    var result = a.IsEquivalentTo(b);
                    // Assert
                    Expect(result)
                        .To.Be.False();
                }

                [Test]
                public void ShouldReturnFalseForMismatchedSize()
                {
                    // Arrange
                    var a = new[] { 1, 2 };
                    var b = new[] { 1, 2, 3 };
                    // Act
                    var result = a.IsEquivalentTo(b);
                    // Assert
                    Expect(result)
                        .To.Be.False();
                }

                [Test]
                public void ShouldReturnFalseForSameSizeDifferentEntries()
                {
                    // Arrange
                    var a = new[] { 1 };
                    var b = new[] { 2 };
                    // Act
                    var result = a.IsEquivalentTo(b);
                    // Assert
                    Expect(result)
                        .To.Be.False();
                }
            }

            [TestFixture]
            public class FindOrAdd
            {
                [Test]
                public void SimpleTypesSimpleEquality()
                {
                    // Arrange
                    var expected = GetRandomString();
                    var collection = new List<string>();
                    // Act
                    var result1 = collection.FindOrAdd(expected);
                    var result2 = collection.FindOrAdd(expected);
                    // Assert
                    Expect(result1)
                        .To.Equal(expected);
                    Expect(result1)
                        .To.Equal(result2);
                    Expect(collection)
                        .To.Equal(new[] { expected });
                }

                [Test]
                public void ComplexTypesWithParameterlessConstructors()
                {
                    // Arrange
                    var collection = new List<Poco>();
                    var item1 = new Poco()
                    {
                        Id = 1,
                        Name = GetRandomString(12)
                    };
                    var item2 = new Poco()
                    {
                        Id = 1,
                        Name = GetRandomString(12)
                    };
                    // Act
                    var result1 = collection.FindOrAdd(
                        o => o.Id == item1.Id
                    );
                    result1.Id = 1;
                    var result2 = collection.FindOrAdd(
                        o => o.Id == item1.Id
                    );
                    // Assert
                    Expect(result1)
                        .Not.To.Be.Null();
                    Expect(result1)
                        .To.Be(result2);
                    // should have a new poco
                    Expect(result1)
                        .Not.To.Be(item1);
                }

                [Test]
                public void ShouldReturnExistingComplexItemViaEqualsOverride()
                {
                    // Arrange
                    var collection = new List<Poco>();
                    var item = GetRandom<Poco>();
                    collection.Add(item);
                    // Act
                    var result = collection.FindOrAdd(item);
                    // Assert
                    Expect(result)
                        .To.Be(item);
                }

                [Test]
                public void GivenMatcherAndGenerator()
                {
                    // Arrange
                    var collection = new List<Poco>();
                    var id = GetRandomInt();
                    var name = GetRandomString();
                    var expected = new
                    {
                        Id = id,
                        Name = name
                    };
                    // Act
                    var result1 = collection.FindOrAdd(
                        o => o.Id == id,
                        () => new Poco()
                        {
                            Id = id,
                            Name = name
                        }
                    );
                    var result2 = collection.FindOrAdd(
                        o => o.Id == id,
                        () => new Poco()
                        {
                            Id = id,
                            Name = name
                        }
                    );
                    // Assert
                    Expect(result1)
                        .To.Be(result2);
                    Expect(result1)
                        .To.Deep.Equal(expected);
                    Expect(collection)
                        .To.Contain.Only(1)
                        .Matched.By(o => o.DeepEquals(expected));
                }

                public class Poco
                {
                    public int Id { get; set; }
                    public string Name { get; set; }

                    public override bool Equals(object obj)
                    {
                        var asPoco = obj as Poco;
                        if (asPoco is null)
                        {
                            return false;
                        }

                        return asPoco.Id == Id && asPoco.Name == Name;
                    }

                    public override int GetHashCode()
                    {
                        // ReSharper disable NonReadonlyMemberInGetHashCode
                        return (Id, Name).GetHashCode();
                        // ReSharper enable NonReadonlyMemberInGetHashCode
                    }
                }
            }

            public static IEnumerable<(bool, int[], string[], int, char)> NonStringTestCases()
            {
                var start = new[] { 1, 11, 101 };
                var isLeft = true;
                var isRight = false;
                yield return (isLeft, start, new[] { "  1", " 11", "101" }, 3, ' ');
                yield return (isLeft, start, new[] { "%%%1", "%%11", "%101" }, 4, '%');
                yield return (isRight, start, new[] { "1  ", "11 ", "101" }, 3, ' ');
                yield return (isRight, start, new[] { "1&&&", "11&&", "101&" }, 4, '&');
            }

            [TestCaseSource(nameof(NonStringTestCases))]
            public void PaddingNonStrings(
                (bool isLeft, int[] start, string[] expected, int requiredLength, char padChar) testCase)
            {
                // Arrange
                var (isLeft, start, expected, requiredLength, padChar) = testCase;
                // Act
                var result = isLeft
                    ? start.PadLeft(requiredLength, padChar)
                    : start.PadRight(requiredLength, padChar);

                // Assert
                Expect(result)
                    .To.Equal(expected);
            }
        }
    }
}