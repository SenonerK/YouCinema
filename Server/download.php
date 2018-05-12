<?php
$dir = scandir("./upload");

foreach ($dir as $value){
    if($value == $_GET['imgID'] . ".png"){
        header("Location: ./upload/" . $value);
    }
}
?>
