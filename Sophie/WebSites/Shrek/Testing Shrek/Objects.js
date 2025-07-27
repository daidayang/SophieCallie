
const Callie = {
    Firstname: 'Callie',
    Lastname: 'Dai',
    nickname: 'weirdo',
    middlename: 'MingHui',
    Age: 9,
    Hobbies: ['watching phone', 'playing roblox', '跟屁蟲', 'likes goldfish'],
    address: {
        street: '3023 old masters dr',
        city: 'Sugarland',
        State: 'TX',
        Zip: '77479'
    }
}
console.log(Callie.Hobbies[1]) //playing roblox
console.log(Callie.address.State) //TX
console.log(Callie.Hobbies[2]) //genpichong
Callie.address.state = 'Texas'   //To change the value of the attribute called state 
console.log(Callie.address.state) //Texas
//const { firstname, lastname } = person;    not a very common method; it pulls out attributes and makes them their own variable

const todos = [
    {
        id: 1,
        text: 'Tell sophie shes the best person EVER',
        Iscompleted: true
    },
    {
        id: 2,
        text: 'Tell daddy he is the kind of best person EVER',
        Iscompleted: false
    },
    {
        id: 3,
        text: 'Tell mommy shes the best person EVER',
        Iscompleted: true
    },

]
console.log(todos[1])
console.log(todos[1].id) //id value


const ApplicationForm = {
    Applicant_Information: {
        Lastname: 'Dai',
        Firstname: 'Dayang',

        Date_Of_Birth: '06/04/1967',
        Sex: 'Male',  //    Female
        Height_In_Ft: 5,
        Height_In_In: 11,
        Eye_Color: 'Brown', //   Blue, 
    },
    Contact_Information: {
        Residence_Address: '3023 Old Masters Dr',
        Residence_City: 'Sugar Land',

    }    
}

const Sophie = {
    Firstname: 'Sophie',
    Lastname: 'Dai',
    Gender: 'Female',
    Height: '5,2',
    Age: 13,
    FavoriteAnimal: 'cat',

}

Callie.Sister = Sophie;

const kids = [
    Sophie, Callie
]

console.log(kids[1].Firstname)
// console.log(kids[0].Sister.Firstname)
// console.log(kids[1].Sister.Firstname)


const Family = {
    Mommy : {
        Firstname: 'Fang',
        Lastname: 'Li',
        Gender: 'Female',
        FavoriteAnimal: 'Tiger',
        Children: Sophie, Callie
    },
    Daddy : {
        Firstname: 'Dayang',
        Lastname: 'Dai',
        Gender: 'Male',
        FavoriteAnimal: undefined,
    },

    Callie,

    Sophie
}

console.log(Family)

//end everything with comma thingie UwU
//start with const
//To convert objextarrary into json format - const.example=JSON.stringify(example);

const familyJSON = JSON.stringify(Family)
console.log(familyJSON)