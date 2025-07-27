function ButtonClicked(name)
{

    if (name=='Vivian') {
        document.getElementById('myImg').src="https://media.discordapp.net/attachments/843657512027947009/850904119078289438/unknown.png"
    } 

    if (name=='Maid'){
        document.getElementById('myImg').src="https://media.discordapp.net/attachments/847961358112260106/847974492621111326/unknown.png"
    }

    if (name=='Life') {
        document.getElementById('myImg').src="https://th.bing.com/th/id/OIP.yaH7RuVRB1Z1rO7YqsmSUAHaEK?w=306&h=180&c=7&o=5&pid=1.7"
    }

    if (name=='KOS') {
        document.getElementById('myImg').src="https://media.discordapp.net/attachments/843647972541726772/850538064778362930/IMG_3615.JPG?width=438&height=584"
    }

    if (name=='Jamie') {
        document.getElementById('myImg').src="https://th.bing.com/th/id/OIP.KU3ojVa3zQ8yMajlPqgOOgHaHa?w=196&h=196&c=7&o=5&pid=1.7"
    }

    if (name=='Roundy') {
        document.getElementById('myImg').src="https://media.discordapp.net/attachments/843657512027947009/850904869783076924/Screenshot_2021-03-17_134007.png?width=635&height=584"
    }
}


function RadioClicked(name)
{

    if (name=='Shrek') {
        document.getElementById('myImg').src="https://th.bing.com/th/id/R96ac08198af32d1f92fa477c3e7cdd0c?rik=F7RUfpE0cS%2bZ4g&pid=ImgRaw"
    } 

    if (name=='Turtle'){
        document.getElementById('myImg').src="https://sep.yimg.com/ay/yhst-33477391359232/western-painted-turtles-chrysemys-picta-bellii-7.jpg"
    }

    if (name=='Callie') {
        document.getElementById('myImg').src="https://media.discordapp.net/attachments/843647972541726772/850538064778362930/IMG_3615.JPG?width=438&height=584"
    }

    if (name=='Mommy') {
        document.getElementById('myImg').src="https://media.discordapp.net/attachments/843647972541726772/850537465777487882/unknown.png"
    }

    if (name=='Daddy') {
        document.getElementById('myImg').src="https://th.bing.com/th/id/OIP.VYFzpO-18T4VbgBKTHrH_QHaJF?w=142&h=180&c=7&o=5&pid=1.7"
    }

    if (name=='Sophie') {
        document.getElementById('myImg').src="https://media.discordapp.net/attachments/843647972541726772/850538802418155560/IMG_4547.JPG?width=438&height=584"
    }
}


function enable()
{
    document.getElementById('MyOptions')

    const element = document.getElementById("MyOptions");
    const checkValue = element.options[element.selectedIndex].value;

    console.log(element.selectedIndex)
    console.log(element.options)


    console.log(checkValue)

    const msg = `no ${checkValue} for you noob`
    console.log(msg)

    if (checkValue=='Shrek') {
        document.getElementById('myImg').src="https://th.bing.com/th/id/R96ac08198af32d1f92fa477c3e7cdd0c?rik=F7RUfpE0cS%2bZ4g&pid=ImgRaw"
        alert('Shrek')    
    } 

    if (checkValue=='Turtle'){
        document.getElementById('myImg').src="https://sep.yimg.com/ay/yhst-33477391359232/western-painted-turtles-chrysemys-picta-bellii-7.jpg"
        alert('Turtle')
    }


    if (checkValue=='') {
        document.getElementById('myImg').src=''
    }
    
    if (checkValue=='Callie') {
        document.getElementById('myImg').src="https://media.discordapp.net/attachments/843647972541726772/850538064778362930/IMG_3615.JPG?width=438&height=584"
        alert('Callie')
    }

    if (checkValue=='Mommy') {
        document.getElementById('myImg').src="https://media.discordapp.net/attachments/843647972541726772/850537465777487882/unknown.png"
        alert('Mommy')
    }

    if (checkValue=='Daddy') {
        document.getElementById('myImg').src="https://th.bing.com/th/id/OIP.VYFzpO-18T4VbgBKTHrH_QHaJF?w=142&h=180&c=7&o=5&pid=1.7"
        alert('Daddy')
    }

    if (checkValue=='Sophie') {
        document.getElementById('myImg').src="https://media.discordapp.net/attachments/843647972541726772/850538802418155560/IMG_4547.JPG?width=438&height=584"
        alert('Sophie')
    }

}