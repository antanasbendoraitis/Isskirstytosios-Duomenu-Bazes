using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Išskirstytosios_duomenų_bazės.Model
{
    class Book
    {
        public string Isbn { get; set; }
        public string URL { get; set; }
        public string Date { get; set; }
        public bool Signed { get; set; }
        public string Fk_Publishing_house { get; set; }
        public int Fk_Author { get; set; }
        public int Fk_Genre { get; set; }
        public int Fk_Format { get; set; }
        public int Fk_Language { get; set; }

        public Book()
        {

        }

        public Book(string isbn, string url, string date, bool signed, string fk_Publishing_house, int fk_Author, int fk_Genre, int fk_Format, int fk_Language)
        {
            Isbn = isbn;
            URL = url;
            Date = date;
            Signed = signed;
            Fk_Publishing_house = fk_Publishing_house;
            Fk_Author = fk_Author;
            Fk_Genre = fk_Genre;
            Fk_Format = fk_Format;
            Fk_Language = fk_Language;
        }

        public override string ToString()
        {
            return String.Format(" {0,8} | {1,90} | {2,4} | {3,5} | {4,50} | {5,6} | {6,6} | {7,6} | {8,6} |", Isbn, URL, Date, Signed, Fk_Publishing_house, Fk_Author, Fk_Genre, Fk_Format, Fk_Language);
        }


        public string CQLString()
        {
            return String.Format("SET url='{1}', date='{2}', signed={3}, fk_Publishing_house='{4}', fk_Author={5}, fk_Genre={6}, fk_Format={7}, fk_Language={8} WHERE isbn='{0}'", Isbn, URL, Date, Signed, Fk_Publishing_house, Fk_Author, Fk_Genre, Fk_Format, Fk_Language);
        }
    }
}
