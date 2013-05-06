using Core.Model;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Configuration;
using NHConfig = NHibernate.Cfg;

namespace Core.Persistence
{
	public class SessionFactory
	{
		private readonly static Lazy<ISessionFactory> _sessionFactory = new Lazy<ISessionFactory>(() =>
		{
			var connectionString = ConfigurationManager.ConnectionStrings["Context"].ConnectionString;

			var autoMap = AutoMap.AssemblyOf<Entity>()
				.Where(x => typeof(Entity).IsAssignableFrom(x))
				.Conventions.Add(new CompressedAttributeConvention());

			return Fluently.Configure()
				.Database(MsSqlConfiguration.MsSql2008.ConnectionString(connectionString))
				.Mappings(m => m.AutoMappings.Add(autoMap))
				.ExposeConfiguration(TreatConfiguration)
				.BuildSessionFactory();
		});

		public ISession OpenSession()
		{
			return _sessionFactory.Value.OpenSession();
		}

		private static void TreatConfiguration(NHConfig.Configuration configuration)
		{
			var update = new SchemaUpdate(configuration);
			update.Execute(false, true);
		}
	}
}
