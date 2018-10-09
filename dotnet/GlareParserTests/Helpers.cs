using System;

namespace Aethon.Glare
{
    public static class Helpers
    {
        // Simple function to simplify throw assertions
        // Instead of:
        //  new Action(() => subject.DoTheThing())
        //    .Should().Throw<Exception>();
        // You can use:
        //  TestAction(() => subject.DoTheThing())
        //    .Should().Throw<Exception>();
        public static Action TestAction(Action action) => action;
    }
}