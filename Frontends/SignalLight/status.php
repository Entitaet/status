<?php
	require_once("rest.php");
	require_once("config.php");

	//Vars
	$serviceOnline = true;

	$url=$apiURL . "entities/?token=" . $token;
	$ret=CallRESTAPI("GET", $url);
	
	$countPersons=(int)$ret;

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