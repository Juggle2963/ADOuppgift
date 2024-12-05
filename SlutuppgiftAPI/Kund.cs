using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlutuppgiftAPI;
public class Kund
{
	public Kund(int iD, string? kundnummer, string namn, string adress, string kontaktuppgift)
	{
		ID = iD;
		Kundnummer = kundnummer;
		Namn = namn ?? throw new ArgumentNullException(nameof(namn));
		Adress = adress ?? throw new ArgumentNullException(nameof(adress));
		Kontaktuppgift = kontaktuppgift ?? throw new ArgumentNullException(nameof(kontaktuppgift));
	}

	public int ID { get; set; }
	public string? Kundnummer { get; set; }
	public string Namn { get; set; } = string.Empty;
	public string Adress { get; set; } = string.Empty;
	public string Kontaktuppgift { get; set; } = string.Empty;

	public override string ToString()
	{
		return $"{ID} - {Kundnummer} - {Namn ?? "null"} - {Adress ?? "null"} - {Kontaktuppgift ?? "null"}";
	}


}
