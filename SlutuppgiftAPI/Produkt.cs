using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlutuppgiftAPI;
public class Produkt
{
	 public int ID { get; set; }
	 public string ProduktNamn { get; set; }
	 public decimal Pris { get; set; }
	public Produkt(int iD, string produktnamn, decimal pris)
	{
		ID = iD;
		ProduktNamn = produktnamn;
		Pris = pris;
	}

	public override string ToString()
	{
		return $"{ID} - {ProduktNamn} - {Pris} ";
	}
}
