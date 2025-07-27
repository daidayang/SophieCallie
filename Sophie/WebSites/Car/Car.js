
var canvas = document.querySelector('canvas')
    ;

canvas.width = window.innerWidth - 10
canvas.height = window.innerHeight - 10

function make_base()
{
  base_image = new Image();
  base_image.src = 'depreshon-stanley-closeup.png';
  base_image.onload = function(){
    c.drawImage(base_image, 0, 0);
  }
}

var c = canvas.getContext('2d');

function Car(x, y, angle) {
    this.x = x;
    this.y = y;
    this.angle = angle;

    this.erase = function () {
        c.fillStyle = "white"
        c.fillRect(this.x, this.y, 200, 140);
    }

    this.draw = function () {
        c.fillStyle = "red"
        c.fillRect(this.x, this.y, 150, 100);
        console.log(c)
    }
    this.moveleft = function () {

        // //  At the edge.  Can't move.  exit
        // if (this.x <= 0)
        //     return;

        this.erase();
        this.x = this.x - 10;
        this.draw();
    }

    this.moveright = function () {

        // //  At the edge.  Can't move.  exit
        // if ( this.x >= 3 )
        //     return;

        this.erase();
        this.x = this.x + 10;
        this.draw();
    }

    this.moveup = function () {

        // //  At the edge.  Can't move.  exit
        // if ( this.y <= 0 )
        //     return;

        this.erase();
        this.y = this.y - 10
        this.draw();
    }

    this.movedown = function () {

        // //  At the edge.  Can't move.  exit
        // if ( this.y <= 0 )
        //     return;

        this.erase();
        this.y = this.y + 10
        this.draw();
    }
}

document.addEventListener('keydown', event => {
    var buttonPressed = event.keyCode;
    console.log(buttonPressed);
    if (buttonPressed == 37) {  //  Left
        console.log("Left");
        car.moveleft();
    }
    if (buttonPressed == 39) {
        console.log("Right");
        car.moveright();
    }

    if (buttonPressed == 38) {  //  Up
        console.log("Up");
        car.moveup();
    }

    if (buttonPressed == 40) {
        console.log("down");
        car.movedown();
    }

    //  This is when + key pressed
    if (buttonPressed == 187) {
        console.log("haha")

        if ( squareArray.length >= 16 )
        {
            console.log("Game OVER")
        }
        else {
            //  Logic to add new square
            var newx
            var newy
            var conflictFound;
            do {
                conflictFound = false;
                newx = Math.trunc(Math.random() * 4)
                newy = Math.trunc(Math.random() * 4)
                for (var i = 0; i < squareArray.length; i++) {
                    if ( squareArray[i].y == newy && squareArray[i].x == newx ) {
                        conflictFound = true;
                        break;
                    }
                }   
                console.log("conflictFound=" + conflictFound)
            } while (conflictFound==true)
            
                var sq1 = new Square(newx, newy)
                sq1.draw();
                squareArray.push(sq1);
           
                console.log('array length:' + squareArray.length)        
            //  Logic to add new square
        }
    }    
});


var car = new Car(400, 400, 0);
car.draw();


make_base();