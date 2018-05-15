<?php

    $uploaddir = "upload/";
    $alphabet = array('a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9');

    function checkIfExists($file)
    {
        $dir = scandir("upload/");
        foreach ($dir as $value){
            if($value == $file){
                return true;
            }
        }
        return false;
    }

    if(isset($_GET["task"]))
    {
        if($_GET["task"] == "test")
        {
            echo "OK";
        }
        else if ($_GET["task"] == "download" && isset($_GET["imgID"]))
        {
            $dir = scandir($uploaddir);
            foreach ($dir as $value){
                if($value == $_GET['imgID']){
                    header("Location: ./upload/" . $value);
                }
            }
        }
        else if ($_GET["task"] == "upload" && isset($_FILES['file']) && is_uploaded_file($_FILES['file']['tmp_name']))
        {
            $ID = "";
            do {
                for ($i = 0; $i < 10; $i++)
                {
                    $ID = $ID . $alphabet[rand(0, count($alphabet)-1)];
                }
            }
            while (checkIfExists($ID));

            $uploadfile = $uploaddir . basename($ID);

            if (move_uploaded_file($_FILES['file']['tmp_name'], $uploadfile))
                die($ID);
            else
                die("ERROR");
        }
        else
            die("<h1>This parameter does not exitst!</h1>");
    }
    else
        die("<h1>This is a CDN. Thers nothing to see here!</h1>");
?>