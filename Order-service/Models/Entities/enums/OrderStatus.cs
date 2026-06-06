using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities.enums
{
	public enum OrderStatus
	{
		Created = 0,
		Paid = 1,
		Collecting = 2,
		TransferredToDelivery = 3,
		Delivered = 4,
		Canceled = 5
	}
}
