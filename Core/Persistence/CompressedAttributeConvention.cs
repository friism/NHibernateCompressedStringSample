using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using System;

namespace Core.Persistence
{
	public class CompressedAttributeConvention : AttributePropertyConvention<CompressedAttribute>
	{
		protected override void Apply(CompressedAttribute attribute, IPropertyInstance instance)
		{
			if (instance.Property.PropertyType != typeof(string))
			{
				throw new ArgumentException();
			}

			instance.CustomType(typeof(CompressedString));
		}
	}
}
