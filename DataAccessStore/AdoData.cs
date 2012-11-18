using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;


namespace DataAccessStore
{
	public class Song
	{ }

	public class Album
	{
		// Bemerkung KST:
		// wir werden die Id's als String definieren um bzgl. der Datenbankanbindung flexibl zu sein
		//   - kannst du dich an die Ausführung von Dirk bzgl. uiid errinnern...

		public string albumId;
		public string interpretId;
		public string title = "";
		public string genre = "";
	}

	public class AdoData : IDataInterface
	{
		public List<Album> GetAllAlbum()
		{
			//SQL Abfrage
			string sql = "Select * from  Album";

			//Datenbankverbindung
			SqlConnection connection = DatenbankClass.Verbindung.GetSqlConnection();
			SqlCommand cmd = new SqlCommand(sql, connection);
			try
			{
				connection.Open();

				//Kommando ausführen
				SqlDataReader reader = cmd.ExecuteReader();

				//In dieser Methode wird anhand des Readers eine Liste von Albums erzeugt.

				return GetAlbumsFromReader(reader);
			}
			catch (SqlException ex)
			{

				throw new DataAccessException("Die Songs konnten nicht ausgelesen werden.", ex);

			}

			finally
			{
				if (connection.State != System.Data.ConnectionState.Closed)
				{
					connection.Close();

				}
			}

		}

		private List<Album> GetAlbumsFromReader(SqlDataReader reader)
		{
			List<Album> list = new List<Album>();

			if (reader == null || !reader.HasRows)
			{
				return list; //leere Liste zurückgeben
			}

			//Alle Zeilen der Ergebnismenge durchlaufen.
			while (reader.Read())
			{
				//Alle Spalten in einer Zeile der Ergebnismenge durchlaufen
				Album album = new Album();
				for (int i = 0; i < reader.FieldCount; i++)
				{
					switch (reader.GetName(i).ToLower())
					{
						case "albumId":
							album.albumId = reader.IsDBNull(i) ? null : reader.GetString(i);
							break;
						case "interpretID":
							album.interpretId = reader.IsDBNull(i) ? null : reader.GetString(i);
							break;
						case "title":
							//Die Tabelle erlaubt für diese Spalte auch Nullwerte,                             
							album.title = reader.IsDBNull(i) ? null : reader.GetString(i);
							break;
						case "genre":
							//Die Tabelle erlaubt für diese Spalte auch Nullwerte
							album.genre = reader.IsDBNull(i) ? null : reader.GetString(i);
							break;
					}
				}
				list.Add(album);
			}
			return list;
		}




		public  void SaveAlbum(Album album)
		{
			// Bemerkung KST:
			// <= 0 ?????
			// da albumId auf string umgestellt wurde, bitte entsprechend implementieren !!!

			//if (album.albumId <= 0)
			//{
			//	InsertAlbum(album);
			//}
			//else
			//{
			//	DeleteAlbum(album);
			//}
		}

		public  void UpdateAlbum(Album album)
		{
			string sql = "UPDATE Album SET title=@title, genre=@description WHERE id=@id";
			SqlConnection connection = DatenbankClass.Verbindung.GetSqlConnection();

			SqlCommand cmd = new SqlCommand(sql, connection);
			cmd.Parameters.AddWithValue("title", album.title);
			cmd.Parameters.AddWithValue("genre", album.genre);
			cmd.Parameters.AddWithValue("id", album.albumId);

			try
			{
				cmd.Connection.Open();
				cmd.ExecuteNonQuery();
			}
			catch (SqlException e1)
			{
				throw new DataAccessException("Fehler beim Auslesen der Albumdaten.", e1);
			}
			finally
			{
				if (cmd.Connection.State != System.Data.ConnectionState.Closed)
				{
					cmd.Connection.Close();
				}
			}

		}

		 public void InsertAlbum(Album album)
		{
			string sql = @"INSERT INTO Album(title, genre) 
                            VALUES(@title, @genre)";

			SqlConnection connection = DatenbankClass.Verbindung.GetSqlConnection();

			SqlCommand cmd = new SqlCommand(sql, connection);
			cmd.Parameters.AddWithValue("title", String.IsNullOrEmpty(album.title) ? string.Empty : album.title);
			cmd.Parameters.AddWithValue("genre", String.IsNullOrEmpty(album.genre) ? string.Empty : album.genre);

			try
			{
				cmd.Connection.Open();
				cmd.ExecuteNonQuery();
			}
			catch (SqlException e1)
			{
//				throw new DataAccessException(this, "Fehler beim Speichern der Albumdaten.", e1);
			}
			finally
			{
				if (cmd.Connection.State != System.Data.ConnectionState.Closed)
				{
					cmd.Connection.Close();
				}
			}


		}

		 public void DeleteAlbum(Album album)
		{
			string sql = @"DELETE FROM Album WHERE albumId=@albumId";
			SqlConnection connection = DatenbankClass.Verbindung.GetSqlConnection();
			SqlCommand cmd = new SqlCommand(sql, connection);
			cmd.Parameters.AddWithValue("albumId", album.albumId);

			try
			{
				cmd.Connection.Open();
				cmd.ExecuteNonQuery();
			}
			catch (SqlException e1)
			{
				throw new DataAccessException("Fehler beim Auslesen der Albumdaten.", e1);
			}
			finally
			{
				if (cmd.Connection.State != System.Data.ConnectionState.Closed)
				{
					cmd.Connection.Close();
				}
			}

		}




	}

}


