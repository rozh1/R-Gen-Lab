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


Разделим все данные на обучающие и проверочные:

```r
length = length(abalone$F)

train.ind = sample(1:length, 2 * length/3)
train = abalone[train.ind, ]
val = abalone[-train.ind, ]
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
## best fitness: 95439.218680, time elapsed: 14.36 seconds evolution step
## 200, fitness evaluations: 9950, best fitness: 30114.186164, time elapsed:
## 25.25 seconds evolution step 300, fitness evaluations: 14950, best
## fitness: 20270.294400, time elapsed: 35.93 seconds evolution step 400,
## fitness evaluations: 19950, best fitness: 19198.055230, time elapsed:
## 47.01 seconds evolution step 500, fitness evaluations: 24950, best
## fitness: 17414.210622, time elapsed: 57.91 seconds evolution step 600,
## fitness evaluations: 29950, best fitness: 17002.166028, time elapsed: 1
## minute, 7.76 seconds evolution step 700, fitness evaluations: 34950, best
## fitness: 16709.795267, time elapsed: 1 minute, 15.62 seconds evolution
## step 800, fitness evaluations: 39950, best fitness: 15894.061301, time
## elapsed: 1 minute, 23.37 seconds evolution step 900, fitness evaluations:
## 44950, best fitness: 15894.061301, time elapsed: 1 minute, 30.77 seconds
## evolution step 1000, fitness evaluations: 49950, best fitness:
## 15894.061301, time elapsed: 1 minute, 35.96 seconds evolution step 1100,
## fitness evaluations: 54950, best fitness: 15894.061301, time elapsed: 1
## minute, 42.2 seconds evolution step 1200, fitness evaluations: 59950, best
## fitness: 15894.061301, time elapsed: 1 minute, 47.19 seconds evolution
## step 1300, fitness evaluations: 64950, best fitness: 15894.061301, time
## elapsed: 1 minute, 53.45 seconds evolution step 1400, fitness evaluations:
## 69950, best fitness: 15894.061301, time elapsed: 2 minutes, 0.83 seconds
## evolution step 1500, fitness evaluations: 74950, best fitness:
## 15894.061301, time elapsed: 2 minutes, 8.25 seconds evolution step 1600,
## fitness evaluations: 79950, best fitness: 15894.061301, time elapsed: 2
## minutes, 15.07 seconds evolution step 1700, fitness evaluations: 84950,
## best fitness: 15894.061301, time elapsed: 2 minutes, 19.53 seconds
## evolution step 1800, fitness evaluations: 89950, best fitness:
## 15894.061301, time elapsed: 2 minutes, 25.48 seconds evolution step 1900,
## fitness evaluations: 94950, best fitness: 15894.061301, time elapsed: 2
## minutes, 31.47 seconds evolution step 2000, fitness evaluations: 99950,
## best fitness: 15894.061301, time elapsed: 2 minutes, 38.08 seconds
## evolution step 2100, fitness evaluations: 104950, best fitness:
## 15894.061301, time elapsed: 2 minutes, 44.73 seconds evolution step 2200,
## fitness evaluations: 109950, best fitness: 15854.810400, time elapsed: 2
## minutes, 51.29 seconds evolution step 2300, fitness evaluations: 114950,
## best fitness: 15854.810400, time elapsed: 2 minutes, 56.82 seconds
## evolution step 2400, fitness evaluations: 119950, best fitness:
## 15854.810400, time elapsed: 3 minutes, 3.35 seconds evolution step 2500,
## fitness evaluations: 124950, best fitness: 15854.810400, time elapsed: 3
## minutes, 9.97 seconds evolution step 2600, fitness evaluations: 129950,
## best fitness: 15854.810400, time elapsed: 3 minutes, 16.69 seconds
## evolution step 2700, fitness evaluations: 134950, best fitness:
## 15854.810400, time elapsed: 3 minutes, 23.57 seconds evolution step 2800,
## fitness evaluations: 139950, best fitness: 15854.810400, time elapsed: 3
## minutes, 28.39 seconds evolution step 2900, fitness evaluations: 144950,
## best fitness: 15854.810400, time elapsed: 3 minutes, 33.76 seconds
## evolution step 3000, fitness evaluations: 149950, best fitness:
## 15854.810400, time elapsed: 3 minutes, 40.37 seconds Genetic programming
## evolution run FINISHED after 3000 evolution steps, 149950 fitness
## evaluations and 3 minutes, 40.37 seconds.
```


Лучший результат достигнут программой:

```r
print(gpResult1$elite[1])
```

```
## [[1]]
## function (F, M, I, Length, Diameter, Height, Whole, Shucked, 
##     Viscera, Shell) 
## 5.38137683334951 + (Shell + (Height + Whole - Shucked)) - 3.0046295553549/-0.686054509708283
```


Проверим программу на тестовом множестве:

```r
best1 <- gpResult1$population[[which.min(sapply(gpResult1$population, fitnessFunction1))]]
best1Compute = best1(val[, 1], val[, 2], val[, 3], val[, 4], val[, 5], val[, 
    6], val[, 7], val[, 8], val[, 9], val[, 10])
meanError = mean(abs(best1Compute - val[, 11]))
meanError
```

```
## [1] 1.732
```


Первые 10 результатов:

```r
best1Compute[1:10]
```

```
##  [1]  8.822  8.876  8.312  9.746  9.664  7.864  7.519 11.362 10.292 11.342
```

```r
val[1:10, 11]
```

```
##  [1] 10  9 10 10 12  9 11 11 12 18
```


Лучшая функция по результатам обучения с ранним остановом:

```r
print(bestFunction)
```

```
## function (F, M, I, Length, Diameter, Height, Whole, Shucked, 
##     Viscera, Shell) 
## 5.38137683334951 + (Shell + (Height + Whole - Shucked)) - 3.23449977110636/-0.81188623323263
```


Средняя ошибка:

```r
lastError
```

```
## [1] 1.693
```


Первые 10 результатов:

```r
best2Compute = bestFunction(val[, 1], val[, 2], val[, 3], val[, 4], val[, 5], 
    val[, 6], val[, 7], val[, 8], val[, 9], val[, 10])
best2Compute[1:10]
```

```
##  [1]  8.426  8.481  7.916  9.351  9.268  7.469  7.124 10.966  9.896 10.946
```

```r
val[1:10, 11]
```

```
##  [1] 10  9 10 10 12  9 11 11 12 18
```

