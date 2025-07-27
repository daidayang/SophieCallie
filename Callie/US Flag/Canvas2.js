var canvas = document.querySelector('canvas');
console.log(canvas);
canvas.width = window.innerWidth - 6;
canvas.height = window.innerHeight - 6;

var c = canvas.getContext('2d');
c.beginPath();
c.fillStyle = "white";
c.strokeRect(200, 75, 1200, 600);
c.stroke();

for (var i = 0; i < 9; i++) {
    c.beginPath();
    let y = i * 100
    c.fillStyle = "red";
    c.fillRect(200, y + 75, 1200, 50);
    c.stroke();
}

c.beginPath();
c.fillStyle = "darkblue";
c.fillRect(200, 75, 550, 350);
c.stroke();

// c.fillStyle = "white";

// c.beginPath();
// c.moveTo(225, 100);
// c.lineTo(230, 90);
// c.lineTo(235, 100);
// c.lineTo(245, 100);
// c.lineTo(237, 108);
// c.lineTo(240, 118);
// c.lineTo(230, 110);
// c.lineTo(221, 118);
// c.lineTo(223, 108);
// c.lineTo(215, 100);
// c.lineTo(225, 100);
// c.closePath();
// c.fill();



function star(x, y) {

    c.fillStyle = "white";

    c.beginPath();

    c.moveTo(x, y);
    c.lineTo(x + 5, y - 10);
    c.lineTo(x + 10, y);
    c.lineTo(x + 20, y);
    c.lineTo(x + 12, y + 8);
    c.lineTo(x + 15, y + 18);
    c.lineTo(x + 5, y + 10);
    c.lineTo(x - 4, y + 18);
    c.lineTo(x - 2, y + 8);
    c.lineTo(x - 10, y);
    c.lineTo(x, y);
    c.closePath();
    c.fill();

}

for (var j = 0; j < 5; j++) {
    for (var i = 0; i < 6; i++) {
        star(225 + 98 * i, 100 + j * 70);
        // star (225+ 98*i , 170);
        // star (225+ 98*i , 240);
        // star (225+ 98*i , 310);
        // star (225+ 98*i , 380);

    }
}


for (var i = 0; i < 5; i++) {
    star(270 + 98 * i, 135);
    star(270 + 98 * i, 205);
    star(270 + 98 * i, 275);
    star(270 + 98 * i, 345);
}


