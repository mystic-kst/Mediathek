using System;
using System.Collections.Generic;
using DataAccessStore;
using DatenbankClass;

namespace MediathekConsole
{
    class Program
    {
        public static void Main(string[] args)
        {
	        var vault = new AdoData();
			List<Album> list = vault.GetAllAlbum();
	        
			if (list != null)
			{
				foreach (Album album in list)
				{
					Console.WriteLine(album.title);
				}
			}
            
            Console.ReadLine();
        }
    }
}
