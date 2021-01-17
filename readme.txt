################### READ FOR CORONA PROJEKT - TEAM CHARLIE ###################


#### En kort introduktion til nogle af de dependencies vi har implementeret i vores projekt


### NuGet packages som er nødvendige, og som skal downloades og implementeres
### i VS for at vores kode kan køre:
	# HtmlAgilityPack by ZZZ Projects
	# Z.Dapper.Plus by ZZZ Projects
	# System.Data.SqlClient by MicroSoft
	# System.IO.Compression by MicroSoft
	# System.IO.Compression.ZipFile by MicroSoft


### App.config skal oprettes lokalt på din egen PC i projektfolderen med følgende indhold:
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
    <connectionStrings>
        <add name="post" 
             connectionString=
             "Data Source=.;
                 Initial Catalog=Databasename;  #### OBS dette skal være din lokale database.
                 Integrated Security=True"
             providerName="System.Data.SqlClient" />
    </connectionStrings>
</configuration>