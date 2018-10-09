using FluentAssertions;
using FluentAssertions.Execution;
using Xunit;

namespace Aethon.Glare.Scanning
{
    public class ScanPositionUnitTests
    {
        [Fact]
        public void New_StoresValues()
        {
            var subject = new ScanPosition(1, 2, 3);

            using (new AssertionScope())
            {
                subject.Absolute.Should().Be(1);
                subject.Row.Should().Be(2);
                subject.Column.Should().Be(3);
            }
        }

        [Fact]
        public void Plus_With0_ReturnsSamePosition()
        {
            var subject = new ScanPosition(100, 20, 30);

            (subject + 0).Should().Be(subject);
        }

        [Fact]
        public void Plus_WithNonZero_ReturnsCorrectPosition()
        {
            var subject = new ScanPosition(100, 20, 30);

            (subject + 25).Should().Be(new ScanPosition(125, 20, 55));
        }
    }
}