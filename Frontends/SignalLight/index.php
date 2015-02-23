<?php require_once(__DIR__ . "/config.php"); ?>

<!DOCTYPE html>
<!--[if lt IE 7]>      <html class="no-js lt-ie9 lt-ie8 lt-ie7"> <![endif]-->
<!--[if IE 7]>         <html class="no-js lt-ie9 lt-ie8"> <![endif]-->
<!--[if IE 8]>         <html class="no-js lt-ie9"> <![endif]-->
<!--[if gt IE 8]><!--> <html class="no-js"> <!--<![endif]-->
    <head>
        <meta charset="utf-8">
        <meta http-equiv="X-UA-Compatible" content="IE=edge">
        <title>Entität e.V. - Status</title>
        <meta name="description" content="">
        <meta name="viewport" content="width=device-width, initial-scale=1">

        <!-- Place favicon.ico and apple-touch-icon.png in the root directory -->
		<link rel="apple-touch-icon" href="/apple-touch-icon.png"/>

        <link rel="stylesheet" href="css/normalize.css">
        <link rel="stylesheet" href="css/main.css">
        <script src="js/vendor/modernizr-2.6.2.min.js"></script>
    </head>
    <body>
        <!--[if lt IE 7]>
            <p class="browsehappy">You are using an <strong>outdated</strong> browser. Please <a href="http://browsehappy.com/">upgrade your browser</a> to improve your experience.</p>
        <![endif]-->

		<div id="content">
		
        <!-- Add your site or application content here -->
        <h2>Entität e.V.</h2>
		
		<div id="status-entitaet"></div>

		</div>
		
        <script src="//ajax.googleapis.com/ajax/libs/jquery/1.10.2/jquery.min.js"></script>
        <script>window.jQuery || document.write('<script src="js/vendor/jquery-1.10.2.min.js"><\/script>')</script>

		<script>
		var xmlHttp;

		function setStatusTimer(){
				getState();
		        setInterval('getState()', 64000);
		}

		function getState() {
		    xmlHttp = GetXmlHttpObject();
		    if (xmlHttp == null) {
		        alert("Browser does not support HTTP Request");
		        return;
		    }
		    var url = "status.php";
		    xmlHttp.onreadystatechange = stateChangedServerState;
		    xmlHttp.open("GET", url, true);
		    xmlHttp.send(null);
		}

		function stateChangedServerState() {
		    if (xmlHttp.readyState == 4 || xmlHttp.readyState == "complete") {
		        document.getElementById("status-entitaet").innerHTML = xmlHttp.responseText;
		    }
		}

		function GetXmlHttpObject() {
		    var xmlHttp = null;
		    try {
		        xmlHttp = new XMLHttpRequest;
		    } catch (e) {
		        try {
		            xmlHttp = new ActiveXObject("Msxml2.XMLHTTP");
		        } catch (e) {
		            xmlHttp = new ActiveXObject("Microsoft.XMLHTTP");
		        }
		    }
		    return xmlHttp;
		}

		setStatusTimer();
		</script>

        <!-- Piwik -->
        <script>
		
        </script>
    </body>
</html>
