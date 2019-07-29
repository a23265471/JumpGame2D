<?php

    //使用者資訊
    $host = "localhost";
    $user = "root";
    $pass = "";

    $db="moonmoon";
    $tableName="test2";

    //連結資料庫
    $con = mysqli_connect($host,$user,$pass);
    $dbs = mysqli_select_db($con,$db);

    $sql = "SELECT * FROM $tableName";
    $result = mysqli_query($con,$sql);

    $data=array();
    while ($row = mysqli_fetch_array($result)){
      array_push($data, $row);
    }

 //   $data["Score"] = $_POST["Score"];

   // echo "ssss";
    echo json_encode($data);
  //  echo $_POST["Score"];

?>