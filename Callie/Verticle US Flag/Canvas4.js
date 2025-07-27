var canvas = document.querySelector('canvas');
console.log(canvas);
canvas.width = window.innerWidth - 6;
canvas.height = window.innerHeight - 6;

var c = canvas.getContext('2d');
c.beginPath();
c.fillStyle = "white";
c.strokeRect (600, 25, 400, 700);
c.stroke();

for (var i = 0; i < 7; i++) {
    c.beginPath();
    let x = i * 62
    c.fillStyle = "red";
    c.fillRect(600+x, 25, 31, 700);
    c.stroke();
}

c.beginPath();
c.fillStyle = "darkblue";
c.fillRect (785, 25, 220, 350);
c.stroke();
 
// c.fillStyle = "white"

// c.beginPath();
// c.moveTo(980, 34 );
// c.lineTo(980, 38 );
// c.lineTo(984, 41 );
// c.lineTo(980, 44 );
// c.lineTo(980, 48 );
// c.lineTo(975, 44 );
// c.lineTo(970, 47 );
// c.lineTo(973, 41 );
// c.lineTo(970, 36 );
// c.lineTo(975, 38 );
// c.lineTo(980, 34);
// c.fill();
// c.closePath();

function star(x, y) {

    c.fillStyle = "white";

    c.beginPath();

    c.moveTo(x, y);
    c.lineTo(x , y + 4);
    c.lineTo(x , y + 3);
    c.lineTo(x + 4, y + 7);
    c.lineTo(x , y + 10);
    c.lineTo(x , y + 14);
    c.lineTo(x - 5, y + 10);
    c.lineTo(x - 10, y + 13);
    c.lineTo(x - 7, y + 7);
    c.lineTo(x - 10, y + 2);
    c.lineTo(x - 5, y + 4);
    c.lineTo(x, y);
    c.closePath();
    c.fill();

}

for (var i = 0; i < 5; i++) {
        star (800 + 50 * i, 40 );
        star (800+ 50*i , 100);
        star (800+ 50*i , 160);
        star (800+ 50*i , 220);
        star (800+ 50*i , 280);
        star (800 + 50*i, 340);
}


for (var i = 0; i < 4; i++) {
    star(825 + 50 * i, 60);
    star(825 + 50 * i, 130);
    star(825 + 50 * i, 200);
    star(825 + 50 * i, 260);
    star(825 + 50 * i, 310);
    
}


