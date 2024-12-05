using Microsoft.Data.SqlClient;
using System.Data;
using System;
using System.Windows.Input;

namespace SlutuppgiftAPI
{
	public class Slutuppgift
	{
		string _connectionString;
		public Slutuppgift(string connectionstring)
		{
			_connectionString = connectionstring;
		}

		/// <summary>
		/// Skapar och lägger till en Kund
		/// </summary>
		/// <param name="kund"></param>
		public void AddKund(Kund kund)
		{
			using SqlConnection connection = new SqlConnection(_connectionString);

			using SqlCommand command = connection.CreateCommand();


			command.CommandType = CommandType.StoredProcedure;
			command.CommandText = "AddKund";
			//In parameter
			command.Parameters.Add("@Kundnummer", SqlDbType.NVarChar, 16).Value = kund.Kundnummer == null ? DBNull.Value : kund.Kundnummer;
			command.Parameters.Add("@Namn", SqlDbType.NVarChar, 32).Value = kund.Namn;
			command.Parameters.Add("@Adress", SqlDbType.NVarChar, 32).Value = kund.Adress;
			command.Parameters.Add("@KontaktUppgift", SqlDbType.NVarChar, 32).Value = kund.Kontaktuppgift;
			//Out parameter
			command.Parameters.Add("@ID", SqlDbType.Int).Direction = ParameterDirection.Output;
			connection.Open();
			command.ExecuteNonQuery();
			kund.ID = (int)command.Parameters["@ID"].Value;
		}

		/// <summary>
		/// Returnerar en Kund med int ID om ID existerar i Kunder
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public Kund GetKund(int id)
		{
			Kund? kund = null;

			using SqlConnection connection = new SqlConnection(_connectionString);

			using SqlCommand command = connection.CreateCommand();

			command.CommandText = @"select * from Kunder where ID = @ID ";
			command.Parameters.Add("@ID", SqlDbType.Int).Value = id;

			connection.Open();
			using SqlDataReader reader = command.ExecuteReader();

			if (reader.Read())
			{
				kund = new Kund(
					id,
					reader.IsDBNull("Kundnummer") ? null : (string)reader["Kundnummer"],
					(string)reader["Namn"],
					(string)reader["Adress"],
					(string)reader["Kontaktuppgift"]
					);

			}
			return kund;
		}

		/// <summary>
		/// Returnerar en List av Kund som sedan kan användas
		/// </summary>
		/// <returns></returns>
		public List<Kund>? GetKunder()
		{
			List<Kund> list = new List<Kund>();


			SqlConnection connection = new SqlConnection(_connectionString);
			SqlCommand command = connection.CreateCommand();

			command.CommandType = CommandType.Text;
			command.CommandText = "select * from Kunder";

			connection.Open();
			SqlDataReader reader = command.ExecuteReader();

			while (reader.Read())
			{
				list.Add(new Kund(
					(int)reader["ID"],
					reader.IsDBNull("Kundnummer") ? null : (string)reader["Kundnummer"],
					(string)reader["Namn"],
					(string)reader["Adress"],
					(string)reader["Kontaktuppgift"]
					));
			}
			return list;
		}

		/// <summary>
		/// Tar Kund ID som parameter för vilken kund som ska uppdateras
		/// </summary>
		/// <param name="kund"></param>
		/// <returns> (int)rows affected)</returns>
		public int UpDateKund(Kund kund)
		{
			using SqlConnection connection = new SqlConnection(_connectionString);
			using SqlCommand command = connection.CreateCommand();

			command.CommandType = CommandType.StoredProcedure;
			command.CommandText = "UpdateKund";

			command.Parameters.Add("@ID", SqlDbType.Int).Value = kund.ID;
			command.Parameters.Add("@Kundnummer", SqlDbType.NVarChar, 16).Value = kund.Kundnummer; 
			command.Parameters.Add("@Namn", SqlDbType.NVarChar, 32).Value = kund.Namn == null ? DBNull.Value : kund.Namn;
			command.Parameters.Add("@Adress", SqlDbType.NVarChar, 32).Value = kund.Adress;
			command.Parameters.Add("@Kontaktuppgift", SqlDbType.NVarChar, 32).Value = kund.Kontaktuppgift;

			connection.Open();
			int i = command.ExecuteNonQuery();
			return i;

		}

		/// <summary>
		/// Tar bort Kund, tar Kund ID som parameter
		/// Kund kan inte raderas om den har en relation till order
		/// Order som är relaterad måste tas bort först
		/// </summary>
		/// <param name="iD"></param>
		public void DeleteKund(int iD)
		{
			using SqlConnection connection = new SqlConnection(_connectionString);
			using SqlCommand command = connection.CreateCommand();

			command.Parameters.Add("ID", SqlDbType.Int).Value = iD;

			command.CommandType = CommandType.StoredProcedure;
			command.CommandText = "DeleteKund";

			connection.Open();
			command.ExecuteNonQuery();
			//Tillåter inte att Kund tas bort om kund har en relation till en order
		}

