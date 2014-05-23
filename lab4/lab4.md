Лабораторная работа №4
========================================================
Задание
--------------------------------------------------------

Haliotis — род моллюсков. Задача заключается в определении возраста особи по различным параметрам.

Надежный способ определения возраста состоит в том, что раковина моллюска разрезается и производится подсчет витков раковины. Другой, менее надежный, но более простой способ состоит в том, чтобы измерить различные параметры особи и по полученным данным попытаться предсказать ее возраст.

С помощью генетического алгоритма найти программу, которая будет определять возраст малюска на основе данных представленных в файле abalone.data

Классификация перменных
--------------------------------------------------------

Содержание файла abalone.data

```
M,0.455,0.365,0.095,0.514,0.2245,0.101,0.15,15
M,0.35,0.265,0.09,0.2255,0.0995,0.0485,0.07,7
F,0.53,0.42,0.135,0.677,0.2565,0.1415,0.21,9
M,0.44,0.365,0.125,0.516,0.2155,0.114,0.155,10
I,0.33,0.255,0.08,0.205,0.0895,0.0395,0.055,7
I,0.425,0.3,0.095,0.3515,0.141,0.0775,0.12,8
F,0.53,0.415,0.15,0.7775,0.237,0.1415,0.33,20
F,0.545,0.425,0.125,0.768,0.294,0.1495,0.26,16
M,0.475,0.37,0.125,0.5095,0.2165,0.1125,0.165,9
F,0.55,0.44,0.15,0.8945,0.3145,0.151,0.32,19
```

Как видно, первый столбец нуждается в кодировании, т.к. на входе должны быть числа. Значения являются дискретными и никак не свзяны, поэтому кодировать будем по каналам:
```
I=0,0,1
M=0,1,0
F=1,0,0
```

В результате получим файл с данными:

```
0,1,0,0.455,0.365,0.095,0.514,0.2245,0.101,0.15,15
0,1,0,0.35,0.265,0.09,0.2255,0.0995,0.0485,0.07,7
1,0,0,0.53,0.42,0.135,0.677,0.2565,0.1415,0.21,9
0,1,0,0.44,0.365,0.125,0.516,0.2155,0.114,0.155,10
0,0,1,0.33,0.255,0.08,0.205,0.0895,0.0395,0.055,7
0,0,1,0.425,0.3,0.095,0.3515,0.141,0.0775,0.12,8
1,0,0,0.53,0.415,0.15,0.7775,0.237,0.1415,0.33,20
1,0,0,0.545,0.425,0.125,0.768,0.294,0.1495,0.26,16
0,1,0,0.475,0.37,0.125,0.5095,0.2165,0.1125,0.165,9
1,0,0,0.55,0.44,0.15,0.8945,0.3145,0.151,0.32,19
```

Импортируем файл abalone_coded.data в R и посмотрим некоторую статистику

```r
abalone = read.table("abalone_coded.data", sep = ",", header = TRUE)
summary(abalone)
```

```
##        F               M               I             Length     
##  Min.   :0.000   Min.   :0.000   Min.   :0.000   Min.   :0.075  
##  1st Qu.:0.000   1st Qu.:0.000   1st Qu.:0.000   1st Qu.:0.450  
##  Median :0.000   Median :0.000   Median :0.000   Median :0.545  
##  Mean   :0.313   Mean   :0.366   Mean   :0.321   Mean   :0.524  
##  3rd Qu.:1.000   3rd Qu.:1.000   3rd Qu.:1.000   3rd Qu.:0.615  
##  Max.   :1.000   Max.   :1.000   Max.   :1.000   Max.   :0.815  
##     Diameter         Height          Whole          Shucked     
##  Min.   :0.055   Min.   :0.000   Min.   :0.002   Min.   :0.001  
##  1st Qu.:0.350   1st Qu.:0.115   1st Qu.:0.442   1st Qu.:0.186  
##  Median :0.425   Median :0.140   Median :0.799   Median :0.336  
##  Mean   :0.408   Mean   :0.140   Mean   :0.829   Mean   :0.359  
##  3rd Qu.:0.480   3rd Qu.:0.165   3rd Qu.:1.153   3rd Qu.:0.502  
##  Max.   :0.650   Max.   :1.130   Max.   :2.825   Max.   :1.488  
##     Viscera           Shell            Rings      
##  Min.   :0.0005   Min.   :0.0015   Min.   : 1.00  
##  1st Qu.:0.0935   1st Qu.:0.1300   1st Qu.: 8.00  
##  Median :0.1710   Median :0.2340   Median : 9.00  
##  Mean   :0.1806   Mean   :0.2388   Mean   : 9.93  
##  3rd Qu.:0.2530   3rd Qu.:0.3290   3rd Qu.:11.00  
##  Max.   :0.7600   Max.   :1.0050   Max.   :29.00
```


