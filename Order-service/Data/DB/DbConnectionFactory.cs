using Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DB
{
	public class DbConnectionFactory : IConnectionFactory
	{
		private readonly string _connectionString;

		public DbConnectionFactory(IConfiguration configuration)
		{
			_connectionString = configuration.GetConnectionString("DB")
				?? throw new InvalidOperationException("Connection string 'DB' is not configured.");
		}

		public IDbConnection CreateConnection()
		{
			return new NpgsqlConnection(_connectionString);
		}
	}
}
