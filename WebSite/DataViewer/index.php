<!DOCTYPE html>
<html lang="en">

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Document</title>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.4.1/jquery.min.js"></script>
    <link rel="stylesheet" href="style.css">
    <link rel="stylesheet" type="text/css" href="https://cdn.datatables.net/1.10.20/css/jquery.dataTables.css">
    <script type="text/javascript" charset="utf8" src="https://cdn.datatables.net/1.10.20/js/jquery.dataTables.js"></script>
</head>

<body>
    <?php
    $bestTable = "BestData";
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
    $best = false;
    if ($_GET["Table"] == $bestTable) {
        $best = true;
    }
    echo "<a href=\"../map/index.php?SesionID=" . $_GET["SesionID"] . "&best" . $best . "\">Map of Dead points</a>";
    ?>
    <script>
        $(document).ready(function() {
            $('#table').DataTable();
        });
    </script>
</body>

</html>