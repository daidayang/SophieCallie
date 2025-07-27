var canvas = document.querySelector('canvas');
console.log(canvas);
canvas.width = window.innerWidth - 6;
canvas.height = window.innerHeight - 6;

var c = canvas.getContext('2d');



function draw_Calendar(NameOfMonth, start, days, tpx, tpy) {

    c.font = '100px serif';
    c.fillText(NameOfMonth, 100, 150);

    c.font = '40px serif';
    c.fillText('2022', 1400, 150);
    
    c.font = '40px serif';
    c.fillText('Sun', 150, 250);
    
    c.font = '40px serif';
    c.fillText('Mon', 350, 250);
    
    c.font = '40px serif';
    c.fillText('Tue', 550, 250);
    
    c.font = '40px serif';
    c.fillText('Wed', 750, 250);
    
    c.font = '40px serif';
    c.fillText('Thu', 950, 250);
    
    c.font = '40px serif';
    c.fillText('Fri', 1150, 250);
    
    c.font = '40px serif';
    c.fillText('Sat', 1350, 250);    


    let x = start;
    let y = 0;

    for (var day = 1; day <= days; day++) {

        c.font = '50px serif';
        c.fillText(day, x * 200 + 170, y * 100 + 325);

        x = x + 1;

        if (x >= 7) {
            x = 0;
            y = y + 1;
        }

    }

}

draw_Calendar('Feburary', 2, 28);  // Feb

// draw_Calendar('September', 4, 30);  // Sep
// draw_Calendar('December', 4, 31);  // Dec






// c.font = '50px serif';
// c.fillText('1', 570, 325);

// c.font = '50px serif';
// c.fillText('2', 770, 325);

// c.font = '50px serif';
// c.fillText('3', 970, 325);

// c.font = '50px serif';
// c.fillText('4', 1170, 325);

// c.font = '50px serif';
// c.fillText('5', 1370, 325);

// c.font = '50px serif';
// c.fillText('6', 170, 425);

// c.font = '50px serif';
// c.fillText('7', 370, 425);

// c.font = '50px serif';
// c.fillText('8', 570, 425);

// c.font = '50px serif';
// c.fillText('9', 770, 425);

// c.font = '50px serif';
// c.fillText('10', 960, 425);

// c.font = '50px serif';
// c.fillText('11', 1160, 425);

// c.font = '50px serif';
// c.fillText('12', 1360, 425);

// c.font = '50px serif';
// c.fillText('13', 160, 525);

// c.font = '50px serif';
// c.fillText('14', 360, 525);

// c.font = '50px serif';
// c.fillText('15', 560, 525);

// c.font = '50px serif';
// c.fillText('16', 760, 525);

// c.font = '50px serif';
// c.fillText('17', 960, 525);

// c.font = '50px serif';
// c.fillText('18', 1160, 525);

// c.font = '50px serif';
// c.fillText('19', 1360, 525);

// c.font = '50px serif';
// c.fillText('20', 160, 625);

// c.font = '50px serif';
// c.fillText('21', 360, 625);

// c.font = '50px serif';
// c.fillText('22', 560, 625);

// c.font = '50px serif';
// c.fillText('23', 760, 625);

// c.font = '50px serif';
// c.fillText('24', 960, 625);

// c.font = '50px serif';
// c.fillText('25', 1160, 625);

// c.font = '50px serif';
// c.fillText('26', 1360, 625);

// c.font = '50px serif';
// c.fillText('27', 160, 725);

// c.font = '50px serif';
// c.fillText('28', 360, 725);

// c.font = '50px serif';
// c.fillText('29', 560, 725);

// c.font = '50px serif';
// c.fillText('30', 760, 725);

// c.font = '50px serif';
// c.fillText('31', 960, 725);


















