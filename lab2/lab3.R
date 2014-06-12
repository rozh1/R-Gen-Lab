library("rgenoud")

neuron <- function(xxInputs, xxWeights)
{
  
  sum = xxWeights[1]
  
  for (i in 1:length(xxInputs))
  {
    sum=sum+xxWeights[i+1]*xxInputs[i]
  }
  
  return (1/(1+exp(-sum*5)))
}

Robot <- function(xxInputs, xxWeights)
{
  result = c(0,0,0)
  len = length(xxWeights)/5
  for (i in 1:len)
  {
    weights = c(xxWeights[(i-1)*5+1],
                xxWeights[(i-1)*5+2],
                xxWeights[(i-1)*5+3],
                xxWeights[(i-1)*5+4],
                xxWeights[(i-1)*5+5])
    result[i]=neuron(xxInputs,weights)
  }
  return (result)
}

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