<?php
require 'flight/Flight.php';

//Database
Flight::set('databaseHost', 'localhost');
Flight::set('databaseName', 'status');
Flight::set('databaseUsername', 'root');
Flight::set('databasePassword', 'root');

//Register error handler	
//Flight::map('error', function(Exception $ex){
//    // Handle error
//    echo $ex->getTraceAsString();
//});

final class AccessRight
{
    const Read = 0;
    const Write = 1;
	const Create = 2;
	const Delete = 3;
}

function CheckAccessRights($token, $right)
{
	//Connect to database
	$mysqli = new mysqli(Flight::get('databaseHost'), Flight::get('databaseUsername'), Flight::get('databasePassword'), Flight::get('databaseName'));
	
	if ($mysqli->connect_errno) {
		Flight::halt(502, "Failed to connect to MySQL: (" . $mysqli->connect_errno . ") " . $mysqli->connect_error);
	}
	
	//Query data
	$sql = "SELECT * FROM token WHERE `token`='" . $token . "'";
	$result = $mysqli->query($sql);
	
	if($result->num_rows>0)
	{
		//Pick data
		$rowNumber=0;
		$result->data_seek($rowNumber);
		$row = $result->fetch_assoc();
		
		if($right==AccessRight::Read)
		{
			$rowValueAsString=$row['read'];
			if($rowValueAsString==1) return true;
		}
		else if($right==AccessRight::Write)
		{
			$rowValueAsString=$row['write'];
			if($rowValueAsString==1) return true;
		}
		else if($right==AccessRight::Create)
		{
			$rowValueAsString=$row['create'];
			if($rowValueAsString==1) return true;
		}
		else if($right==AccessRight::Delete)
		{
			$rowValueAsString=$row['delete'];
			if($rowValueAsString==1) return true;
		}
	}

	return false;
}

//Routing
Flight::route('GET /@name', function($name)
{	
	//Check Access rights
	$request = Flight::request();
	$token = Flight::request()->query['token'];

	$accessGranted=CheckAccessRights($token, AccessRight::Read);
	
	if($accessGranted==false)
	{
		Flight::halt(403);
	}
	
	//Connect to database
	$mysqli = new mysqli(Flight::get('databaseHost'), Flight::get('databaseUsername'), Flight::get('databasePassword'), Flight::get('databaseName'));
	
	if ($mysqli->connect_errno) {
		Flight::halt(502, "Failed to connect to MySQL: (" . $mysqli->connect_errno . ") " . $mysqli->connect_error);
	}
	
	//Query data
	$sql = "SELECT * FROM status WHERE `key`='" . $name . "'";
	$result = $mysqli->query($sql);
	
	//Pick data and returm result
	if($result->num_rows>0)
	{
		$rowNumber=0;
		$result->data_seek($rowNumber);
		$row = $result->fetch_assoc();
		$rowValueAsString=$row['value'];
	
		echo $rowValueAsString;
	}
	else
	{
		echo "PASS Exception";
		throw new RestException(204);
	}
});

Flight::route('POST /@name/@id', function($name, $id)
{	
	//Check Access rights
	$request = Flight::request();
	$token = Flight::request()->query['token'];

	$accessGranted=CheckAccessRights($token, AccessRight::Write);
	
	if($accessGranted==false)
	{
		Flight::halt(403);
	}	
	
	//Connect to database
	$mysqli = new mysqli(Flight::get('databaseHost'), Flight::get('databaseUsername'), Flight::get('databasePassword'), Flight::get('databaseName'));
	
	if ($mysqli->connect_errno) 
	{
		Flight::halt(502, "Failed to connect to MySQL: (" . $mysqli->connect_errno . ") " . $mysqli->connect_error);
	}	
	
	//Update value
	$dbName=Flight::get('databaseName');
	$sql="UPDATE {$dbName} SET value='{$id}' WHERE `key`='{$name}'";	

	if(!$mysqli->query($sql))
	{	
		Flight::halt(502, "Query failed: (" . $mysqli->errno . ") " . $mysqli->error);
	}
});

Flight::route('PUT /@name/@id', function($name, $id)
{	
	//Check Access rights
	$request = Flight::request();
	$token = Flight::request()->query['token'];

	$accessGranted=CheckAccessRights($token, AccessRight::Create);
	
	if($accessGranted==false)
	{
		Flight::halt(403);
	}	
	
	//Connect to database
	$mysqli = new mysqli(Flight::get('databaseHost'), Flight::get('databaseUsername'), Flight::get('databasePassword'), Flight::get('databaseName'));
	
	if ($mysqli->connect_errno) 
	{
		Flight::halt(502, "Failed to connect to MySQL: (" . $mysqli->connect_errno . ") " . $mysqli->connect_error);
	}	
	
	//Check if key exist
	//Query data
	$sql = "SELECT * FROM status WHERE `key`='" . $name . "'";
	$result = $mysqli->query($sql);
	
	if($result->num_rows>0)
	{
		Flight::halt(409); //Resource exist
	}
	
	//Create value
	$dbName=Flight::get('databaseName');
	$sql="INSERT INTO {$dbName} (`key`, `value`) VALUES ('{$name}', '{$id}')";	

	if(!$mysqli->query($sql))
	{	
		Flight::halt(502, "Query failed: (" . $mysqli->errno . ") " . $mysqli->error);
	}
	
	Flight::halt(201); 
});

Flight::route('DELETE /@name', function($name, $id)
{	
	//Check Access rights
	$request = Flight::request();
	$token = Flight::request()->query['token'];

	$accessGranted=CheckAccessRights($token, AccessRight::Delete);
	
	if($accessGranted==false)
	{
		Flight::halt(403);
	}	
	
	//Connect to database
	$mysqli = new mysqli(Flight::get('databaseHost'), Flight::get('databaseUsername'), Flight::get('databasePassword'), Flight::get('databaseName'));
	
	if ($mysqli->connect_errno) 
	{
		Flight::halt(502, "Failed to connect to MySQL: (" . $mysqli->connect_errno . ") " . $mysqli->connect_error);
	}	
	
	//Update value
	$dbName=Flight::get('databaseName');
	$sql = "DELETE FROM status WHERE `key`='" . $name . "'";

	if(!$mysqli->query($sql))
	{	
		Flight::halt(502, "Query failed: (" . $mysqli->errno . ") " . $mysqli->error);
	}
});

Flight::start();
?>
