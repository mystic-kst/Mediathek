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
	{
        public Guid songID;
        public string songtitle;
        public Guid albumID;
    }

	public class Album
	{
		
        public Album() {}
        public Album(string Interpret, string Title, string Genre)
        {
            interpret = Interpret;
            title = Title;
            genre = Genre;
        }
        
        public Guid albumID;
		public string interpret;
		public string title;
		public string genre;
        public string imagepath;
        public int datum ;
        public List<Song> Songs;

        
	}

	public class AdoData : IDataInterface
	{
		public List<Album> GetAllAlbum()
		{
			//SQL Abfrage
			string sql = "Select *  from  Album";

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

				throw new DataAccessException("Die Alben konnten nicht ausgelesen werden.", ex);

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
				Album album = new Album();
                //Alle Spalten in einer Zeile der Ergebnismenge durchlaufen
				for (int i = 0; i < reader.FieldCount; i++)
				{
                    var test = reader.GetName(i).ToLower();
					switch (reader.GetName(i).ToLower())
					{  
                         
						case "albumid":
                            album.albumID = reader.GetGuid(i);
							break;
                        case "interpret":
                            album.interpret = reader.IsDBNull(i) ? null : reader.GetString(i);
                            break;
						case "title":
							//Die Tabelle erlaubt für diese Spalte auch Nullwerte,                             
							album.title = reader.IsDBNull(i) ? null : reader.GetString(i);
							break;
						case "genre":
							//Die Tabelle erlaubt für diese Spalte auch Nullwerte
							album.genre = reader.IsDBNull(i) ? null : reader.GetString(i);
							break;
                        case "imagepath":
                            album.imagepath = reader.IsDBNull(i) ? null : reader.GetString(i);
                            break;
                        case "datum":
                            //album.datum = reader.IsDBNull(i) ? null : reader.GetInt32(i);  // !!! Fehler
                            //album.datum = reader[i].GetType() != typeof(DBNull) ? reader.GetInt32(i): 0;
                            album.datum = reader.IsDBNull(i) ? 0 : (int)reader.GetInt32(i);
                            break;
					}
				}
				list.Add(album);
			}
			return list;
		}




		public  void SaveAlbum(Album album)
		{
			
			// da albumId auf string umgestellt wurde, bitte entsprechend implementieren !!!

            //if (album.albumID != String.Empty)
            //{
            //    InsertAlbum(album);
            //}
            //else
            //{
            //    DeleteAlbum(album);
            //}
		}

		public  void UpdateAlbum(Album album)
		{
            string sql = "UPDATE Album SET interpret=@interpret, title=@title, genre=@genre, datum=@datum WHERE albumID=@albumid";
			SqlConnection connection = DatenbankClass.Verbindung.GetSqlConnection();

			SqlCommand cmd = new SqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("interpret", album.interpret);
			cmd.Parameters.AddWithValue("title", album.title);
			cmd.Parameters.AddWithValue("genre", album.genre);
            //cmd.Parameters.AddWithValue("imagePath", album.imagepath);
            cmd.Parameters.AddWithValue("datum", album.datum);
            cmd.Parameters.AddWithValue("albumid", album.albumID);

			try
			{
				cmd.Connection.Open();
				cmd.ExecuteNonQuery();
			}
			catch (SqlException e1)
			{
				throw new DataAccessException(this,"Fehler beim Auslesen der Albumdaten.", e1);
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
			string sql = @"INSERT INTO Album(interpret, title, genre, imagepath, datum) 
                            VALUES(@interpret, @title, @genre, @imagepath, @datum)";

			SqlConnection connection = DatenbankClass.Verbindung.GetSqlConnection();

			SqlCommand cmd = new SqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("interpret", String.IsNullOrEmpty(album.genre) ? string.Empty : album.interpret);
			cmd.Parameters.AddWithValue("title", String.IsNullOrEmpty(album.title) ? string.Empty : album.title);            
			cmd.Parameters.AddWithValue("genre", String.IsNullOrEmpty(album.genre) ? string.Empty : album.genre);
            cmd.Parameters.AddWithValue("imagepath", String.IsNullOrEmpty(album.imagepath) ? string.Empty : album.imagepath);
            cmd.Parameters.AddWithValue("datum", album.datum);

			try
			{
				cmd.Connection.Open();
				cmd.ExecuteNonQuery();
			}
			catch (SqlException e1)
			{
				throw new DataAccessException(this, "Fehler beim Speichern der Albumdaten.", e1);
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
			string sql = @"DELETE Album WHERE albumID=@albumid";
            
			SqlConnection connection = DatenbankClass.Verbindung.GetSqlConnection();
			SqlCommand cmd = new SqlCommand(sql, connection);
			cmd.Parameters.AddWithValue("albumid", album.albumID);

			try
			{
				cmd.Connection.Open();
				cmd.ExecuteNonQuery();
			}
			catch (SqlException e1)
			{
				throw new DataAccessException("Fehler beim Löschen der Albumdaten.", e1);
			}
			finally
			{
				if (cmd.Connection.State != System.Data.ConnectionState.Closed)
				{
					cmd.Connection.Close();
				}
			}

		}

         public List<Song> GetSongsFromAlbum()
         {
             //SQL Abfrage
             string sql = "Select * from Song";
                 

             //Datenbankverbindung
             SqlConnection connection = DatenbankClass.Verbindung.GetSqlConnection();
             SqlCommand cmd = new SqlCommand(sql, connection);
             try
             {
                 connection.Open();

                 //Kommando ausführen
                 SqlDataReader reader = cmd.ExecuteReader();

                 //In dieser Methode wird anhand des Readers eine Liste von Albums erzeugt.

                 return GetSongsFromReader(reader);
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

         private List<Song> GetSongsFromReader(SqlDataReader reader)
            {
 	            List<Song> songlist = new List<Song>();

			    if (reader == null || !reader.HasRows)
			    {
				    return songlist; //leere Liste zurückgeben
			    }

			    //Alle Zeilen der Ergebnismenge durchlaufen.
			    while (reader.Read())
			    {				
				Song song = new Song();
                //Alle Spalten in einer Zeile der Ergebnismenge durchlaufen
				    for (int i = 0; i < reader.FieldCount; i++)
				    {
					    switch (reader.GetName(i).ToLower())
					    {  
                         
						case "songid":
                            song.songID = reader.GetGuid(i);
							break;
                        case "albumid":
                            song.albumID = reader.GetGuid(i);
                            break;
						case "songtitle":
							//Die Tabelle erlaubt für diese Spalte auch Nullwerte,                             
							song.songtitle = reader.IsDBNull(i) ? null : reader.GetString(i);
							break;
					    }
				    }
				songlist.Add(song);
			    }
			return songlist;
            }

         public void AlbumSongs(Album album)
         {
             string sql = "";
             SqlConnection connection = DatenbankClass.Verbindung.GetSqlConnection();

             SqlCommand cmd = new SqlCommand(sql, connection);
             cmd.Parameters.AddWithValue("interpret", album.interpret);
             cmd.Parameters.AddWithValue("title", album.title);
             cmd.Parameters.AddWithValue("genre", album.genre);
             cmd.Parameters.AddWithValue("albumid", album.albumID);

             try
             {
                 cmd.Connection.Open();
                 cmd.ExecuteNonQuery();
             }
             catch (SqlException e1)
             {
                 throw new DataAccessException(this, "Fehler beim Auslesen der Songdaten.", e1);
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


