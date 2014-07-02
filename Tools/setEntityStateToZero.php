<?php
	require_once("rest.php");
	require_once("config.php");

	$url=$apiURL . "entities/0/?token=" . $token;
	CallRESTAPI("POST", $url);
?>