Нормируем столбцы кроме последнего:

```r
abalone$Length = (abalone$Length - mean(abalone$Length))/sd(abalone$Length)
abalone$Diameter = (abalone$Diameter - mean(abalone$Diameter))/sd(abalone$Diameter)
abalone$Height = (abalone$Height - mean(abalone$Height))/sd(abalone$Height)
abalone$Whole = (abalone$Whole - mean(abalone$Whole))/sd(abalone$Whole)
abalone$Shucked = (abalone$Shucked - mean(abalone$Shucked))/sd(abalone$Shucked)
abalone$Viscera = (abalone$Viscera - mean(abalone$Viscera))/sd(abalone$Viscera)
abalone$Shell = (abalone$Shell - mean(abalone$Shell))/sd(abalone$Shell)
```


Разделим все данные на обучающие, проверочные и тестовые:

```r
length = length(abalone$F)
train.ind = sample(1:length, length/3)
train = abalone[train.ind, ]
temp = abalone[-train.ind, ]
length = length(temp$F)
val.ind = sample(1:length, length/2)
val = temp[val.ind, ]
test = temp[-val.ind, ]
```


Подключим библиотеку rgd и инициализируем наборы данных

```r
library(rgp)
```

```
## Warning: package 'rgp' was built under R version 3.0.3
```

```
## *** RGP version 0.4-0 initialized successfully.  Type
## 'help(package="rgp")' to bring up the RGP help pages, or type
## 'vignette("rgp_introduction")' to show RGP's package vignette.  Type
## 'symbolicRegressionUi()' to bring up the symbolic regression UI if the
## optional package 'rgpui' is installed.
```

```r

functionSet1 <- functionSet("+", "-", "*", "/")
inputVariableSet1 <- inputVariableSet("F", "M", "I", "Length", "Diameter", "Height", 
    "Whole", "Shucked", "Viscera", "Shell")
constantFactorySet1 <- constantFactorySet(function() rnorm(1))
```


Создадим переменные для обучения с ранним остановом:

```r
lastError = 1e+07
bestFunction = function() {
}
```


Составим фитнес-функцию:

```r
fitnessFunction1 = function(f) {
    t = f(train[, 1], train[, 2], train[, 3], train[, 4], train[, 5], train[, 
        6], train[, 7], train[, 8], train[, 9], train[, 10])
    diff = sum((train[, 11] - t)^2)
    if (is.na(diff)) {
        diff = 1e+07
    }
    tmp = f(val[, 1], val[, 2], val[, 3], val[, 4], val[, 5], val[, 6], val[, 
        7], val[, 8], val[, 9], val[, 10])
    error = mean(abs(tmp - val[, 11]))
    if (!is.na(error)) {
        if (error < lastError) {
            lastError <<- error
            bestFunction <<- f
        }
    }
    return(diff)
}
```


Запустим алгоритм генетического программирования

```r
gpResult1 <- geneticProgramming(functionSet = functionSet1, inputVariables = inputVariableSet1, 
    constantSet = constantFactorySet1, fitnessFunction = fitnessFunction1, populationSize = 1000, 
    stopCondition = makeStepsStopCondition(3000))
```

