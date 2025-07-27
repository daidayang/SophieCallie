var canvas = document.querySelector('canvas');

canvas.width = window.innerWidth     //width
canvas.height = window.innerHeight   //height

var c = canvas.getContext('2d');

for (m = 1; m < 9; m++) {
    for (i = 1; i <= 9-m; i++) {
        c.beginPath();
        if ( m > 4 ) {
            c.fillStyle = "red";
        }
        else {
            c.fillStyle = "blue";
        }
        c.arc(100 * i, 90 * m, 20, 0, Math.PI * 2, false);
        c.fill();
    }

}