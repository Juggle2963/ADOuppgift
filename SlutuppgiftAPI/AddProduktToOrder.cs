using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlutuppgiftAPI;
public class AddProduktToOrder
{
	public AddProduktToOrder(int id, int produkterId, int antal)
	{
		Id = id;
		ProdukterId = produkterId;
		Antal = antal;
	}

	public int Id { get; set; }
	public int ProdukterId { get; set; }
	public int Antal { get; set; }
}
