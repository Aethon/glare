using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace Scratch
{
    public class UnitTest1
    {
        private readonly ITestOutputHelper output;

        public UnitTest1(ITestOutputHelper output)
        {
            this.output = output;
        }

        public UnitTest1(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public async Task Test1()
        {
            var task = Task.FromResult(18);
            var value = 0;
            await task.ContinueWith(t => value += t.Result * 10);
            await task.ContinueWith(t => value += t.Result * 100);

            value.Should().Be(1980);
        }

        enum One
        {
            First
        }

        enum Two
        {
            First
        }
        
        
        [Fact]
        public void EnumsAreNotTheSame()
        {
            One.First.Should().Be(Two.First);
        }

        public struct ParserKey<T> where T : Enum
        {
            private readonly Enum TypeKey;
            private readonly IList<object> StateItems;
            
            
        }
    }
}
