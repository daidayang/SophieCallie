var canvas = document.querySelector('canvas');
console.log(canvas);
canvas.width = window.innerWidth - 6;
canvas.height = window.innerHeight - 6;

var c = canvas.getContext('2d');

c.beginPath();
c.fillStyle = "white";
c.strokeRect(200, 75, 1200, 600);
c.stroke();

c.beginPath();
c.fillStyle = "red";
c.fillRect(200, 400, 1200, 275);
c.stroke();

c.beginPath();
c.fillStyle = "darkblue";
c.fillRect(200, 75, 400, 600);
c.stroke();

c.fillStyle = "white";

c.beginPath();
c.moveTo(325, 325);
c.lineTo(380, 325);
c.lineTo(400, 255);
c.lineTo(420, 325);
c.lineTo(475, 325);
c.lineTo(435, 370);
c.lineTo(470, 435);
c.lineTo(400, 400);
c.lineTo(330, 435);
c.lineTo(365, 370);
c.lineTo(325, 325);
c.closePath();
c.fill();

