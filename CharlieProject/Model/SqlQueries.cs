#region Coded by Martin

using CharlieProject.View.Windows;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace CharlieProject.Model
{
	class SqlQueries
	{
        //Connectionstring to database is fetched from App.config which is ignored by GitHub
        SqlConnection connString = new SqlConnection(ConfigurationManager.ConnectionStrings["path"].ConnectionString);

        public string[][] infectedDataIncoming;
        public string[][] infectedLevelsIncoming;
        public int i;
        
        public string infectedPercent = "";
        public float tested;
        public float infected;

        /// <summary>
        ///This method pulls data from the database for infectedDataIncoming dependent on Municipality selected in MainWindow.
        /// </summary>
        public string[] SelectAMunicipality(int muniID)
        {  
            try
            {
                SqlCommand infectedShow = new SqlCommand(@"SELECT TOP(7) ID,numberTested, numberInfected,municipalityID, date
                    FROM Infected WHERE municipalityID = @municipalityID ORDER BY date DESC", connString); //Query sent to database.
                infectedShow.Parameters.Add("@municipalityID", SqlDbType.Int).Value = muniID; //the Municipality to search for.
                connString.Open();
                SqlDataReader rd = infectedShow.ExecuteReader();

                //this is a way to get the length (number of rows) in rd...
                DataTable dt = new DataTable();
                dt.Load(rd);
                // ...to use for defining length of this array:
                infectedDataIncoming = new string[dt.Rows.Count][];

                //calls the Executereader again since data disappears from c# after dt.load.
                rd = infectedShow.ExecuteReader();

                //using an int to refer to current column...
                i = 0;
                //...and build the array with the values.
                foreach (var str in rd)
                {
                    infectedDataIncoming[i] = new string[]{rd.GetValue(1).ToString(),
                    rd.GetValue(2).ToString(),
                    rd.GetValue(3).ToString(),Convert.ToDateTime(rd.GetValue(4)).ToString("yyyy-MM-dd")};
                    i++;
                }
                rd.Close();

                //Need to pick data from the newest date.
                //SELECT-statement is build to have newest date in top row (index0), which is "easy" for us to pick out here:
                tested = float.Parse(infectedDataIncoming[0][0]);
                infected = float.Parse(infectedDataIncoming[0][1]);
                infectedPercent = string.Format("{0:0.00}", (infected / tested * 100));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kunne ikke hente data. Dette er fejlmeddelelsen:\n" + ex.Message);
            }
            finally
            {
                if (connString != null && connString.State == ConnectionState.Open) connString.Close();
            }

            string[] currentInfectionLevel = ShowInfectionLevelsWithBusinesses(float.Parse(infectedPercent));

            string[] output = new string[] { tested.ToString(), infected.ToString(), infectedPercent,
                currentInfectionLevel[0], currentInfectionLevel[1], currentInfectionLevel[2] };

            return output;
        }

        //Method to pick the other database-data we need.
        public string[] ShowInfectionLevelsWithBusinesses(float currentInfectionPercent)
        {
            string[] output = new string[3];
            try
            {
                SqlCommand infectedLevels = new SqlCommand(@"SELECT 
                    SubIndustry.industryGroupName, 
                    SubIndustry.subIndustryName 
                    FROM ContaminationLevelAndIndustryGroup
                    INNER JOIN Industry ON Industry.ID = ContaminationLevelAndIndustryGroup.industryID
                    INNER JOIN SubIndustry ON SubIndustry.subIndustryCode = Industry.subIndustryCode
                    WHERE contaminationLevelID IN
                    (SELECT ID FROM ContaminationLevel WHERE infectionLowerBorder <= @infectionLevel)
                    ORDER BY SubIndustry.industryGroupName"
                    , connString);
                infectedLevels.Parameters.Add("@infectionLevel", SqlDbType.Decimal).Value = currentInfectionPercent;
                connString.Open();
                SqlDataReader rdInfectedLevels = infectedLevels.ExecuteReader();

                //this is a way to get the length (number of rows) in rdInfectedLevels...
                DataTable dt = new DataTable();
                dt.Load(rdInfectedLevels);

                // ...to use for defining length of this array:
                infectedLevelsIncoming = new string[dt.Rows.Count][];
                rdInfectedLevels = infectedLevels.ExecuteReader();
                i = 0;
                foreach (var str in rdInfectedLevels)
                {
                    infectedLevelsIncoming[i] = new string[]{
                    rdInfectedLevels.GetValue(0).ToString(),
                    rdInfectedLevels.GetValue(1).ToString(),
                    };
                    i++;
                };
                //Can't call the reader with a new query before it's closed.
                rdInfectedLevels.Close();
                connString.Close();

                SqlCommand infLvl = new SqlCommand(@"SELECT TOP(1) * FROM ContaminationLevel 
				WHERE infectionLowerBorder<=@infectionLevel
				ORDER BY infectionLowerBorder DESC", connString);
                infLvl.Parameters.Add("@infectionLevel", SqlDbType.Decimal).Value = currentInfectionPercent;

                connString.Open();
                SqlDataReader rdInfLvl = infLvl.ExecuteReader();
                dt = new DataTable();
                dt.Load(rdInfLvl);
                output[0] = dt.Rows[0]["ID"].ToString();
                output[1] = dt.Rows[0]["contaminationLevelName"].ToString();
                
                rdInfLvl.Close();
                connString.Close();

                string closeBusinesses = "";

                for (int y = 0; y < infectedLevelsIncoming.Length; y++)
                {
                    for (int x = 0; x < infectedLevelsIncoming[y].Length; x++)
                    {
                        if (x == 0)
                        {
                            closeBusinesses += infectedLevelsIncoming[y][x] + ":\n";
                        }
                        else
                        {
                            closeBusinesses += infectedLevelsIncoming[y][x] + "\n\n";
                        }
                    }
                }
                output[2] = closeBusinesses;

            }
            catch (Exception ex)
            {
                popup win = new popup();
                win.popupWindow.Text = "Kunne ikke hente data. Dette er fejlmeddelelsen:\n" + ex.Message;
            }
            finally
            {
                if (connString != null && connString.State == ConnectionState.Open) connString.Close();
            }
            
            return output;
        }

        //Data to insert to Infected but in a "perfect" version of the app it would be inserted from the csv-files.
        public void InsertSomeData()
        {
            popup win = new popup();
            try
            {
                SqlCommand inserts = new SqlCommand(
                @"INSERT INTO Infected (numberTested,numberInfected,municipalityID,date)VALUES
                (1050, 1, 4, '2020-01-01'),(1060, 2, 5, '2020-01-01'),(1070, 3, 6, '2020-01-01'),
                (1180, 4, 4, '2020-02-01'),(1190, 5, 5, '2020-02-01'),(1200, 6, 6, '2020-02-01'),
                (1300, 7, 4, '2020-03-01'),(1310, 8, 5, '2020-03-01'),(1320, 9, 6, '2020-03-01'),
                (1500, 21, 4, '2020-04-01'),(1510, 22, 5,'2020-04-01'),(1520, 23, 6, '2020-04-01'),
                (1600, 14, 4, '2020-05-01'),(1610, 15, 5, '2020-05-01'),(1620, 16, 6, '2020-05-01'),
                (1700, 17, 4, '2020-06-01'),(1710, 18, 5, '2020-06-01'),(1720, 19, 6, '2020-06-01'),
                (1750, 21, 4, '2020-07-01'),(1760, 22, 5, '2020-07-01'),(1770, 23, 6, '2020-07-01'),
                (1800, 24, 4, '2020-08-01'),(1810, 25, 5, '2020-08-01'),(1820, 26, 6, '2020-08-01'),
                (1850, 27, 4, '2020-09-01'),(1860, 28, 5, '2020-09-01'),(1870, 29, 6, '2020-09-01'),
                (2018, 17, 4, '2020-10-01'), (2019, 18, 5, '2020-10-01'), (2020, 19, 6, '2020-10-01'),
                (2069, 21, 4, '2020-11-01'), (2069, 22, 5, '2020-11-01'), (2070, 23, 6, '2020-11-01'),
                (2118, 24, 4, '2020-12-01'), (2119, 25, 5, '2020-12-01'), (2120, 26, 6, '2020-12-01'),
                (2268, 27, 4, '2021-01-14'), (2269, 28, 5, '2021-01-14'), (2270, 29, 6, '2021-01-14')"
                , connString);
                connString.Open();
                inserts.ExecuteNonQuery();
                win.popupWindow.Text = "DATA ER SENDT!";
                win.popupWindow.FontSize = 40;
                win.Show();
            }
            catch (Exception ex)
            {                
                win.popupWindow.Text = "Kunne ikke hente data. Dette er fejlmeddelelsen:\n" + ex.Message;
                win.Show();
            }
            finally
            {
                if (connString != null && connString.State == ConnectionState.Open) connString.Close();
            }
        }
    }
	#endregion
}
