using System;
using System.Text;

namespace Launcher.Databases
{
	#region Interfaces

	/// <summary>
	/// Combines interfaces that are implemented by numeric content types.
	/// </summary>
	public interface IEquatableToNumberContent : IEquatable<IntegerContent>, IEquatable<DecimalContent>,
												 IEquatable<DoubleContent>
	{
	}

	/// <summary>
	/// Combines interfaces that are implemented by objects that can be equated to numbers.
	/// </summary>
	public interface IEquatableToNumber : IEquatable<long>, IEquatable<ulong>, IEquatable<int>,
										  IEquatable<uint>, IEquatable<decimal>, IEquatable<float>,
										  IEquatable<double>
	{
	}

	/// <summary>
	/// Combines interfaces that are implemented by objects that can be equated to text.
	/// </summary>
	public interface IEquatableToText : IEquatable<TextContent>, IEquatable<string>, IEquatable<StringBuilder>
	{
	}

	/// <summary>
	/// Combines interfaces that are implemented by objects that can be compared to numbers.
	/// </summary>
	public interface IComparableToNumber : IComparable<long>, IComparable<ulong>, IComparable<int>,
										   IComparable<uint>, IComparable<decimal>, IComparable<float>,
										   IComparable<double>
	{
	}

	/// <summary>
	/// Combines interfaces that are implemented by objects that can be compared to numbers that are encapsulated into
	/// numeric entry content objects.
	/// </summary>
	public interface IComparableToNumberContent : IComparable<IntegerContent>, IComparable<DecimalContent>,
												  IComparable<DoubleContent>
	{
	}

	/// <summary>
	/// Combines interfaces that are implemented by objects that can be compared to text.
	/// </summary>
	public interface IComparableToText : IComparable<string>, IComparable<TextContent>,
										 IComparable<StringBuilder>
	{
	}

	#endregion
}