library(rgp)

abalone = read.table("abalone_coded.data",sep=",",header=TRUE)
summary(abalone)

abalone$Length = (abalone$Length - mean(abalone$Length))/sd(abalone$Length)
abalone$Diameter = (abalone$Diameter - mean(abalone$Diameter))/sd(abalone$Diameter)
abalone$Height = (abalone$Height - mean(abalone$Height))/sd(abalone$Height)
abalone$Whole = (abalone$Whole - mean(abalone$Whole))/sd(abalone$Whole)
abalone$Shucked = (abalone$Shucked - mean(abalone$Shucked))/sd(abalone$Shucked)
abalone$Viscera = (abalone$Viscera - mean(abalone$Viscera))/sd(abalone$Viscera)
abalone$Shell = (abalone$Shell - mean(abalone$Shell))/sd(abalone$Shell)
#abalone$Rings = (abalone$Rings - min(abalone$Rings))/(max(abalone$Rings) - min(abalone$Rings))

functionSet1 <- functionSet("+", "-", "*", "/")
inputVariableSet1 <- inputVariableSet("F","M","I","Length","Diameter","Height","Whole","Shucked","Viscera","Shell")
constantFactorySet1 <- constantFactorySet(function() rnorm(1))

length = length(abalone$F);

train.ind = sample(1:length,2*length/3)
train = abalone[train.ind,]
val = abalone[-train.ind,]

lastError = 10000000

bestFunction = function(){}


fitnessFunction1 = function(f){
   t = f(train[,1],train[,2],train[,3],train[,4],train[,5],train[,6],train[,7],train[,8],train[,9],train[,10])
   diff = sum((abs(train[,11]-t))^2)
   if (is.na(diff)) {
     diff = 10000000
   }
   
   tmp = f(val[,1],val[,2],val[,3],val[,4],val[,5],val[,6],val[,7],val[,8],val[,9],val[,10])
   
   error = mean(abs(tmp - val[,11]))
   if (!is.na(error))
   {
     if (error<lastError) 
     {
       lastError <<- error
       bestFunction <<- f
     }
   }
   return(diff)
}

gpResult1 <- geneticProgramming(functionSet=functionSet1,
                                inputVariables=inputVariableSet1,
                                constantSet=constantFactorySet1,
                                fitnessFunction=fitnessFunction1,
                                populationSize=100,
                                eliteSize=15,
                                stopCondition=makeStepsStopCondition(300))

print(gpResult1$elite[1])

best1 <- gpResult1$population[[which.min(sapply(gpResult1$population, fitnessFunction1))]]

best1Compute = best1(val[,1],val[,2],val[,3],val[,4],val[,5],val[,6],val[,7],val[,8],val[,9],val[,10])
mean(abs(best1Compute - val[,11]))

best1Compute[1:10]
val[1:10,11]

print(bestFunction)

lastError

best2Compute = bestFunction(val[,1],val[,2],val[,3],val[,4],val[,5],val[,6],val[,7],val[,8],val[,9],val[,10])
best2Compute[1:10]
val[1:10,11]