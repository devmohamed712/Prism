using System;
using System.Collections.Generic;
using System.Text;
using Prism.DAL;

namespace Prism.Repository
{
	public class EmailLogsRepository : Repository<TblEmailLogs>, IEmailLogsRepository
	{
		public EmailLogsRepository(PrismContext context) : base(context) { }
	}
}
