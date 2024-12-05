using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlutuppgiftAPI;
public class Order
{
	public Order(int iD, string orderNummer, string orderDatum, int kunderID)
	{
		ID = iD;
		OrderNummer = orderNummer ?? throw new ArgumentNullException(nameof(orderNummer));
		OrderDatum = orderDatum ?? throw new ArgumentNullException(nameof(orderDatum));
		KunderID = kunderID;
	}

	public int ID { get; set; }
	public string OrderNummer { get; set; } = string.Empty;
	public string OrderDatum { get; set; } = string.Empty;
	public int KunderID { get; set; }

	public override string ToString()
	{
		return $"{ID} - {OrderNummer} - {OrderDatum} - {KunderID}";
	}
}
