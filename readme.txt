################### README FOR CORONA PROJEKT - TEAM CHARLIE ###################


#### En kort introduktion til nogle af de dependencies vi har implementeret i vores projekt


### NuGet packages som er nødvendige, og som skal downloades og implementeres
### i VS for at vores kode kan køre:
	# HtmlAgilityPack by ZZZ Projects
	# Z.Dapper.Plus by ZZZ Projects
	# System.Data.SqlClient by MicroSoft
	# System.IO.Compression by MicroSoft
	# System.IO.Compression.ZipFile by MicroSoft


### App.config ligger ikke i repo på GitHub men er tilføjet i afleveringsfilen med disse data:
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
    <connectionStrings>
        <add name="post" 
             connectionString=
             "Data Source=ServerName;               #### OBS dette skal være dit lokale databaseservernavn eller Localhost.
                 Initial Catalog=Charlie-CoronaDB;  #### Dette er navnet på den backup af databasen vi har tilføjet projektet.
                 Integrated Security=True"
             providerName="System.Data.SqlClient" />
    </connectionStrings>
</configuration>