<!DOCTYPE html>
<html lang="en">

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Document</title>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.4.1/jquery.min.js"></script>
    <link rel="stylesheet" href="style.css">
    <link rel="stylesheet" type="text/css" href="DataTables/datatables.min.css" />
    <script type="text/javascript" src="DataTables/datatables.min.js"></script>
</head>

<body>
    <?php
    $bestTable = "BestData";
    $avgTable = "AvgData";
    $conn = mysqli_connect("127.0.0.1", "UnityUser", "BKunjm3uCqjdBQpL", "NeuralNetwork", "5456");
    if (isset($_GET["SesionID"])) {
        if (isset($_GET["Table"])) {
            $sql = "select * from NeuralNetwork." . $_GET["Table"] . " where SesionID=" . $_GET["SesionID"] . ";";
        } else {
            header('Location: ../');
            exit();
        }
    } else {
        header('Location: ../');
        exit();
    }
    $sqlresult = $conn->query($sql);
    $endLine = "\n";
    $htmltable =  "<table id=\"table\">" . $endLine;
    $firstLine   = true;
    while ($row = $sqlresult->fetch_assoc()) {
        if ($firstLine) {
            $htmltable .=   "<thead>" . $endLine;
            $htmltable .=   "<tr>"  . $endLine;
            foreach ($row as $key => $value) {
                $htmltable .=   "<th>" . $key . "</th>"  . $endLine;
            }
            $htmltable .=   "</tr>"  . $endLine;
            $htmltable .=   "</thead>" . $endLine;
            $htmltable .=   "<tbody>" . $endLine;
            $firstLine = false;
        }
        $htmltable .=   "<tr>"  . $endLine;
        foreach ($row as $key => $value) {
            $htmltable .=   "<td>" . $value . "</td>"  . $endLine;
        }
        $htmltable .=   "</tr>"   . $endLine;
    }
    $htmltable .=   "</tbody>" . $endLine;
    $htmltable .=   "</table>"   . $endLine;
    echo $htmltable;
    $conn->close();
    if ($_GET["Table"] == $bestTable) {
        echo "<a href=\"../map/index.php?SesionID=" . $_GET["SesionID"] . "&best=1\">Map of Best Dead points</a>";
    } elseif ($_GET["Table"] == $avgTable) {
        echo "<a href=\"../map/index.php?SesionID=" . $_GET["SesionID"] . "&best=0\">Map of all Dead points</a>";
    } else {
        echo "<a href=\"../map/index.php?SesionID=" . $_GET["SesionID"] . "\">Map of all with best points</a>";
    }
    ?>
    <script>
        $(document).ready(function() {
            $('#table').DataTable();
        });
    </script>
</body>

</html>