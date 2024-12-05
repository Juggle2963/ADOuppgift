namespace Crud;

using Microsoft.Data.SqlClient;
using SlutuppgiftAPI;
internal class Program
{
	static void Main(string[] args)
	{
		Slutuppgift uppg = new Slutuppgift(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Slutuppgift;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");

		Kund kund = null;
		Produkt produkt = null;

		//KUNDER

		//LÄGG TILL KUND
		uppg.AddKund(new Kund(1, "order15", "Jonny", "Brevgatan 2", "0522-13252"));

		//SKRIV UT EN KUND, MED PK-ID FRÅN KUND TABELL
		Kund kund_Tre = uppg.GetKund(4);
		Console.WriteLine(kund_Tre);



		//SKRIVER UT ALLA KUNDER
		List<Kund> list = uppg.GetKunder();
		foreach (var item in list)
		{
			Console.WriteLine(item);
		}

		//UPDATE KUND
		int rader = uppg.UpDateKund(new Kund(7, "ord18", "Bobbo", "Postgatan 3", "0532-18502"));
		Console.WriteLine($"{rader} har uppdaterats");

		//DELETE KUND - tillåter endast att Kund PK tas bort om ingen order finns på Kund
		uppg.DeleteKund(7);

		//PRODUKT

		//LÄGG TILL PRODUKT
		uppg.AddProdukt(new Produkt(1, "trehjuling", 32));

		//SÖK PÅ Produkt ID
		Produkt produkt2 = uppg.GetProduktByID(3);
		Console.WriteLine(produkt2);

		//SKRIVER UT ALLA Produkter
		List<Produkt> list2 = uppg.GetProdukter();
		foreach (var item in list2)
		{
			Console.WriteLine(item);
		}

		//UPDATERA PRODUKT - int ID är ID på produkt som ska uppdateras - tillåter inte uppdatering om order på produkt finns 
		int rader2 = uppg.UpDateProdukt(new Produkt(6, "fyrhjuling", 2500));
		Console.WriteLine($"{rader2} rad har uppdaterats");

		//DELETE PRODUKT - tillåter inte att ta bort om produkt finns i order
		//Metoden tar en int ID på produkten som ska raderas
		int produktDeleted = uppg.DeletProdukt(6);
		Console.WriteLine($"{produktDeleted} har raderats");

		//ORDER

		//SKAPAR EN ORDER och lägger till en produkt
		//
		//Tar in två objekt -  första int(en) i båda objekten tilldelas automatiskt och kan sättas till vad som helst här.
		// ID i Order sätts i queryn eftersom det är en PK nyckel
		//OrderiD i Products2Order sätts i queryn till samma värde som PK ID i Order eftersom dessa relaterar till varandra
		
		uppg.AddOrder(new Order(1, "ord19", "2022-03-22", 6), new Products2Order(1, 1, 3));

		//SKAPAR och lägger till en produkt till en befintlig order - kollar om order existerar innan den läggs till
		uppg.AddProductOrder(new AddProduktToOrder(3, 1, 5));

		//VISAR ALLA ORDRAR - Skapade en poco klass med propertys. ch i queryn slog jag samman tre tabeller för att visa viktigaste
		//fälten och få med Kund, Order och produkter på ett snyggt sätt
		List<VisaOrdrar> visaOrdrar = uppg.GetOrdrar();
		foreach (var item in visaOrdrar)
		{
			Console.WriteLine(item);
		}




		//VISAR EN ORDER -tar in Order ID och visar denna order
		VisaOrdrar order = uppg.GetOrderById(7);
		Console.WriteLine(order);

		//UPPDATERAR en order - tar in Order ID som ska ändras - Nytt ordernummer - produkt som ska ändras - nytt antal av produkten
		//
		//Skapat en Poco klass som tar in de propertys som behövs
		uppg.UpDateOrder(new UppdateraOrder(2, "order20", 2, 4));


		//DELETE ORDER - Tar bort Order samt produkter kopplade till order i Products2Order - Tar in Order ID
		int deleted = uppg.DeleteOrder(2);
		Console.WriteLine($"{deleted} poster togs bort");










		//ORDER



		//ADD
		//uppg.AddOrder(new Order(2, "ord11", "2011-03-12", 6), new Products2Order(3, 2, 5));		//C*/



		//SKRIVER UT ALLA ORDRAR
		/*	List<VisaOrdrar> list = uppg.GetOrdrar();

			foreach (var item in list)                          //R
			{
				Console.WriteLine(item);
			}*/

		//SKRIVER UT EN ORDER VARS Order (ID) anges
		/*	VisaOrdrar? visaOrder = null;
			visaOrder = uppg.GetOrderById(6);                   //R

			Console.WriteLine(visaOrder);
	*/



		//DELETE ORDER
		//uppg.DeleteOrder(8);                                //D


	}
}
