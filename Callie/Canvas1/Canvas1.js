var canvas = document.querySelector('canvas');
console.log(canvas);
canvas.width = window.innerWidth - 4;
canvas.height = window.innerHeight - 4;

var c = canvas.getContext('2d');

// c.fillRect(100, 100, 200, 100)
// c.fillRect(600, 100, 200, 100)
// c.fillRect(50, 300, 800, 100)
// c.strokeStyle = "#ff00ff"
// c.strokeRect(100, 100 ,200 , 200)

//  Line
// c.beginPath();
// c.moveTo(100,300);
// c.lineTo(200, 100);
// c.lineTo(300, 300);
// c.lineTo(100, 300)
// c.strokeStyle="#0000ff"
// c.stroke();

// c.beginPath();
// c.moveTo(200, 100)
// c.lineTo(200, 300)
// c.strokeStyle='#ff0000'
// c.stroke();

for( var i = 0; i <101; i++){
    var x = Math.random() * window.innerWidth;
    var y = Math.random() * window.innerHeight
    var d = Math.random() * 30
c.beginPath();
c.arc(x, y, d, 0, Math.PI * 2, false);
if ( i % 5 == 0) c.strokeStyle = "#0000ff"
if ( i % 5 == 1) c.strokeStyle = "#00ff00"
if ( i % 5 == 2) c.strokeStyle = "#ff0000"
if ( i % 5 == 3) c.strokeStyle = "#ff00ff"
if ( i % 5 == 4) c.strokeStyle = "#00ffff"
c.stroke();
}

// Animating the Canvas | HTML5 Canvas Tutorial for Beginners - Ep. 2
// https://www.youtube.com/watch?v=83L6B13ixQ0
// Animating the Canvas | HTML5 Canvas Tutorial for Beginners - Ep. 3
// https://www.youtube.com/watch?v=yq2au9EfeRQ

for( var i = 0; i <101; i++){
    var x = Math.random() * window.innerWidth;
    var y = Math.random() * window.innerHeight
    var d = Math.random() * 50
c.beginPath();
c.strokeRect(x, y, d, d,);
if ( i % 5 == 0) c.strokeStyle = "#0000ff"
if ( i % 5 == 1) c.strokeStyle = "#00ff00"
if ( i % 5 == 2) c.strokeStyle = "#ff0000"
if ( i % 5 == 3) c.strokeStyle = "#ff00ff"
if ( i % 5 == 4) c.strokeStyle = "#00ffff"
c.stroke();
}

for( var i = 0; i <101; i++){
    var x = Math.random() * window.innerWidth;
    var y = Math.random() * window.innerHeight
    var d = Math.random() * 30
c.beginPath();
c.moveTo(x, y);
c.lineTo(x+d, y+2*d);
c.lineTo(x-d, y+2*d);
c.lineTo(x, y)
if ( i % 5 == 0) c.strokeStyle = "#0000ff"
if ( i % 5 == 1) c.strokeStyle = "#00ff00"
if ( i % 5 == 2) c.strokeStyle = "#ff0000"
if ( i % 5 == 3) c.strokeStyle = "#ff00ff"
if ( i % 5 == 4) c.strokeStyle = "#00ffff"
c.stroke();
}

for( var i = 0; i < 101; i++){
    var x = Math.random() * window.innerWidth;
    var y = Math.random() * window.innerHeight
    var d = Math.random() * 30
c.beginPath();
c.ellipse(x, y, 15, d, 0, 0, Math.PI*2, false)
if ( i % 5 == 0) c.strokeStyle = "#0000ff"
if ( i % 5 == 1) c.strokeStyle = "#00ff00"
if ( i % 5 == 2) c.strokeStyle = "#ff0000"
if ( i % 5 == 3) c.strokeStyle = "#ff00ff"
if ( i % 5 == 4) c.strokeStyle = "#00ffff"
c.stroke();
     x = Math.random() * window.innerWidth;
     y = Math.random() * window.innerHeight
c.beginPath();
c.ellipse(x, y, d, 15, 0, 0, Math.PI*2, false)
if ( i % 5 == 0) c.strokeStyle = "#0000ff"
if ( i % 5 == 1) c.strokeStyle = "#00ff00"
if ( i % 5 == 2) c.strokeStyle = "#ff0000"
if ( i % 5 == 3) c.strokeStyle = "#ff00ff"
if ( i % 5 == 4) c.strokeStyle = "#00ffff"
c.stroke();
}

for( var i = 0; i < 101; i++){
    var x = Math.random() * window.innerWidth;
    var y = Math.random() * window.innerHeight
    if ( i % 5 == 0) c.strokeStyle = "#0000ff"
    if ( i % 5 == 1) c.strokeStyle = "#00ff00"
    if ( i % 5 == 2) c.strokeStyle = "#ff0000"
    if ( i % 5 == 3) c.strokeStyle = "#ff00ff"
    if ( i % 5 == 4) c.strokeStyle = "#00ffff"
    var l = '';
    if ( i % 3 == 0) l = 'a';
    if ( i % 3 == 1) l = 'b';
    if ( i % 3 == 2) l = 'c';

    var s = Math.random() * 12 + 36;
    c.font = Math.floor(s)  + 'px sans' 
console.log(c.font)
    c.strokeText (l, x, y);
}



