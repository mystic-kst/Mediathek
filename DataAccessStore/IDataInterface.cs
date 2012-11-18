using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccessStore
{
	public interface IDataInterface
	{
		//Gibt eine Liste aller Alben
		List<Album> GetAllAlbum();

		//Speichert den Album in der Datenbank ab
		void SaveAlbum(Album album);

		//Löscht einen Album
		void DeleteAlbum(Album album);
	}
}