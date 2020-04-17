<!DOCTYPE html>
<html lang="en">

<head>
  <meta charset="UTF-8" />
  <meta name="viewport" content="width=device-width, initial-scale=1.0" />
  <title>Document</title>
</head>

<body style="margin: 0; display: flex;">
  <canvas id="canvas"></canvas>
  <div style="display: none;" id="Points">
    <?php
    if (isset($_GET["SesionID"])) {
      $conn = mysqli_connect("127.0.0.1", "UnityUser", "BKunjm3uCqjdBQpL", "NeuralNetwork", "5456");
      if (isset($_GET["best"])) {
        if ($_GET["best"]) {
          $sql = "select Dead_X,Dead_Y from NeuralNetwork.BestData where SesionID=" . $_GET["SesionID"] . ";";
          $result = $conn->query($sql);
          while ($row = $result->fetch_assoc()) {
            echo "<p>{\"x\":" . $row["Dead_X"] . ",\"y\":" . $row["Dead_Y"] . ",\"best\":true}</p>";
          }
        } else {
          $sql = "select Dead_X,Dead_Y from NeuralNetwork.PlayerData where SesionID=" . $_GET["SesionID"] . ";";

          $sql = "select Dead_X,Dead_Y from NeuralNetwork.PlayerData where SesionID=" . $_GET["SesionID"] . ";";
          $result = $conn->query($sql);
          while ($row = $result->fetch_assoc()) {
            echo "<p>{\"x\":" . $row["Dead_X"] . ",\"y\":" . $row["Dead_Y"] . ",\"best\":false}</p>";
          }
        }
      } else {
        $sql = "select Dead_X,Dead_Y from NeuralNetwork.BestData where SesionID=" . $_GET["SesionID"] . ";";
        $result = $conn->query($sql);
        while ($row = $result->fetch_assoc()) {
          echo "<p>{\"x\":" . $row["Dead_X"] . ",\"y\":" . $row["Dead_Y"] . ",\"best\":true}</p>";
        }

        $sql = "select Dead_X,Dead_Y from NeuralNetwork.PlayerData where SesionID=" . $_GET["SesionID"] . ";";
        $result = $conn->query($sql);
        while ($row = $result->fetch_assoc()) {
          echo "<p>{\"x\":" . $row["Dead_X"] . ",\"y\":" . $row["Dead_Y"] . ",\"best\":false}</p>";
        }
      }
      $conn->close();
    }
    ?>
  </div>
  <script src="app.js" id="canvas"></script>
</body>

</html>