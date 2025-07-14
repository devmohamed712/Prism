using System;
using System.Collections.Generic;
using System.Text;
using Prism.DAL;

namespace Prism.Repository
{
	public class LabsRepository : Repository<TblLabs>, ILabsRepository
	{
		public LabsRepository(PrismContext context) : base(context) { }
	}
}
