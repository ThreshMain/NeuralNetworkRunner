<!DOCTYPE html>
<html lang="en">

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Document</title>
    <link rel="stylesheet" href="style.css">
</head>

<body>
    <table>
        <thead>
            <tr>
                <th>Session number</th>
                <th>Best</th>
                <th>Avg</th>
                <th>All</th>
            </tr>
        </thead>
        <tbody>
            <?php

            $conn = mysqli_connect("127.0.0.1", "UnityUser", "BKunjm3uCqjdBQpL", "NeuralNetwork", "5456");
            $sql = "SELECT * FROM NeuralNetwork.Session;";
            $result = $conn->query($sql);
            while ($row = $result->fetch_assoc()) {
                echo "<tr>";
                echo "<td>" . $row["id"] . "</td>";
                echo "<td><a href=\"DataViewer/index.php?SesionID=" . $row["id"] . "&Table=BestData\">Link</a></td>";
                echo "<td><a href=\"DataViewer/index.php?SesionID=" . $row["id"] . "&Table=AvgData\">Link</a></td>";
                echo "<td><a href=\"DataViewer/index.php?SesionID=" . $row["id"] . "&Table=PlayerData\">Link</a></td>";
                echo "</tr>";
            }
            ?>

        </tbody>
    </table>
</body>

</html>