		//Produkter
		/// <summary>
		/// 
		/// </summary>
		/// <param name="produkt"></param>
		/// <returns></returns>
		public void AddProdukt(Produkt produkt)
		{
			using SqlConnection connection = new SqlConnection(_connectionString);

			using SqlCommand command = connection.CreateCommand();


			command.CommandType = CommandType.StoredProcedure;
			command.CommandText = "AddProdukt";
			//In parameter
			command.Parameters.Add("@Produkt", SqlDbType.NVarChar, 64).Value = produkt.ProduktNamn;
			command.Parameters.Add("@Pris", SqlDbType.Money).Value = produkt.Pris;
			//Out parameter
			command.Parameters.Add("@ID", SqlDbType.Int).Direction = ParameterDirection.Output;
			connection.Open();
			command.ExecuteNonQuery();
			produkt.ID = (int)command.Parameters["@ID"].Value;
		}


		/// <summary>
		/// Tar in ID och returnerar Produkt om den existerar
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public Produkt GetProduktByID(int id)
		{
			Produkt? produkt = null;

			using SqlConnection connection = new SqlConnection(_connectionString);

			using SqlCommand command = connection.CreateCommand();

			command.CommandText = @"select * from Produkter where ID = @ID ";
			command.Parameters.Add("@ID", SqlDbType.Int).Value = id;

			connection.Open();
			using SqlDataReader reader = command.ExecuteReader();

			if (reader.Read())
			{
				produkt = new Produkt(
					id,
					(string)reader["Produkt"],
					(decimal)reader["Pris"]

					);

			}
			return produkt;
		}

		/// <summary>
		/// Skriver ut alla produkter i Produkter
		/// </summary>
		/// <returns></returns>
		public List<Produkt>? GetProdukter()
		{
			List<Produkt> list = new List<Produkt>();


			SqlConnection connection = new SqlConnection(_connectionString);
			SqlCommand command = connection.CreateCommand();

			command.CommandType = CommandType.Text;
			command.CommandText = "select * from Produkter";

			connection.Open();
			SqlDataReader reader = command.ExecuteReader();

			while (reader.Read())
			{
				list.Add(new Produkt(
					(int)reader["ID"],
					(string)reader["Produkt"],
					(decimal)reader["Pris"]
					));

			}
			return list;
		}
		/// <summary>
		/// Ange ID på produkt att uppdatera, om produkten finns i en order tillåts inte en update
		/// Returnerar en int med hur många rader som ändrats
		/// </summary>
		/// <param name="produkt"></param>
		public int UpDateProdukt(Produkt produkt)
		{
			using SqlConnection connection = new SqlConnection(_connectionString);
			using SqlCommand command = connection.CreateCommand();

			command.CommandType = CommandType.StoredProcedure;
			command.CommandText = "UpdateProdukt";

			command.Parameters.Add("@ID", SqlDbType.Int).Value = produkt.ID;
			command.Parameters.Add("@Produkt", SqlDbType.NVarChar, 64).Value = produkt.ProduktNamn;
			command.Parameters.Add("@Pris", SqlDbType.Money).Value = produkt.Pris;

			connection.Open();
			int i = command.ExecuteNonQuery();
			return i;
			
		}

		/// <summary>
		/// Tar bort produkt med Produkt ID som parameter
		/// </summary>
		/// <param name="iD"></param>
		/// <returns></returns>
		public int DeletProdukt(int iD)
		{
			using SqlConnection connection = new SqlConnection(_connectionString);
			using SqlCommand command = connection.CreateCommand();

			command.Parameters.Add("ID", SqlDbType.Int).Value = iD;

			command.CommandType = CommandType.StoredProcedure;
			command.CommandText = "DeleteProdukt";

			connection.Open();
			int i = command.ExecuteNonQuery();
			//Tillåter inte att produkt tas bort om den finns på en order
			return i;

		}

		/// <summary>
		/// skapar order och lägger till ett önskat antal av en produkt
		/// </summary>
		/// <param name="produkt"></param>
		public void AddOrder(Order order, Products2Order pto)
		{
			using SqlConnection connection = new SqlConnection(_connectionString);

			using SqlCommand command = connection.CreateCommand();


			command.CommandType = CommandType.StoredProcedure;
			command.CommandText = "AddOrder";
			//In parameter order
			command.Parameters.Add("@Ordernummer", SqlDbType.NVarChar, 16).Value = order.OrderNummer;
			command.Parameters.Add("@Orderdatum", SqlDbType.Date).Value = order.OrderDatum;
			command.Parameters.Add("@KunderID", SqlDbType.Int).Value = order.KunderID;
			//In parameter pto
			command.Parameters.Add("@ProdukterId", SqlDbType.Int).Value = pto.ProdukterId;
			command.Parameters.Add("@Antal", SqlDbType.Int).Value = pto.Antal;

			//Out parameter
			command.Parameters.Add("@ID", SqlDbType.Int).Direction = ParameterDirection.Output;
			//command.Parameters.Add("@OrderID", SqlDbType.Int).Direction = ParameterDirection.Output;
			connection.Open();
			command.ExecuteNonQuery();
			order.ID = (int)command.Parameters["@ID"].Value;
			
		}

