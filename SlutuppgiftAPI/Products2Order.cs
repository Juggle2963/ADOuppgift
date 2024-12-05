using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlutuppgiftAPI;
public class Products2Order
{
	public int OrderId { get; set; }
	public int ProdukterId { get; set; }
	public int Antal { get; set; }	
	public Products2Order(int orderId, int produkterId, int antal)
	{
		OrderId = orderId;
		ProdukterId = produkterId;
		Antal = antal;
	}
}
