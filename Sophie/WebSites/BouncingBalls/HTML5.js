console.log("mama")
//control canvas size, width, and height ig
var canvas = document.querySelector('canvas')
        ;

canvas.width = window.innerWidth     //width
canvas.height = window.innerHeight   //height


console.log(window.innerHeight)
var c = canvas.getContext('2d');

//c.fillRect(x, y, width, height);
// c.fillStyle = "#8fb6ab"
// c.fillRect(100, 100, 100, 100);
// c.fillRect(400, 100, 100, 100);
// c.fillRect(300, 300, 100, 100);
// console.log(canvas);

//2. Drawing Elements 
    
//1. Rectangles
//2. lines
//3. Arcs / Circles
//4. Bezier Curves
//5. Images
//6. Text

//line 
//c.beginPath();
//c.moveTo(50, 300);
//c.lineTo(300, 100)
//c.lineTo(400, 300)
//c.strokeStyle = "#B48395"
//c.stroke();

// Arc / Cicle
//c.beginPath();
//c.arc(300, 300, 30, 0, Math.PI * 2, false);
//c.strokeStyle = ""
//c.stroke();

//for (var i = 0; i < 3; i++) {
        //var x = Math.random() * window.innerWidth;
        //var y = Math.random() * window.innerHeight;
        //c.beginPath();
        //c.arc(x, y, 30, 0, Math.PI * 2, false);
        //c.stroke();
//}

//x: Int, y: Int, r: Int startAngle: Float, endAngle: 
        //Float, drawCounterClockwise: Bool (false));
//for can multiply, randomize, the amount of objects

// var x = Math.random() * innerWidth;
// var y = Math.random() * innerHeight;
// var dx = (Math.random() - 0.5) * 8;
// var dy = (Math.random() - 0.5) * 8;
// var radius = 30;


function Circle(id, x, y, dx, dy, radius) {
        this.id = id;
        this.x = x;
        this.y = y;
        this.dx = dx;
        this.dy = dy;
        this.radius = radius;
        this.touhched = false;

        this.draw = function() {
                c.beginPath();
                c.arc(this.x, this.y, this.radius, 0, Math.PI * 2, false);
                // c.strokeStyle = "#9873AC";
                c.fillStyle = "white";
                // c.stroke();
                c.fill();
        }

        this.update = function() {
                //      bounce at edge
                if (this.x + this.radius > innerWidth || this.x - this.radius < 0) {
                        this.dx = -this.dx;
                }
                if (this.y + this.radius > innerHeight || this.y - this.radius < 0) {
                        this.dy = -this.dy;
                }

                //      bounce at eachother
                this.touhched = false;
                for( var idx=0; idx<circleArray.length; idx++)
                {
                        if ( idx == this.id)
                                continue;

                        var other = circleArray[idx];
                        if ( ( (other.x + other.radius > this.x - this.radius) && (other.x + other.radius < this.x + this.radius) 
                            || (other.x - other.radius > this.x - this.radius) && (other.x - other.radius < this.x + this.radius) ) && 
                            ( (other.y + other.radius > this.y - this.radius) && (other.y + other.radius < this.y + this.radius) 
                            || (other.y - other.radius > this.y - this.radius) && (other.y - other.radius < this.y + this.radius) ) )
                        {
                                this.touhched = true;
                                break;
                        }
                }
                if (this.touhched) {
                       this.dx = -this.dx;
                       this.dy = -this.dy;
                }


                
                this.x += this.dx;
                this.y += this.dy;

                this.draw();
        }
}



var circleArray = [];

for (var i = 0; i < 10; i++) {
        var radius = 30;
        var x = Math.random() * (innerWidth - radius * 2) + radius;
        var y = Math.random() * (innerHeight - radius * 2) + radius;
        var dx = Math.random() * 3 + 1;
        var dy = Math.random() * 3 + 1;
        circleArray.push(new Circle(i, x, y, dx, dy, radius));
}


function animate() {
        requestAnimationFrame(animate);
        c.clearRect(0, 0, innerWidth, innerHeight);

        for (var i = 0; i < circleArray.length; i++) {
                circleArray[i].update();
// if (i ==0)
                // console.log(circleArray[i].dx);

        }
        // c.beginPath();
        // c.arc(x, y, radius, 0, Math.PI * 2, false);
        // c.strokeStyle = "purple";
        // c.stroke();

        //if (x + radius > innerWidth || x - radius < 0) {
        //          dx = -dx;
        //  }

        //  if (y + radius > innerHeight || y - radius < 0) {
        //          dy = -dy;
        //  }
        //  x += dx;
        //  y += dy;
}

animate();  
