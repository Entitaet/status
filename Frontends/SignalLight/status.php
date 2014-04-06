<?php

	require_once("config.php");

	//Vars
	$serviceOnline = true;

	//Datenbank
	$db = @mysql_connect ($dbHost, $dbUser, $dbPassword) or $serviceOnline=false;

	if($serviceOnline==true)
	{
		//Datenbank auf UTF8 setzen
		mysql_query("SET NAMES 'UTF8'");

		// Datenbank auswählen
		mysql_select_db ($dbName, $db) or die("Die Datenbank \"$name\" konnte nicht ausgew&auml;hlt werden");

		$sql = "SELECT * FROM status WHERE `key`='entities'";

		$result = mysql_query($sql);
	
		$countPersons=0;
	
		if($result!=false)
		{
			$row = mysql_fetch_array($result);
			$countPersons=intval($row['value']);
		}
		else
		{
			$service_online=false;
		}

		//Datenbank schließen
		mysql_close($db);
	}

	//Ausgabe
	if($serviceOnline==true)
	{
		if($countPersons>0)
		{
			echo '<img class="statusImage" src="' . $statusURL . '/img/green.svg"/><div>Im Moment ist der Hackerspace besetzt und kann besucht werden.</div>';
		}
		else
		{
			echo '<img class="statusImage" src="' . $statusURL . '/img/red.svg"/><div>Im Moment ist der Hackerspace leider nicht besetzt.</div>';
		}
	}
	else
	{
		echo '<img class="statusImage" src="' . $statusURL . '/img/yellow.svg"/><div>Im Moment kann der Status des Hackerspaces nicht ermittelt werden.</div>';
	}
?>