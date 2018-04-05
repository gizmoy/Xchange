# Xchange

### Introduction

Interpreter of simple programming language with currency types. 
Parsing visualizator can used to debugging and learning basic concepts of interpreters implementation. 
Exchanges rates are downloaded from the Web.

### BNF notation of grammar

`
program ​= { functionDefinition } .

functionDefinition ​= "function" id "(" parameters ")" statementBlock . 
parameters ​= [ id { "," id } ] . 
arguments ​= [ assignable [castOp] { "," assignable [castOp] } ] . 
statementBlock​ = "{" { instructions } "}" . 
instructions = ifStatement   | returnStatement | whileStatement |  
               initStatement | assignStatement | functionCall   |    
               breakCommand  | continueCommand | statementBlock 


ifStatement ​= "if" "(" generalCond ")" statementBlock [ "else" statementBlock]. 
returnStatement ​= "return" assignable ";" . 
whileStatement​ = "while" "(" generalCond ")" statementBlock . 
initStatement​ = type id [ assignmentOp assignable ] ";" . 
assignStatement​ = id assignmentOp assignable ";" . 
functionCall ​= id "(" arguments ")" ";" .
breakCommand = "break" ";" .
continueCommand = "continue" ";" . 

assignable ​= functionCall [ castOp ] | generalExpr .
generalExpr ​= multiplicativeExpr {additiveOp multiplicativeExpr} [castOp].
multiplicativeExpr ​= baseExpr { multiplicativeOp baseExpr } . 
baseExpr​ = ( number | castableId | nestedExpr ) .
nestedExpr ​= "(" generalExpr ")" .

generalCond ​= andCond { orOp andCond } . 
andCond​ = equalityCond { andOp equalityCond } . 
equalityCond​ = relationalCond { equalityOp relationalCond } . 
relationalCond ​= baseCond { relationalOp​ baseCond } . 
baseCond​ = [ negationOp ] ( nestedCond | castableId | number ) . 
nestedCond​ = "(" generalCond ")" . 

negationOp = "!" . 
assignmentOp ​= "=" . 
orOp​= "||" . 
andOp ​= "&&" . 
equalityOp​ = "==" | "!=" . 
relationalOp​ = "<" | ">" | "<=" | ">=" . 
additiveOp ​= "+" | "-­" . 
`

### Visualization of parsing

Parsing of correct script and incorrect one :
![parsing](https://i.imgur.com/gcmshSp.png)

### Examples of correct scripts

`
function factorial(n)
{
	if (n == 1)
	{
		return 1;
	}
	
	return n * factorial(n-1);
}

function pow(num, exp)
{
	var i = 0;
	var result = 1;

	while (1)
	{
		if (i >= exp)
		{
			break;
		}

		result = result * num;

		i = i + 1;
	}

	return result;
}

function sum(a, b)
{
	var i = 0;
	var sum = 0;

	while (i < a)
	{
		sum = sum + b;
		i = i + 1;
	}

	return sum;
}

function main()
{
	printLine(factorial(4));
	printLine(pow(2, 10));
	printLine(sum(5, 100.usd));
	printLine(sum(5, 100).usd);

	return 0;
}
`


