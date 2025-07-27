var canvas = document.querySelector('canvas');
console.log(canvas);
canvas.width = window.innerWidth - 6;
canvas.height = window.innerHeight - 6;

var c = canvas.getContext('2d');

var d1_x = 5;
var d1_y = 5;

var d2_x = 5;
var d2_y = 5;

var Key_Arrow_Left_Pressed = false;
var Key_Arrow_Right_Pressed = false;
var Key_Arrow_Up_Pressed = false;
var Key_Arrow_Down_Pressed = false;

var x = 100;
var y = 100;

var x2 = 200;
var y2 = 200;

function MoveCircle(delta_x, delta_y) {
    c.beginPath();
    c.fillStyle = "white";
    c.arc(x, y, 21.5, 0, Math.PI * 2, false);
    c.fill();

    x = x + delta_x;
    y = y + delta_y;

    if (x > 1289){
        x = 1289
        d1_x = -5;
    }

    if (y > 668){
        y = 668
        d1_y = -5;
    }

    if (x < 22) {
        x = 22
        d1_x = +5;
    }

    if (y < 22){
        y = 22
        d1_y = +5;
    }

    if ( Math.abs(x2 - x) < 37 && Math.abs(y2 - y) < 37  ) {
            x -= delta_x;
            y -= delta_y;
        }

    c.beginPath();
    c.fillStyle = "#ff0000"
    c.arc(x, y, 20, 0, Math.PI * 2, false);
    c.fill();

//    console.log('x=' + x + " y=" + y)
}


// document.addEventListener('keyup', (event) => {
//     var name = event.key;
//     var code = event.code;
//     console.log(`Key pressed ${name} \n Key code Value: ${code}`);

//     if (code == 'ArrowLeft') {
//         Key_Arrow_Left_Pressed = false;
//     }

//     if (code == 'ArrowRight') {
//         Key_Arrow_Right_Pressed = false;
//     }

//     if (code == 'ArrowUp') {
//         Key_Arrow_Up_Pressed = false;
//     }

//     if (code == 'ArrowDown') {
//         Key_Arrow_Down_Pressed = false;
//     }
// }, false);


document.addEventListener('keydown', (event) => {
    var name = event.key;
    var code = event.code;
    console.log(`Key pressed ${name} \n Key code Value: ${code}`);

    if (code == 'Numpad4') {
        d1_x = -5;
        d1_y = 0;
    }

    if (code == 'Numpad6') {
        d1_x = +5;
        d1_y = 0;
    }

    if (code == 'Numpad8') {
        d1_x = 0;
        d1_y = -5;
    }

    if (code == 'Numpad2') {
        d1_x = 0;
        d1_y = +5;
    }

    if (code == 'Numpad7') {
        d1_x = -5;
        d1_y = -5;
    }

    if (code == 'Numpad9') {
        d1_x = +5;
        d1_y = -5;
    }

    if (code == 'Numpad1') {
        d1_x = -5
        d1_y = +5;
    }

    if (code == 'Numpad3') {
        d1_x = +5;
        d1_y = +5;
    }



    if (code == 'KeyA') {
        MoveCircle2(-5, 0);
    }

    if (code == 'KeyD')
        MoveCircle2(+5, 0);

    if (code == 'KeyW')
        MoveCircle2(0, -5);

    if (code == 'KeyS')
        MoveCircle2(0, +5);

}, false);


function MoveCircle2(delta_x, delta_y) {
//    console.log('HAHA')


    c.beginPath();
    c.fillStyle = "white";
    c.arc(x2, y2, 21.5, 0, Math.PI * 2, false);
    c.fill();

    x2 += delta_x;
    y2 += delta_y;

    if (x2 > 1289 || y2 > 668 || x2 < 22 || y2 < 22) {
        x2 -= delta_x;
        y2 -= delta_y;
    }

    if ( Math.abs(x - x2) < 37 && Math.abs(y - y2) < 37  ) {
        x2 -= delta_x;
        y2 -= delta_y;
    }

    // if ( y2 > 668 ) {
    //     // y2 = 668; 
    //     x2 -= delta_x;
    //     y2 -= delta_y;
    // }

    // if ( x2 < 22 ) {
    //     // x2 = 22
    //     x2 -= delta_x;
    //     y2 -= delta_y;
    // }

    // if ( y2 < 22 ){
    //     // y2 = 22; 
    //     x2 -= delta_x;
    //     y2 -= delta_y;
    // }

    c.beginPath();
    c.fillStyle = "#0000ff"
    c.arc(x2, y2, 20, 0, Math.PI * 2, false);
    c.fill();

//    console.log('x=' + x2 + " y=" + y2)
}





// Add event listener on keyup
//   document.addEventListener('keyup', (event) => {
//     var name = event.key;
//     if (name === 'Control') {
//         console.log('Control key released');
//     }
//   }, false);



// Animating the Canvas | HTML5 Canvas Tutorial for Beginners - Ep. 2
// https://www.youtube.com/watch?v=83L6B13ixQ0
// Animating the Canvas | HTML5 Canvas Tutorial for Beginners - Ep. 3
// https://www.youtube.com/watch?v=yq2au9EfeRQ

c.strokeStyle = "#0000ff"
c.strokeRect(0, 0, 1311, 690)




function timer_event() {


    MoveCircle(d1_x, d1_y);
    MoveCircle2(d2_x, d2_y);
}

var myTimerHandler = setInterval(timer_event, 1000);




