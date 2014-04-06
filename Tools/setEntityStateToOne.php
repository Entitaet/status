<?php

	require_once("config.php");

	//Datenbank
	$db = @mysql_connect ($dbHost, $dbUser, $dbPassword) or die ("Es konnte keine Verbindung zum Datenbankserver hergestellt werden");

	//Datenbank auf UTF8 setzen
	mysql_query("SET NAMES 'UTF8'");

	// Datenbank auswählen
	mysql_select_db ($dbName, $db) or die("Die Datenbank \"$name\" konnte nicht ausgew&auml;hlt werden");

	$sql = "UPDATE status SET value = '1'  WHERE `key`='entities'";
	mysql_query($sql);

	//Datenbank schließen
	mysql_close($db);
?>