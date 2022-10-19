// alert()
const display1El = document.querySelector('.display-1');
const display2El = document.querySelector('.display-2');
const tempResultEl = document.querySelector('.temp-result');
const numbersEl = document.querySelectorAll('.number');
const operationEl = document.querySelectorAll('.operation');
const equalEl = document.querySelectorAll('.equal');
const clearAllEl = document.querySelectorAll('.all-clear');
const clearlastEl = document.querySelector('.last-entity-clear')

let dis1Num = '';
let dis2Num = '';
let result = null;
let lastOperation = '';
let haveDot = false;

numbersEl.forEach(number => {
    number.addEventListener('click', (e) => {
        if(e.target.innerText === '.' && !haveDot){
            haveDot = true;
        }else if(e.target.innerText === '.' && haveDot){
            return;
        }
        dis2Num += e.target.innerText;
        display2El.innerText = dis2Num;
    })
})

operationEl.forEach(operation =>{
    operation.addEventListener('click', (e) =>{
      if (!dis2Num) result;
      haveDot = false;
      const operationName = e.target.innerText;
      if(dis1Num && dis2Num && lastOperation){
        mathOperation();
      } else{
        result = parseFloat(dis2Num);
      }
      clearVar(operationName);
      lastOperation = operationName;
      console.log(result);
    })
});

function clearVar(name = ''){
    dis1Num += dis2Num+ ' ' + name + '',
    display1El.innerText = dis1Num;
    display2El.innerText = ' ';
    dis2Num = '';
    tempResultEl.innerText = result;
}

function mathOperation(){
    if(lastOperation === 'X'){
        result = parseFloat(result) * parseFloat(dis2Num);
    } else if(lastOperation === '+'){
        result = parseFloat(result) + parseFloat(dis2Num);
    } else if(lastOperation === '-'){
        result = parseFloat(result) - parseFloat(dis2Num);
    } else if(lastOperation === '/'){
        result = parseFloat(result) / parseFloat(dis2Num);
    } else if(lastOperation === '%'){
        result = parseFloat(result) % parseFloat(dis2Num);
    }
}

equalEl.addEventListener('click', (e)=>{
    haveDot = false;
    mathOperation();
    clearVar();
    display2El.innerText = result;
    tempResultEl.innerText = '';
    dis2Num = result;
    dis1Num = '';
})

 
// clearAllEl.addEventListener('click', (e) => {
//     display1El.innerText = '';
//     display2El.innerText = '';
//     dis1Num = '';
//     dis2Num = '';
//     result = '';
//     tempResultEl.innerText = ' '
// })