		/// <summary>
		/// Lägger till en eller flera av en produkt, Tar in Orders ID, Produkt och Antal
		/// </summary>
		/// <param name=" ToOrder"></param>
		public void AddProductOrder(AddProduktToOrder ToOrder)
		{
			using SqlConnection connection = new SqlConnection(_connectionString);

			using SqlCommand command = connection.CreateCommand();


			command.CommandType = CommandType.StoredProcedure;
			command.CommandText = "AddProduct2Order";
		
			command.Parameters.Add("@ID", SqlDbType.Int).Value = ToOrder.Id;
			command.Parameters.Add("@ProdukterID", SqlDbType.Int).Value = ToOrder.ProdukterId;
			command.Parameters.Add("@Antal", SqlDbType.Int).Value = ToOrder.Antal;
			

			connection.Open();
			command.ExecuteNonQuery();
			ToOrder.Id = (int)command.Parameters["@ID"].Value;
		}

		/// <summary>
		/// Skriver ut alla ordrar med kundinfo orderinfo och beställda produkter
		/// </summary>
		/// <returns></returns>
		public List<VisaOrdrar>? GetOrdrar()
		{
			List<VisaOrdrar> list = new List<VisaOrdrar>();


			SqlConnection connection = new SqlConnection(_connectionString);
			SqlCommand command = connection.CreateCommand();

			command.CommandType = CommandType.Text;
			command.CommandText = "select * from Getallorders";

			connection.Open();
			SqlDataReader reader = command.ExecuteReader();

			while (reader.Read())
			{
				list.Add(new VisaOrdrar(
					reader.IsDBNull("Kundnummer") ? null : (string)reader["Kundnummer"],
					(string)reader["Namn"],
					(string)reader["Ordernummer"],
					(DateTime)reader["Orderdatum"],
					(int)reader["KunderID"],
					(string)reader["Produkter"],
					(decimal)reader["Totalt"]
					));
			}
			return list;
			
		}

		/// <summary>
		/// Tar in ID på Order och returnerar om order existerar
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public VisaOrdrar GetOrderById(int id)
		{
			VisaOrdrar visaOrder = null;

			using SqlConnection connection = new SqlConnection(_connectionString);

			using SqlCommand command = connection.CreateCommand();

			command.CommandText = @"select * from GetOrderID where orderID = @ID ";
			command.Parameters.Add("@ID", SqlDbType.Int).Value = id;

			connection.Open();
			using SqlDataReader reader = command.ExecuteReader();

			if (reader.Read())
			{
				visaOrder = new VisaOrdrar(
					reader.IsDBNull("Kundnummer") ? null : (string)reader["Kundnummer"],
					(string)reader["Namn"],
					(string)reader["Ordernummer"],
					(DateTime)reader["Orderdatum"],
					(int)reader["KunderID"],
					(string)reader["Produkter"],
					(decimal)reader["Totalt"]
					);

			}
			return visaOrder;
		}

		/// <summary>
		/// Update order, Uppdaterar Ordernummer i order och antal av en produkt tar in Order ID att ändra, nytt Ordernummer,
		/// produkt att ändra, nytt antal
		/// </summary>
		/// <param name="produkt"></param>
		public void UpDateOrder(UppdateraOrder uppdateraOrder)
		{
			using SqlConnection connection = new SqlConnection(_connectionString);
			using SqlCommand command = connection.CreateCommand();

			command.CommandType = CommandType.StoredProcedure;
			command.CommandText = "UpDateOrder";

			command.Parameters.Add("@ID", SqlDbType.Int).Value = uppdateraOrder.Id;
			command.Parameters.Add("@Ordernummer", SqlDbType.NVarChar, 16).Value = uppdateraOrder.Ordernummer;
			command.Parameters.Add("@ProdukterID", SqlDbType.Int).Value = uppdateraOrder.ProdukterId;
			command.Parameters.Add("@Antal", SqlDbType.Int).Value = uppdateraOrder.Antal;

			connection.Open();
			int i = command.ExecuteNonQuery();
		
		}


		/// <summary>
		/// Delete en order med Order(PK) tar en int som input
		/// </summary>
		/// <param name="iD"></param>
		public int DeleteOrder(int iD)
		{
			using SqlConnection connection = new SqlConnection(_connectionString);
			using SqlCommand command = connection.CreateCommand();

			command.Parameters.Add("ID", SqlDbType.Int).Value = iD;

			command.CommandType = CommandType.StoredProcedure;
			command.CommandText = "DeleteOrder";

			connection.Open();
			int i = command.ExecuteNonQuery();
			//Tar bort post i Products2Order först och sedan post i Order
			return i;

		}

	}
}
