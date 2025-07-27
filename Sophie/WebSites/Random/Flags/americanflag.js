var canvas = document.querySelector('canvas');

canvas.width = window.innerWidth     //width
canvas.height = window.innerHeight   //height

var c = canvas.getContext('2d');

c.fillStyle = "white";
c.fillRect(275, 125, 1040, 550);
c.fill();

for (var s = 0; s < 7; s++) {
  c.fillStyle = "red";
  c.fillRect(0 + 275, s * 84.8 + 125, 1040, 42.4)
  c.fill();
}

c.fillStyle = "blue";
c.fillRect(275, 125, 420, 296)
c.fill();


for (var o = 0; o < 5; o++) {
  for (var j = 0; j < 6; j++) {
    make_star(290 + o * 72, 145 + j * 45)
  }
  for (var j = 0; j < 5; j++) {
    make_star(325 + o * 72, 170 + j * 45)
  }
}

for (var m = 0; m < 6; m++) {
  make_star(650, 145 + m * 45)
}

function make_star(sx, sy) {
  base_image = new Image();
  base_image.src = "eee.png";
  base_image.onload = function () {
    c.drawImage(base_image, sx, sy, 33.6, 33.6);
  }
}



// c.fillStyle = "white";
// c.beginPath();
// c.moveTo(108, 0.0);
// c.lineTo(141, 70);
// c.lineTo(218, 78.3);
// c.lineTo(162, 131);
// c.lineTo(175, 205);
// c.lineTo(108, 170);
// c.lineTo(41.2, 205);
// c.lineTo(55, 131);
// c.lineTo(1, 78);
// c.lineTo(75, 68);
// c.lineTo(108, 0);
// c.closePath();
// c.fill();

