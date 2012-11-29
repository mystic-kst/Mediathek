using System;
using System.Collections.Generic;
using DataAccessStore;
using DatenbankClass;
using System.Linq;

namespace MediathekConsole
{
    class Program
    {
        public static void Main(string[] args)
        {


            while (true)
            {
                Console.Clear();
                Console.WriteLine("Mediathek...\n");
                Console.WriteLine("1. Alle Alben anzeigen ->" + "\t" + "5. Album löschen ->");
                Console.WriteLine("2. Album mit Songs anzeigen ->" + "\t" + "6. Alben nach Genre ->");
                Console.WriteLine("3. Neues Album hinzufügen ->" + "\t" + "7. Alben nach Datum ->");
                Console.WriteLine("4. Änderungen ->" + "\t\t" +  "8. Song suchen ->");
                Console.WriteLine("9. Exit");


                string Eingabe = Console.ReadLine();
                if (Eingabe == "1")
                {
                    var test1 = new AdoData();
                    List<Album> list = test1.GetAllAlbum();

                    if (list != null)
                    {
                        foreach (Album album in list)
                        {
                            Console.WriteLine(album.interpret + "  " + album.title + "  " + album.genre + "  " + album.datum);

                        }
                    }
                }
                else if (Eingabe == "2")
                {

                    var test2 = new AdoData();
                    List<Album> list = test2.GetAllAlbum();
                    List<Song> songlist = test2.GetSongsFromAlbum();

                    Console.WriteLine("Album mit Songs: ");
                    Console.Write("Interpret: ");
                    string interpret = Console.ReadLine();
                    Console.Write("Albumtitel: ");
                    string title = Console.ReadLine();

                    var t = list.Where(p => p.interpret == interpret && p.title == title);
                    foreach (Album album in t)
                    {
                       
                            var test = from a in t
                                       join so in songlist on a.albumID equals so.albumID
                                       select new { Interpret = a.interpret, Album = a.title, Song = so.songtitle };

                            //foreach (var aso in test)
                            //{
                            //    Console.WriteLine(aso);
                            //}
                            foreach (var item in test)
                            {
                                Console.WriteLine("{0,-10} {1,-10} {2,-10}", item.Interpret, item.Album, item.Song);
                            }
                            Console.WriteLine("Songs in Album:  {0} ", test.Count());
                            Console.WriteLine(System.Environment.NewLine);
                        
                    }

                }
                else if (Eingabe == "3")
                {

                    Console.Write("Geben Sie den Interpreten ein: ");
                    string interpret = Console.ReadLine();
                    Console.Write("Geben Sie den Albumtitel ein: ");
                    string title = Console.ReadLine();
                    Console.Write("Geben Sie die Genre ein: ");
                    string genre = Console.ReadLine();

                    Album neuAlbum = new Album(interpret, title, genre);

                    var test3 = new AdoData();
                    test3.InsertAlbum(neuAlbum);

                }
                else if (Eingabe == "4")
                {
                    var test4 = new AdoData();
                    List<Album> list = test4.GetAllAlbum();

                    if (list != null)
                    {
                        foreach (Album album in list)
                        {
                            Console.WriteLine(album.interpret + "  " + album.title + "  " + album.genre);

                        }
                    }
                    Console.Write("Änderungen Album: ");
                    Console.Write("\nInterpret: ");
                    string interpret = Console.ReadLine();
                    Console.Write("Album Titel: ");
                    string title = Console.ReadLine();
                    Console.Write("Genre: ");
                    string genre = Console.ReadLine();

                    var test = list.Where(p => p.interpret == interpret && p.title == title && p.genre == genre);
                    foreach (Album album in test)
                    {
                        Console.WriteLine("Was wollen Sie Ändern? ");
                        Console.Write(album.interpret + " : ");
                        album.interpret = Console.ReadLine();               
                        Console.Write(album.title + " : ");
                        album.title = Console.ReadLine();
                        Console.Write(album.genre + " : ");
                        album.genre = Console.ReadLine();
                        //Console.Write(album.imagepath + " :");
                        //Console.ReadLine();
                        Console.Write(album.datum + " : ");
                        album.datum = Convert.ToInt32(Console.ReadLine());
                        
                        test4.UpdateAlbum(album);
                        
                    }

                }
                else if (Eingabe == "5")
                {

                    var test5 = new AdoData();
                    List<Album> list = test5.GetAllAlbum();
                    foreach (Album album in list)
                    {
                        Console.WriteLine(album.interpret + " " + album.title + "   " + album.genre);
                    }


                    Console.WriteLine("\nWelcher Album soll gelöscht werden:  ");
                    Console.Write("Geben Sie den Interpreten ein: ");
                    string interpret = Console.ReadLine();
                    Console.Write("Geben Sie den Albumtitel ein: ");
                    string title = Console.ReadLine();
                    Console.Write("Geben Sie die Genre ein: ");
                    string genre = Console.ReadLine();



                    var test = list.Where(p => p.interpret == interpret && p.title == title && p.genre == genre);
                    foreach (Album album in test)
                    {
                        //Console.WriteLine(album.interpret);
                        test5.DeleteAlbum(album);
                    }


                }
                else if (Eingabe == "6")
                {
                    var test6 = new AdoData();
                    List<Album> list = test6.GetAllAlbum();                   

                    Console.WriteLine("Geben Sie die Genre ein: ");
                    string genre = Console.ReadLine();

                    var test = list.Where(p => p.genre == genre);
                    foreach (Album album in test)
                    {
                        Console.WriteLine(album.title + "  " + album.interpret);
                        
                    }

                }
                else if (Eingabe == "7")
                {
                    var test7 = new AdoData();
                    List<Album> list = test7.GetAllAlbum();

                    Console.WriteLine("Alben von alt nach neu:");

                    var test = from l in list
                               orderby l.datum ascending
                               select l;

                    foreach (var album in test)
                    {
                        Console.WriteLine(album.datum + "  " + album.interpret + "  " + album.title);
                    }

                }
                else if (Eingabe == "8")
                {
                    var test8 = new AdoData();
                    List<Album> list = test8.GetAllAlbum();
                    List<Song> songlist = test8.GetSongsFromAlbum();

                    Console.WriteLine("Gesuchter Song: ");
                    string songtitle = Console.ReadLine();

                    var t = songlist.Where(p => p.songtitle == songtitle);
                    foreach (Song song in t)
                    {

                        var test = from so in t
                                   join a in list on so.albumID equals a.albumID
                                   select new { Interpret = a.interpret, Album = a.title, Song = so.songtitle };

                        //foreach (var aso in test)
                        //{
                        //    Console.WriteLine(aso);
                        //}
                        foreach (var item in test)
                        {
                            Console.WriteLine("Gefunden in: ");
                            Console.WriteLine("{0,-10} {1,-10} {2,-10}", item.Interpret, item.Album, item.Song);
                        }
                        

                    }
                    
                }
                else if (Eingabe == "9")
                {
                    System.Environment.Exit(0);
                }
                else
                {
                    Console.WriteLine("Fehler in der Eingabe...");
                }
                Console.ReadLine();

            } 
        }
            

            
        
    }
}
