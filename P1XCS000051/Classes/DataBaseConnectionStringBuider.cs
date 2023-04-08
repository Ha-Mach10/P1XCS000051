using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Threading.Tasks;

using MySql.Data.MySqlClient;

namespace P1XCS000051
{
    class DataBaseConnectionStringBuider
    {
        /// <summary>
        /// 
        /// </summary>
        internal enum Selector : ushort
        {
            id                = 1,
            develop_number    = 2,
            develop_name      = 3,
            code_name         = 4,
            create_date       = 5,
            use_applications  = 6,
            version           = 7,
            revition_date     = 8,
            diversion_number  = 9,
            old_number        = 10,
            new_number        = 11,
            inheritenceNumber = 12,
            explanation       = 13,
            summary           = 14
        }
        internal string SchemaManegerConnectionString()
        {
            var defaultVariant = Properties.Settings.Default;
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
            builder.Server = defaultVariant.mySQLServerName;
            builder.UserID = defaultVariant.mySQLUserName;
            builder.Database = defaultVariant.mySQLDatabaseName;
            builder.PersistSecurityInfo = true;
            builder.Password = defaultVariant.mySQLPassword;

            string connentionString = builder.ToString();

            return connentionString;
        }
        private DataTable SearchMgtTableSetter(string command)
        {
            try
            {
                string conStr = SchemaManegerConnectionString();

                using (MySqlConnection conn = new MySqlConnection(conStr))
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(command, conn))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        internal DataTable SearchMgtTable(string one, string tow, string three, string four, string five, string six, string seven, string eight,
                                          string nine, string ten, string eleven,
                                          Selector selectorOne, Selector selectorTow, Selector selectorThree, Selector selectorFour,
                                          Selector selectorFive, Selector selectorSix, Selector selectorSeven, Selector selectorEight,
                                          Selector selectorNine, Selector selectorTen, Selector selectorEleven)
        {
            string command = $@"SELECT *
                                FROM `manager_codes`
                                WHERE `id` = {one}
                                AND `develop_number` = {tow}
                                AND ``= {three}
                                AND {selectorFour}` = {four}
                                AND {selectorFive}` = {five}
                                AND {selectorSix}` = {six}
                                AND {selectorSeven}` = {seven}
                                AND {selectorEight}` = {eight}
                                AND `` = {nine}
                                AND `explanation` = {ten}
                                AND `summary` = {eleven};";

            return SearchMgtTableSetter(command);
        }
    }
}
