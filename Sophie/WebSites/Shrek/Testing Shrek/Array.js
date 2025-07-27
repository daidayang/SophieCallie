// Array -  Variables that hold multiple values
console.log('************** Array Test ***************')
const Fluffster = ['fluffy', 'pink', 'murderous']
console.log('Fluffster is')
console.log(Fluffster);
const OddNumbers = [1,3,4509,5,9]
console.log(OddNumbers)
// Arrays are zero based
console.log(OddNumbers[2])
OddNumbers[2]=7
console.log(OddNumbers)
// Array push (To Add To The Back)
OddNumbers.push(15)
console.log(OddNumbers)
// Array Unshift (To Add To The Beginning)
OddNumbers.unshift(-1)
console.log(OddNumbers)
// Array Pop (To Get Rid Of The End)
OddNumbers.pop()
console.log(OddNumbers)
// Array IndexOf (Check Position of The Value Inside An Array)
console.log(OddNumbers.indexOf(7))