var canvas = document.querySelector('canvas');
console.log(canvas);
canvas.width = window.innerWidth - 6;
canvas.height = window.innerHeight - 6;

var c = canvas.getContext('2d');

var Pedal1_y = 100;
var Pedal2_y = 100;

var ball_x = 100;
var ball_y = 100;
var ball_dlt_x = -5;
var ball_dlt_y = -5;

function MovePedal1(delta_y) {
    c.beginPath();
    c.fillStyle = "white";
    c.fillRect(1575, Pedal1_y, 5, 150);
    c.stroke();

    Pedal1_y = Pedal1_y + delta_y;

    if (Pedal1_y < 10){
        Pedal1_y = 10        
    }

    if (Pedal1_y > 656){
        Pedal1_y = 656        
    }

    c.beginPath();
    c.fillStyle = "black";
    c.fillRect(1575, Pedal1_y, 5, 150);
    c.stroke();
}


function MovePedal2(delta_y) {
    c.beginPath();
    c.fillStyle = "white";
    c.fillRect(50, Pedal2_y, 5, 150);
    c.stroke();

    Pedal2_y = Pedal2_y + delta_y;

    if (Pedal2_y < 10){
        Pedal2_y = 10        
    }

    if (Pedal2_y > 656){
        Pedal2_y = 656        
    }

    c.beginPath();
    c.fillStyle = "black";
    c.fillRect(50, Pedal2_y, 5, 150);
    c.stroke();
}



function MoveBall() {
    c.beginPath();
    c.fillStyle = "white";
    c.arc(ball_x, ball_y, 21.5, 0, Math.PI * 2, false);
    c.fill();

    ball_x = ball_x + ball_dlt_x;
    ball_y = ball_y + ball_dlt_y;

    if (ball_x > 1550){
        ball_x = 1550
        ball_dlt_x = -5;
    }

    if (ball_y > 720){
        ball_y = 720
        ball_dlt_y = -5;
    }

    if (ball_x < 80) {
        ball_x = 80
        ball_dlt_x = +5;
    }

    if (ball_y < 22){
        ball_y = 22
        ball_dlt_y = +5;
    }

    c.beginPath();
    c.fillStyle = "#ff0000"
    c.arc(ball_x, ball_y, 20, 0, Math.PI * 2, false);
    c.fill();

//    console.log('x=' + x + " y=" + y)
}



document.addEventListener('keydown', (event) => {
    var name = event.key;
    var code = event.code;
    console.log(`Key pressed ${name} \n Key code Value: ${code}`);

    if (code == 'ArrowUp') {
        MovePedal1(-40) ;
    }

    if (code == 'ArrowDown') {
        MovePedal1(+40) ;
    }

    if (code == 'KeyW') {
        MovePedal2(-40) ;
    }

    if (code == 'KeyS') {
        MovePedal2(+40) ;
    }
});



function timer_event() {
    MoveBall();
}

var myTimerHandler = setInterval(timer_event, 20);


