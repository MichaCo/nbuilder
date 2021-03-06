﻿using NUnit.Framework;
using Rhino.Mocks;

namespace FizzWare.NBuilder.Tests.Unit.Picking
{
    [TestFixture]
    public class BetweenConstraintTests
    {
        private MockRepository mocks;
        private IUniqueRandomGenerator uniqueRandomGenerator;
        private int lower;
        private int upper;

        [SetUp]
        public void SetUp()
        {
            mocks = new MockRepository();
            uniqueRandomGenerator = mocks.StrictMock<IUniqueRandomGenerator>();
        }

        [Test]
        public void ShouldBeAbleToUseBetweenPickerConstraint()
        {
            lower = 1;
            upper = 5;
            var constraint = new BetweenConstraint(uniqueRandomGenerator, lower, upper);

            using (mocks.Record())
            {
                uniqueRandomGenerator.Expect(x => x.Next(lower, upper)).Return(2);
            }

            using (mocks.Ordered())
            {
                int end = constraint.GetEnd();

                Assert.That(end, Is.EqualTo(2));
            }
        }

        [Test]
        public void ShouldBeAbleToAddUpperUsingAnd()
        {
            using (mocks.Record())
            {
                uniqueRandomGenerator.Expect(x => x.Next(lower, upper)).Return(2);
            }

            var constraint = new BetweenConstraint(uniqueRandomGenerator, lower);
            constraint.And(upper);

            using (mocks.Playback())
            {
                constraint.GetEnd();
            }
        }
    }
}