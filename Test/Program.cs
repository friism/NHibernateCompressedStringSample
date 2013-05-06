using Core.Model;
using Core.Persistence;
using NHibernate.Linq;
using System;
using System.Linq;

namespace Test
{
	public class Program
	{
		static void Main(string[] args)
		{
			var document = new Document { Text = "Foo" };
			using (var session = new SessionFactory().OpenSession())
			{
				using (var transaction = session.BeginTransaction())
				{
					session.SaveOrUpdate(document);
					transaction.Commit();
				}

				var retrievedDocument = session.Query<Document>().First();
				Console.WriteLine(retrievedDocument.Text);
				session.Delete(retrievedDocument);

				Console.WriteLine("press the any key");
				Console.ReadKey();
			}
		}
	}
}