```
## STARTING genetic programming evolution run (Age/Fitness/Complexity Pareto
## GP search-heuristic) ... evolution step 100, fitness evaluations: 4950,
## best fitness: 77913.564417, time elapsed: 10.1 seconds evolution step 200,
## fitness evaluations: 9950, best fitness: 17499.287571, time elapsed: 17.16
## seconds evolution step 300, fitness evaluations: 14950, best fitness:
## 12449.855071, time elapsed: 23.9 seconds evolution step 400, fitness
## evaluations: 19950, best fitness: 11062.485892, time elapsed: 30.61
## seconds evolution step 500, fitness evaluations: 24950, best fitness:
## 11062.485892, time elapsed: 38.41 seconds evolution step 600, fitness
## evaluations: 29950, best fitness: 9963.574748, time elapsed: 45.1 seconds
## evolution step 700, fitness evaluations: 34950, best fitness: 9871.279109,
## time elapsed: 55.02 seconds evolution step 800, fitness evaluations:
## 39950, best fitness: 9871.279109, time elapsed: 1 minute, 3.7 seconds
## evolution step 900, fitness evaluations: 44950, best fitness: 9871.279109,
## time elapsed: 1 minute, 11.03 seconds evolution step 1000, fitness
## evaluations: 49950, best fitness: 9871.279109, time elapsed: 1 minute,
## 18.14 seconds evolution step 1100, fitness evaluations: 54950, best
## fitness: 9871.279109, time elapsed: 1 minute, 26.33 seconds evolution step
## 1200, fitness evaluations: 59950, best fitness: 9871.279109, time elapsed:
## 1 minute, 34.82 seconds evolution step 1300, fitness evaluations: 64950,
## best fitness: 9871.279109, time elapsed: 1 minute, 41.68 seconds evolution
## step 1400, fitness evaluations: 69950, best fitness: 9871.279109, time
## elapsed: 1 minute, 46.77 seconds evolution step 1500, fitness evaluations:
## 74950, best fitness: 9871.279109, time elapsed: 1 minute, 52.4 seconds
## evolution step 1600, fitness evaluations: 79950, best fitness:
## 9086.394131, time elapsed: 2 minutes, 0.58 seconds evolution step 1700,
## fitness evaluations: 84950, best fitness: 8807.170582, time elapsed: 2
## minutes, 9.72 seconds evolution step 1800, fitness evaluations: 89950,
## best fitness: 8807.170582, time elapsed: 2 minutes, 14.96 seconds
## evolution step 1900, fitness evaluations: 94950, best fitness:
## 8807.170582, time elapsed: 2 minutes, 19.84 seconds evolution step 2000,
## fitness evaluations: 99950, best fitness: 8805.435796, time elapsed: 2
## minutes, 26.16 seconds evolution step 2100, fitness evaluations: 104950,
## best fitness: 8805.435796, time elapsed: 2 minutes, 32.99 seconds
## evolution step 2200, fitness evaluations: 109950, best fitness:
## 8805.435796, time elapsed: 2 minutes, 41.67 seconds evolution step 2300,
## fitness evaluations: 114950, best fitness: 8805.435796, time elapsed: 2
## minutes, 49.84 seconds evolution step 2400, fitness evaluations: 119950,
## best fitness: 8805.435796, time elapsed: 2 minutes, 58.17 seconds
## evolution step 2500, fitness evaluations: 124950, best fitness:
## 8804.025059, time elapsed: 3 minutes, 4.65 seconds evolution step 2600,
## fitness evaluations: 129950, best fitness: 8804.025059, time elapsed: 3
## minutes, 10.07 seconds evolution step 2700, fitness evaluations: 134950,
## best fitness: 8804.025059, time elapsed: 3 minutes, 17.44 seconds
## evolution step 2800, fitness evaluations: 139950, best fitness:
## 8804.025059, time elapsed: 3 minutes, 23.65 seconds evolution step 2900,
## fitness evaluations: 144950, best fitness: 8804.025059, time elapsed: 3
## minutes, 29.09 seconds evolution step 3000, fitness evaluations: 149950,
## best fitness: 8804.025059, time elapsed: 3 minutes, 35.44 seconds Genetic
## programming evolution run FINISHED after 3000 evolution steps, 149950
## fitness evaluations and 3 minutes, 35.44 seconds.
```


Лучший результат достигнут программой:

```r
print(gpResult1$elite[1])
```

```
## [[1]]
## function (F, M, I, Length, Diameter, Height, Whole, Shucked, 
##     Viscera, Shell) 
## Shell + (Shell - Shucked + Height) - (-3.45846145397903 - 6.50641773892362)
```


Проверим программу на тестовом множестве:

```r
best1 <- gpResult1$population[[which.min(sapply(gpResult1$population, fitnessFunction1))]]
best1Compute = best1(test[, 1], test[, 2], test[, 3], test[, 4], test[, 5], 
    test[, 6], test[, 7], test[, 8], test[, 9], test[, 10])
meanError = mean(abs(best1Compute - test[, 11]))
meanError
```

```
## [1] 1.69
```


Первые 10 результатов:

```r
best1Compute[1:10]
```

```
##  [1]  9.906  9.201 11.584 10.307  9.794 10.001  9.111 10.211  7.736  8.069
```

```r
test[1:10, 11]
```

```
##  [1]  9  9 19 14 11 10 10 12 11 10
```


Лучшая функция по результатам обучения с ранним остановом:

```r
print(bestFunction)
```

```
## function (F, M, I, Length, Diameter, Height, Whole, Shucked, 
##     Viscera, Shell) 
## Shell + (Shell - Shucked + Height) - (-3.07049010776546 - 6.50641773892362)
```


Средняя ошибка:

```r
best2Compute = bestFunction(test[, 1], test[, 2], test[, 3], test[, 4], test[, 
    5], test[, 6], test[, 7], test[, 8], test[, 9], test[, 10])
mean(abs(best2Compute - test[, 11]))
```

```
## [1] 1.634
```


Первые 10 результатов:

```r
best2Compute[1:10]
```

```
##  [1]  9.518  8.813 11.196  9.919  9.406  9.613  8.723  9.823  7.348  7.681
```

```r
test[1:10, 11]
```

```
##  [1]  9  9 19 14 11 10 10 12 11 10
```

