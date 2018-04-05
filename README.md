# Xchange

### Introduction

Interpreter of simple programming language with currency types. 
Parsing visualizator can used to debugging and learning basic concepts of interpreters implementation. 
Exchanges rates are downloaded from the Web.

### BNF grammar notation

<pre>
  <b>program</b> ​= { functionDefinition } . 
  <b>functionDefinition</b> ​= "function" id "(" parameters ")" statementBlock . 
  <b>parameters</b> ​= [ id { "," id } ] . 
  <b>arguments</b> ​= [ assignable [castOp] { "," assignable [castOp] } ] . 
  <b>statementBlock</b>​ = "{" { instructions } "}" . 
  <b>instructions</b> = ifStatement   | returnStatement | whileStatement |  
               initStatement | assignStatement | functionCall   |    
               breakCommand  | continueCommand | statementBlock 


  <b>ifStatement</b> ​= "if" "(" generalCond ")" statementBlock [ "else" statementBlock]. 
  <b>returnStatement</b> ​= "return" assignable ";" . 
  <b>whileStatement​</b> = "while" "(" generalCond ")" statementBlock . 
  <b>initStatement​</b> = type id [ assignmentOp assignable ] ";" . 
  <b>assignStatement​</b> = id assignmentOp assignable ";" . 
  <b>functionCall</b> ​= id "(" arguments ")" ";" .
  <b>breakCommand</b> = "break" ";" .
  <b>continueCommand</b> = "continue" ";" . 

  <b>assignable</b> ​= functionCall [ castOp ] | generalExpr .
  <b>generalExpr</b> ​= multiplicativeExpr {additiveOp multiplicativeExpr} [castOp].
  <b>multiplicativeExpr</b> ​= baseExpr { multiplicativeOp baseExpr } . 
  <b>baseExpr</b>​ = ( number | castableId | nestedExpr ) .
  <b>nestedExpr</b> ​= "(" generalExpr ")" .

  <b>generalCond</b> ​= andCond { orOp andCond } . 
  <b>andCond</b>​ = equalityCond { andOp equalityCond } . 
  <b>equalityCond​</b> = relationalCond { equalityOp relationalCond } . 
  <b>relationalCond</b> ​= baseCond { relationalOp​ baseCond } . 
  <b>baseCond​</b> = [ negationOp ] ( nestedCond | castableId | number ) . 
  <b>nestedCond​</b> = "(" generalCond ")" . 

  <b>negationOp</b> = "!" . 
  <b>assignmentOp</b> ​= "=" . 
  <b>orOp​</b> = "||" . 
  <b>andOp</b> ​= "&&" . 
  <b>equalityOp​</b> = "==" | "!=" . 
  <b>relationalOp</b>​ = "<" | ">" | "<=" | ">=" . 
  <b>additiveOp</b> ​= "+" | "-­" .   
</pre>

### Visualization of parsing

Parsing of correct script and incorrect one :
![parsing](https://i.imgur.com/gcmshSp.png)

### Examples of correct scripts

<pre>
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
</pre>


