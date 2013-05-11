using LZ4Sharp;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;
using System;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Core.Persistence
{
	public class CompressedString : IUserType
	{
		public object Assemble(object cached, object owner)
		{
			throw new NotImplementedException();
		}

		public object DeepCopy(object value)
		{
			if (value == null)
			{
				return null;
			}

			var @string = value as string;
			return string.Copy(@string);
		}

		public object Disassemble(object value)
		{
			throw new NotImplementedException();
		}

		public new bool Equals(object x, object y)
		{
			return x.Equals(y);
		}

		public int GetHashCode(object x)
		{
			return x.GetHashCode();
		}

		public bool IsMutable
		{
			get { return true; }
		}

		public object NullSafeGet(IDataReader rs, string[] names, object owner)
		{
			var value = rs[names[0]] as byte[];
			if (value != null)
			{
				var deCompressor = LZ4DecompressorFactory.CreateNew();
				return Encoding.UTF8.GetString(deCompressor.Decompress(value));
			}

			return null;
		}

		public void NullSafeSet(IDbCommand cmd, object value, int index)
		{
			var parameter = (DbParameter)cmd.Parameters[index];

			if (value == null)
			{
				parameter.Value = DBNull.Value;
				return;
			}

			var compressor = LZ4CompressorFactory.CreateNew();
			parameter.Value = compressor.Compress(Encoding.UTF8.GetBytes(value as string));
		}

		public object Replace(object original, object target, object owner)
		{
			throw new NotImplementedException();
		}

		public Type ReturnedType
		{
			get { return typeof(string); }
		}

		public SqlType[] SqlTypes
		{
			get { return new[] { new BinarySqlType(int.MaxValue) }; }
		}
	}
}
