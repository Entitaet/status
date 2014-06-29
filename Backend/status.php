<?php

use Luracast\Restler\RestException;

class Status
{
    //Database
    private $dbHost = 'localhost'; // Datenbank Server - meistens "localhost" kann aber auch eine IP sein
    private $dbName = 'status'; // Datenbank Name
    private $dbUser = 'root'; // Datenbank Benutzer
    private $dbPassword = 'root'; // Datenbank Passwort  
		
    /**
     * @param string $n1 {@from path}
     *
     * @return array
     */
    protected function get($n1)
    {		
		//Connect to database
		$mysqli = new mysqli($this->dbHost, $this->dbUser, $this->dbPassword, $this->dbName);
		
		if ($mysqli->connect_errno) {
			throw new RestException(502, "Failed to connect to MySQL: (" . $mysqli->connect_errno . ") " . $mysqli->connect_error);
		}
		
		//Query data
		$sql = "SELECT * FROM status WHERE `key`='" . $n1 . "'";
		$result = $mysqli->query($sql);
		
		//Pick data
		$rowNumber=0;
		$result->data_seek($rowNumber);
		$row = $result->fetch_assoc();
		$rowValueAsString=$row['value'];
		
		//Return result
		if($result->num_rows>0)
		{	
	        return array(
	            'value' => ($rowValueAsString)
	        );
		}
		else
		{
			throw new RestException(204);
		}
    }
	
    /**
     * @param string $n1 {@from path}
     * @param string $n2 {@from path}
     *
     * @return array
     */
    protected function post($n1, $n2)
    {	
		//Connect to database
		$mysqli = new mysqli($this->dbHost, $this->dbUser, $this->dbPassword, $this->dbName);
		
		if ($mysqli->connect_errno) 
		{
			throw new RestException(502, "Failed to connect to MySQL: (" . $mysqli->connect_errno . ") " . $mysqli->connect_error);
		}	
		
		//Update value
		$sql="UPDATE {$this->dbName} SET value='{$n2}' WHERE `key`='{$n1}'";	

		if(!$mysqli->query($sql))
		{	
			throw new RestException(502, "Query failed: (" . $mysqli->errno . ") " . $mysqli->error);
		}
	}
}