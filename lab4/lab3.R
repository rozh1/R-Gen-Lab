library("rgenoud")

STACKERROR = 100
NOERROR = 0

dropHead = function(stack){
  if (length(stack)>1){
    stack = stack[1:(length(stack)-1)]
  } else {
    stack = numeric()
  }
}

execute = function(operators, stack=numeric()){
  for (oper in operators){
    #cat('stack:')
    #print(stack)
    #cat('oper:', oper, '\n')
    #print('')
    num = suppressWarnings(as.numeric(oper))
    if (!is.na(num)){
      if (length(stack)==0){
        stack = c(num)
      } else{
        stack = append(stack, num)
      }
    } else if (oper == '+')
    {
      # Есть ли два элемента в стеке?
      if (length(stack) < 2){
        # Сигнализируем об ошибке
        result = c(error=STACKERROR, len=NA, value=NA);
        return(result);
      }
      x1 = stack[length(stack)]
      stack = dropHead(stack)
      x2 = stack[length(stack)]
      stack = dropHead(stack)
      stack = append(stack, x1+x2)
    } else if (oper == '*')
    {
      # Есть ли два элемента в стеке?
      if (length(stack) < 2){
        # Сигнализируем об ошибке
        result = c(error=STACKERROR, len=NA, value=NA);
        return(result);
      }
      x1 = stack[length(stack)]
      stack = dropHead(stack)
      x2 = stack[length(stack)]
      stack = dropHead(stack)
      stack = append(stack, x1*x2)
    } else if (oper == '-')
    {
      # Есть ли два элемента в стеке?
      if (length(stack) < 2){
        # Сигнализируем об ошибке
        result = c(error=STACKERROR, len=NA, value=NA);
        return(result);
      }
      x1 = stack[length(stack)]
      stack = dropHead(stack)
      x2 = stack[length(stack)]
      stack = dropHead(stack)
      stack = append(stack, x2-x1)
    } else if (oper == '/')
    {
      # Есть ли два элемента в стеке?
      if (length(stack) < 2){
        # Сигнализируем об ошибке
        result = c(error=STACKERROR, len=NA, value=NA);
        return(result);
      }
      x1 = stack[length(stack)]
      stack = dropHead(stack)
      x2 = stack[length(stack)]
      stack = dropHead(stack)
      stack = append(stack, x2/x1)
    } else if (oper == 'dup')
    {
      # Есть ли элемент в стеке?
      if (length(stack) < 1){
        # Сигнализируем об ошибке
        result = c(error=STACKERROR, len=NA, value=NA);
        return(result);
      }
      x1 = stack[length(stack)]
      stack = append(stack, x1)
    } else if (oper == 'swap')
    {
      # Есть ли два элемента в стеке?
      if (length(stack) < 2){
        # Сигнализируем об ошибке
        result = c(error=STACKERROR, len=NA, value=NA);
        return(result);
      }
      x1 = stack[length(stack)]
      stack = dropHead(stack)
      x2 = stack[length(stack)]
      stack = dropHead(stack)
      stack = append(stack, x1)
      stack = append(stack, x2)
    } else if (oper == 'over')
    {
      # Есть ли два элемента в стеке?
      if (length(stack) < 2){
        # Сигнализируем об ошибке
        result = c(error=STACKERROR, len=NA, value=NA);
        return(result);
      }
      x1 = stack[length(stack)-1]
      stack = append(stack, x1)
    } else if (oper == 'nop')
    {
      
    }
  }
  result = c(error=NOERROR,
             len=length(stack),
             value=stack[length(stack)])
  return(result)
}

execute(c(12, 34, '+', '2', '*'))
execute(c(12, 34, '+', '2', '+'))
execute(c(12, 34, '+', '2', '-'))
execute(c(12, 34, '+', '2', '/'))
execute(c(12, 34, '+', '2', '*', '+'))
execute(c(12, 'dup', '+', '2', '*'))
execute(c(12, '3', 'swap'))
execute(c('3', 'dup'))
execute(c(12, '3', 'over'))
execute(c('over', 'swap'), stack=c(13,4))


data = read.csv(file="neurons_data.csv",header=TRUE,sep=";")
data
inputs = data[,1:4]
outputs = data[,5:7]

Num = 15*1

f <- function(xx)
{
  len = length(xx)/15
  summ=0
  
  inputLen = length(inputs[,1])
  outputColls = length(outputs[1,])
  
  for (inputIndex in 1:inputLen)
  {
    input = as.vector(as.matrix(inputs[inputIndex,]))
    for (i in 1:len)
    {
      weights = rep(0,15)
      for (j in 1:15)
      {
        weights[j] = xx[(i-1)*15+j]
      }
      
      output = Robot(xxInputs=input,xxWeights=weights)
      needOutput = as.vector(as.matrix(outputs[inputIndex,]))
      
      for(outputIndex in 1:outputColls)
      {
        summ = summ + abs(output[outputIndex] - needOutput[outputIndex])
      }
    }
  }
  return (summ)
}

dom = c(-1,1)
m = matrix(dom,nrow=Num,ncol=2,byrow=TRUE)

res = genoud(fn=f,max.generations=50,nvars=Num,Domains=m,pop.size=100,boundary.enforcement=2,BFGS=FALSE,P9=0)

resultWeights = res$par
resultWeights
f(resultWeights)


input = as.vector(as.matrix(inputs[1,]))

Robot(xxInputs=input,xxWeights=resultWeights)
as.vector(as.matrix(outputs[1,]))


input=c(0,0,0,0)
Robot(xxInputs=input,xxWeights=resultWeights)