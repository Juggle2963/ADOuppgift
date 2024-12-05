using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlutuppgiftAPI;
public class VisaOrdrar
{
	//POCO KLASS - Används för att skriva ut ordrar med väsentlig info Från Tabellerna -> Kunder, Order, Products2Order och Produkter
	public VisaOrdrar(string kundnummer, string namn, string ordernummer, DateTime orderdatum, int kunderID, string produkter, decimal totalt)
	{
		Kundnummer = kundnummer ?? throw new ArgumentNullException(nameof(kundnummer));
		Namn = namn ?? throw new ArgumentNullException(nameof(namn));
		Ordernummer = ordernummer ?? throw new ArgumentNullException(nameof(ordernummer));
		Orderdatum = orderdatum;
		KunderID = kunderID;
		Produkter = produkter ?? throw new ArgumentNullException(nameof(produkter));
		Totalt = totalt;
	}

	public string Kundnummer { get; set; } = string.Empty;
	public string Namn { get; set; }
	public string Ordernummer { get; set; }
	public DateTime Orderdatum { get; set; }
	public int KunderID { get; set; }
	public string Produkter { get; set; }
	public decimal Totalt { get; set; }

	public override string ToString()
	{
		return $"{Kundnummer} - {Namn} - {Ordernummer} - {Orderdatum} - {KunderID} - {Produkter} - {Totalt}";
	}

	
}
