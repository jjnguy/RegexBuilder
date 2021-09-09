using System;
using Xunit;

namespace jjnguy.RegexBuilder.Tests
{
  public class UnitTest1
  {
    [Fact]
    public void Test1()
    {
      var bldr = new Builder()
        .OneOrMore("g")
        .OneOrMore(bldr => bldr
          .Digit()
          .Digit());
    }
  }
}
