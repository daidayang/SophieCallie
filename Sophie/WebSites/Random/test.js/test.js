//drawing functions

var canvas = document.querySelector('canvas');

canvas.width = window.innerWidth     //width
canvas.height = window.innerHeight   //height

var c = canvas.getContext('2d');


function f(x) {
    // return x ** 3 * 0.00001  + 50
    // return 5 * x - 25
    return - 5 * x + 25
}

var x = 5
var y = 
console.log(f(x)) 

c.beginPath();
c.moveTo(0,400);
c.lineTo(1600, 400);
c.moveTo(800,0);
c.lineTo(800, 800);

c.strokeStyle = "#B48395"
c.lineWidth = 1
c.stroke();



for (var x = -800; x < 800; x = x+5) {
    if (x==-800)
    c.moveTo(x + 800,f(x) + 400);
    else
    c.lineTo(x + 800,f(x) + 400);
    c.strokeStyle = "#B48395"
    c.lineWidth = 1
    c.stroke();
}