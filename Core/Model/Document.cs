using Core.Persistence;

namespace Core.Model
{
	public class Document : Entity
	{
		[Compressed]
		public virtual string Text { get; set; }
	}
}
