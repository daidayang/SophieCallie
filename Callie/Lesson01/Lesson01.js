console.log('i am cool')
const v1 = 'callie is cool'
let v2 = 'ajh, gfv, kjh, fsdk, ajd, hg';

let v3 = v2.split(',');
console.log(v2.toUpperCase())
console.log(v3)
console.log(v3[1])
v3[2] = 'dog'
v3.unshift('cat')
v3.pop()
console.log(v3)

console.log(v1)
let odds = [1, 3, 5, 7, 9]
console.log(odds)

const sisters = [{
    FirstName: 'Sophie',
    LastName: 'Dai',
    Grade: 8,
    Age: 14,
    Gender: 'F',
    Address: {
        Street: '3023 Old Masters Dr',
        Zip: 77479,
        City: 'Sugar Land',
    },
    Phone: '326-327-9686',
    Gmail: 'catsphene@gmai.com',
},
{
    FirstName: 'Callie',
    LastName: 'Dai',
    Grade: 5,
    Age: 10,
    Gender: 'F',
    Address: {
        Street: '3023 Old Masters Dr',
        Zip: 77479,
        City: 'Sugar Land',
    },
    Phone: '281-744-0555',
    Gmail: 'calliejiayidai@gmail.com',
}
]

console.log(sisters)
console.log(sisters[1].FirstName)
console.log(sisters[0].Gmail)
console.log(sisters[0].Address.Zip)
sisters[0].FavoriteFoods = ['ytt', 'Spicy Beef', 'ym']
sisters[1].FavoriteFoods = ['Pho', 'Sushi', 'Sugary Stuff', 'OLIVE GARDEN']
sisters[0].Age = 20
console.log(sisters)
console.log(sisters[1].FavoriteFoods[3])
console.log(sisters[0].FavoriteFoods[0])

const Books = [{
    Title: 'Tuck Everlasting',
    Author: 'Natalie Babbit',
    Pages: 139,
    Price: 4.99,
},
{
    Title: 'Boy Tales of Childhood',
    Author: 'Roald Dahl',
    Pages: 176,
    Price: 7.99,
}
]
console.log(Books)

const Clothes = [{
    Type: 'Jacket',
    Color: 'Red',
    Cost: 14.99,
    Size: 'Small',
},
{
    Type: 'Jeans',
    Color: 'blue',
    Cost: 9.99,
    Size: 'Large',
}
]
console.log(Clothes)

// var b = 5
// var t = 0;
// for ( var i = b; i < 65; i=i+5+t ) {
//     console.log(i);
//     t = t + 1;
// }



// for ( var i = 15; i > 0; i=i-5 ) {
//     console.log('-------');
//     console.log(i);
//     console.log('-------');

//     for ( var j = 0; j < 5; j=j+1 ) {
//         console.log(j);
//     }
// }

// for ( var z = 3; z < 34; z = z + 10){

//     console.log (z);
// }

// for ( var y = 10; y < 31; y = y + 10){
//     console.log ("-----");
//     console.log (y);
//     console.log ('-----');

//     for ( var x = 1; x < 4; x = x + 1){

//         console.log (' ' + x);
//     }
// }

// for ( var y = 1; y < 4; y = y + 1){
//     console.log ("-----");
//     console.log (y);
//     console.log ('-----');

//     for ( var x = 1; x < 4; x = x + 1){
//         console.log (' ' + (x+y-1));
//     }
// }


// for ( var y = 1; y < 4; y = y + 1){
//     console.log ("-----");
//     console.log (y);
//     console.log ('-----');

//     for ( var x = y-2; x < y+6; x = x + y){
//         console.log (' ' + x);
//     }
// }

// for ( var a = 1; a < 4; a = a + 1){
//     console.log ("---");
//     console.log (a);
//     console.log ('---');

//     for ( var b = a+5; b < a+8; b = b + 1){
//         console.log (' ' + b);
//     }
// }

for (var t = 3; t < 6; t = t + 1) {
    console.log('---');
    let v = t
    if (t > 4) {
        console.log('> 4');
        v = v + 6
        console.log(v);
    }
    else {
        console.log('<= 4');
        v = v - 6
        console.log(v);
    }
}

console.log('\\ /');
console.log('O O ');
console.log('-~-')