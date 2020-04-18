var canvas = document.getElementById("canvas"),
  ctx = canvas.getContext("2d");

let points = [];
let mouseDown = false;
let cursor = {
  x: 10,
  y: 10,
};
let map = {
  x: 0,
  y: 0,
  scale: 10,
  minScale: 0.5,
};
let intervalScaleDown;
let intervalScaleUp;
let pointElement = Array.from(document.getElementById("Points").getElementsByTagName("p"));
pointElement.forEach((element) => {
  points.push(JSON.parse(element.innerText));
});
function centerMap() {
  avgX = 0;
  avgY = 0;
  points.forEach((element) => {
    avgX += element.x;
    avgY += element.y;
  });
  avgX /= points.length;
  avgY /= points.length;
  map.x = avgX;
  map.y = avgY;
}
centerMap();
window.onresize = resize;
function resize() {
  canvas.width = window.innerWidth * 1;
  canvas.height = window.innerHeight * 1;
}
document.onmousedown = () => (mouseDown = true);
document.onmouseup = () => (mouseDown = false);
document.onmousemove = mouseMove;
function mouseMove(event) {
  if (mouseDown) {
    map.x += (event.pageX - cursor.x) * map.scale;
    map.y += (event.pageY - cursor.y) * map.scale;
  }
  cursor.x = event.pageX;
  cursor.y = event.pageY;
}
document.onkeydown = keyDown;
function keyDown(event) {
  switch (event.which) {
    case 38:
      if (intervalScaleDown == null) {
        intervalScaleDown = setInterval(() => {
          if (map.scale > map.minScale) {
            map.scale -= 0.1;
            map.x -= cursor.x * 0.05;
            map.y -= cursor.y * 0.05;
          }
        }, 1);
      }
      break;
    case 40:
      if (intervalScaleUp == null) {
        intervalScaleUp = setInterval(() => {
          map.scale += 0.1;
          map.x += cursor.x * 0.05;
          map.y += cursor.y * 0.05;
        }, 1);
      }
      break;
  }
}
document.onkeyup = keyUp;
function keyUp(event) {
  switch (event.which) {
    case 38:
      if (intervalScaleDown != null) {
        clearInterval(intervalScaleDown);
      }
      intervalScaleDown = null;
      break;
    case 40:
      if (intervalScaleUp != null) {
        clearInterval(intervalScaleUp);
      }
      intervalScaleUp = null;
      break;
  }
}
function draw() {
  ctx.beginPath();
  ctx.fillStyle = "#000";
  ctx.fillRect(0, 0, canvas.width, canvas.height);

  for (let i = 0; i < points.length; i++) {
    if (points[i].best) {
      ctx.fillStyle = "#0F0";
      ctx.fillRect((points[i].x + map.x) / map.scale, (points[i].y + map.y) / map.scale, 3, 3);
    } else {
      ctx.fillStyle = "#FFF";
      ctx.fillRect((points[i].x + map.x) / map.scale, (points[i].y + map.y) / map.scale, 1, 1);
    }
  }
  ctx.stroke();
  requestAnimationFrame(draw);
}

document.addEventListener("wheel", scroll);
function scroll(event) {
  let changeScroll = (event.deltaX + event.deltaY) / 100;
  map.scale += changeScroll;
  if (map.scale < map.minScale) {
    map.scale = map.minScale;
  } else {
    map.x += cursor.x * 0.5 * changeScroll;
    map.y += cursor.y * 0.5 * changeScroll;
  }
}
resize();
draw();
