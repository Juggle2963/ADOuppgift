using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlutuppgiftAPI;
public class UppdateraOrder
{
	//Poco för att uppdatera en Order, Låter inte användaren ändra alla fält i posten
	public UppdateraOrder(int id, string ordernummer, int produkterId, int antal)
	{
		Id = id;
		Ordernummer = ordernummer ?? throw new ArgumentNullException(nameof(ordernummer));
		ProdukterId = produkterId;
		Antal = antal;
	}

	public int Id { get; set; }
	public string Ordernummer { get; set; }
	public int ProdukterId { get; set; }
	public int Antal { get; set; }

	
}
