# Sequential Logic Gates Implementation

## Table of Contents
- [About](#-About)
- [Bit](#-Bit)
- [Register](#-Register)
- [RAM8](#-RAM8)
- [RAM64](#-RAM64)
- [RAM512](#-RAM512)
- [RAM4K](#-RAM4K)
- [RAM16K](#-RAM16K)
- [PC](#-PC)

## About
- these are sequential logic gates so it needs a `Clock pulse` to work , so the circuit execute and store the data in a `full clock pulse` not just form `x+ to x`
- each chip will be explained as `Interface` and as `Implementation`
- `Interface` of the chip discuss how to deal with it
- `Implementation` of the chip discuss the inner implementation of the chip using previous implemented logic gates


## Bit
### Interface
- the chip has 2 input as 1-bit `in` and `load` and 1-bit output `out`
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
- using 16 [1-bit Register](#-Bit) to store the each bit of the 16 bit

### How To Use It
- to **Read** the value of the `Register` all what I have to do is to read the `out` output , because at any point of time the `out` output emits the value stored inside the `Register` 
- to **Write** `V` into the `Register` set `load = 1` and `in = V`




## RAM8
### Interface
### Implementation
### Summary

## RAM64
### Interface
### Implementation
### Summary

## RAM512
## RAM4K
## RAM16K
## PC
### Interface
### Implementation
### Summary
