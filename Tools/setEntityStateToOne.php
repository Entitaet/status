<?php
	require_once("rest.php");
	require_once("config.php");
	
	$url=$apiURL . "entities/1/?token=" . $token;
	CallRESTAPI("POST", $url);
?>