using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.IO;

namespace FileReader
{
    class Program
    {

        static void Main(string[] args)
        {
            StreamReader streamReader = new StreamReader(@"C:\Users\rwohl\Documents\data.txt");
            String str = "";
            List<Brewery> breweryList = new List<Brewery>();
            while((str = streamReader.ReadLine()) != null)
            {
                Brewery obj = new Brewery();
                var arr = str.Split('$');               
                obj.breweryId = Convert.ToInt32(arr[0]);
                obj.breweryName = arr[1];
                obj.city = arr[2];
                obj.state = arr[3];
                breweryList.Add(obj);
            }

            streamReader.Close();
            streamReader.Dispose();

            MySqlConnection con = new MySqlConnection("Server=127.0.0.1; database=beerdb; UID=db_user; password=Devenv0825; SslMode=none");
            con.Open();

            MySqlCommand cmd = con.CreateCommand();
            var breweryNameText = new MySqlParameter("name", MySqlDbType.VarChar);
            cmd.Parameters.Add(breweryNameText);
            var city = new MySqlParameter("city", MySqlDbType.VarChar);
            cmd.Parameters.Add(city);
            for (int i = 0; i < breweryList.Count; i++)
            {
                
                breweryNameText.Value = breweryList[i].breweryName;
                city.Value = breweryList[i].city;
                
                cmd.CommandText = "INSERT INTO breweries (brewery_id, brewery_name, city, state) VALUES (" + breweryList[i].breweryId + ", @name , @city , '" + breweryList[i].state + "')";

               

                cmd.ExecuteNonQuery();

                Console.WriteLine(breweryList[i].breweryId + " " + breweryList[i].breweryName + " " + breweryList[i].city + " " + breweryList[i].state);
            }

            con.Close();
            Console.ReadKey();
        }
    }
}
