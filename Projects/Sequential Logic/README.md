## About

- these are sequential logic gates so it needs a `Clock pulse` to work , so the circuit execute and store the data in a `full clock pulse` not just form `x+ to x`
- each chip will be explained as `Interface` and as `Implementation`
- `Interface` of the chip discuss how to deal with it
- `Implementation` of the chip discuss the inner implementation of the chip using previous implemented logic gates

## Bit

### Interface

- the chip has 2 input as 1-bit which are `in` and `load` and 1-bit output `out`
- `in` represent the current input to store it in the bit register
- `load` a control bit used to control how the `1-bit register` work , if `load = 1` then the `1-bit register` value is set to `in` , but if `load = 0` then the `1-bit register` maintain its value
- `out` represent the current state of the `1-bit register`

### Implementation

- using `MUX 2x1` and feed it using the `1-bit register value` and `current input` and i will use `load` bit as a selector to decide the output of the `MUX`, if `load=0` the MUX output will be `DFF output` , if `load=1` the MUX output will be `current input`
- the output of the `MUX` which represent the value that i want to store in the `1-bit register` will be used as the input of the `DFF`

## Register

### Interface

- the chip has 2 input as a 16-bit `in` and a 1-bit `load` and a 16-bit output `out`
- `in` represent the current input to store it in the register
- `load` a control bit used to control how the `16-bit register` work , if `load = 1` then the `16-bit register` value is set to `in` , but if `load = 0` then the `16-bit register` maintain its value or the previous output
- `out` represent the current state of the `16-bit register`

### Implementation

- using 16 [1-bit Register](#bit) to store the each bit of the 16 bit

### How To Use It

- to **Read** the value of the `Register` all what I have to do is to read the `out` output , because at any point of time the `out` output emits the value stored inside the `Register`
- to **Write** `V` into the `Register` set `load = 1` and `in = V`

## RAM8

### Interface

- the chip compose of [8 16-bit Register](#register)
- the chip take [16-bit Register](#register) as input and `1-bit load` and `3-bit as address`
- it has an `16-bit output`

```
if(load == 1)
  Register[Address] = in
  out = Register[address]
else
  don't change any register values and keep it as it's
  out = Register[address]
```

- the purpose of `Load` :

  - load = 1 -> write to specific register
  - load = 0 -> read from the specific register

- the reading behavior ins't related to clock pulse

  - it just read the value already stored in the Register

- the writing behavior is related to the clock pulse

### Implementation

- `Dmux 1*8` : to select which register to load the input based on the address because i have `3-bit address`
- `8 16-bit register` : used to store the output of the Data
- `Mux 8*1` : used to decide which register is chosed to emit it's value as output based on the address

## PC

### Interface

- it take `16-bit input` and 3 control bits
- it `out` the result based on the control bits

### Implementation

```
A 16-bit counter.
if      reset(t):  out(t+1) = 0
else if load(t) :  out(t+1) = in(t)
else if inc(t)  :  out(t+1) = out(t) + 1
else               out(t+1) = out(t)
```

- i have 4 operation here based on the description above which are :

  1. load in(x)
  2. load output(y)
  3. increment output(y)
  4. reset the register

- so i draw a truth table based on the 3-bit control input to deduce each operation

  | reset(R) | load(L) | inc(I) | loadX(lx) | incY(iy) | reset(r) | loadY(ly) |
  | -------- | ------- | ------ | --------- | -------- | -------- | --------- |
  | 0        | 0       | 0      | 0         | 0        | 0        | 1         |
  | 0        | 0       | 1      | 0         | 1        | 0        | 0         |
  | 0        | 1       | 0      | 1         | 0        | 0        | 0         |
  | 0        | 1       | 1      | 1         | 0        | 0        | 0         |
  | 1        | 0       | 0      | 0         | 0        | 1        | 0         |
  | 1        | 0       | 1      | 0         | 0        | 1        | 0         |
  | 1        | 1       | 0      | 0         | 0        | 1        | 0         |
  | 1        | 1       | 1      | 0         | 0        | 1        | 0         |

  - laodX(lx) = R`.L
  - incY(iy) = R\`.L`.I
  - reset(r) = R
  - loadY(ly) = R\`.l\`.I`

  ***

- i will use `ALU` to perform the operations based on the controls bits
- so i will feed the control bits with the `counter` control bits
  | out | zx | nx | zy | ny | f | no |
  | --- | --- | --- | --- | --- | --- | --- |
  | x | 0 | 0 | lx | lx | 0 | 0 |
  | y | ly | ly | 0 | 0 | 0 | 0 |
  | y+1 | iy | iy | 0 | iy | iy | iy |
  | 0 | r | 0 | r | 0 | r | 0 |

  - zx = ly + iy + r
  - nx = ly + iy
  - zy = lx + r
  - ny = lx + iy
  - f = iy + r
  - no = iy

  ***

- the load bit in the register will be :
- load = inc + load + reset
- if it's one it will load the input otherwise it will keep its data
