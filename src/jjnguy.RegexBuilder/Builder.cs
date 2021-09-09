using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace jjnguy.RegexBuilder
{
  public record Builder(IEnumerable<ISegment> _pieces) : ISegment
  {
    public Builder() : this(new ISegment[0])
    {
    }

    public Builder(string text) : this(new ISegment[] { new BasicSegment(text) })
    {
    }

    public Builder Then(string text) => Then(new BasicSegment(text));
    public Builder Then(Func<Builder, Builder> groupBuilder) => Then(groupBuilder(new())._pieces);
    public Builder Then(params ISegment[] segments) => Then(segments as IEnumerable<ISegment>);
    public Builder Then(IEnumerable<ISegment> segments) => this with
    {
      _pieces = _pieces.Concat(segments)
    };

    public Builder ThenGroup(IEnumerable<ISegment> of) =>
      Then(ISegment.GroupStart).Then(of).Then(ISegment.GroupEnd);

    public Builder OneOrMore(string text) => OneOrMore(new BasicSegment(text));
    public Builder OneOrMore(Func<Builder, Builder> groupBuilder) => OneOrMore(groupBuilder(new())._pieces);
    public Builder OneOrMore(params ISegment[] of) => OneOrMore(of as IEnumerable<ISegment>);
    public Builder OneOrMore(IEnumerable<ISegment> of) => ThenGroup(of).Then(ISegment.OneOrMore);

    public Builder Digit() => Then("\\d");

    public string Value => string.Join("", _pieces.Select(p => p.Value));

    public Regex Build() => new Regex(Value);
  }

  public interface ISegment
  {
    public static readonly ISegment GroupStart = new BasicSegment("(");
    public static readonly ISegment GroupEnd = new BasicSegment(")");
    public static readonly ISegment ZeroOrOne = new BasicSegment("?");
    public static readonly ISegment OneOrMore = new BasicSegment("+");
    public static readonly ISegment ZeroOrMore = new BasicSegment("*");
    string Value { get; }
  }

  public class BasicSegment : ISegment
  {
    public string Value { get; private set; }

    public BasicSegment(string value)
    {
      // TODO: validate for only non-special characters
      Value = value;
    }
  }
}
