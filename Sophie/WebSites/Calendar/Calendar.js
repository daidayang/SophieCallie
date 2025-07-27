var canvas = document.querySelector('canvas');

canvas.width = window.innerWidth     //width
canvas.height = window.innerHeight   //height

var c = canvas.getContext('2d');

function drawmonth(days, x, y, ori_x, ori_y, monthname) {
    c.font = "15px Arial"
    c.fillText("Su", 100, 110, 140, 140)
    c.fillText("Mo", 150, 110, 140, 140)
    c.fillText("Tu", 200, 110, 140, 140)
    c.fillText("We", 250, 110, 140, 140)
    c.fillText("Th", 300, 110, 140, 140)
    c.fillText("Fr", 350, 110, 140, 140)
    c.fillText("Sa", 400, 110, 140, 140)
    c.font = "15px Arial"
    for (i = 1; i <= days; i++) {
        c.fillText(i, 50 * x, 50 * y, 140, 140)
        if (x == 8) {
            y++
            x = 1
        }
        x++
    }
    c.font = "20px Arial"
    c.fillText(monthname, 215 + ori_x, 55 + ori_y, 140, 140)
}
drawmonth(32, 3, 3, 4, 3, "January")

//8/5/2021 Calendar Assignment ez


