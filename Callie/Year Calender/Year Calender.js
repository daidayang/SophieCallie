var canvas = document.querySelector('canvas');
console.log(canvas);
canvas.width = window.innerWidth - 6;
canvas.height = window.innerHeight + 15;

var c = canvas.getContext('2d');




function draw_Calendar(tl_x, tl_y, NameOfMonth, start, days) {


    c.font = '25px serif';
    c.fillText(NameOfMonth, tl_x+150, tl_y-40);

    c.beginPath();
    c.moveTo(tl_x+45, tl_y+5);
    c.lineTo(tl_x+370, tl_y+5);
    c.stroke();

    c.font = '20px serif';
    c.fillText('S', tl_x+50, tl_y-10);
    c.fillText('M', tl_x+100, tl_y-10);
    c.fillText('T', tl_x+150, tl_y-10);
    c.fillText('W', tl_x+200, tl_y-10);
    c.fillText('T', tl_x+250, tl_y-10);
    c.fillText('F', tl_x+300, tl_y-10);
    c.fillText('S', tl_x+350, tl_y-10);    

    let x = start;
    let y = 0;

    for (var day = 1; day <= days; day++) {

        c.font = '20px serif';
        c.fillText(day, x * 50 + tl_x+50, y * 25 + tl_y+30);

        x = x + 1;

        if (x >= 7) {
            x = 0;
            y = y + 1;
        }
    }
}

draw_Calendar(0, 150, 'January', 6, 31);  
draw_Calendar(390, 150, 'Feburary', 2, 28);
draw_Calendar(790, 150, 'March', 2, 31);
draw_Calendar(1190, 150, 'April', 5, 30);
draw_Calendar(0, 375, 'May', 0, 31);  
draw_Calendar(390, 375, 'June', 3, 30);
draw_Calendar(790, 375, 'July', 5, 31);
draw_Calendar(1190, 375, 'August', 1, 31);
draw_Calendar(0, 600, 'September', 4, 30);  
draw_Calendar(390, 600, 'October', 6, 31);
draw_Calendar(790, 600, 'November', 2, 30);
draw_Calendar(1190, 600, 'December', 4, 31);

    c.font = '50px serif';
    c.fillText('2022 Calender', 625, 70);



