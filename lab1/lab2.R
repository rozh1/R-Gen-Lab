library("png")
library("rgenoud")

logo = readPNG("homo.png")

imageSize=dim(logo)

nrows = imageSize[1]
ncols = imageSize[2]

Num = 4*100

imageNum=0

DrawImage <- function(xx, filename="")
{
  outImageMatrix=matrix(1,nrow=nrows,ncol=ncols)
  len = length(xx)/4
  summ=0
  for (i in 1:len)
  {
    x1=xx[(i-1)*4+1]
    y1=xx[(i-1)*4+2]
    x2=xx[(i-1)*4+3]
    y2=xx[(i-1)*4+4]
    outImageMatrix[y1:y2,x1:x2] = 0 
  }
  if (filename=="")
    #writePNG(outImageMatrix, sprintf("result/%06d.png",imageNum))
  else
    writePNG(outImageMatrix, sprintf(filename,imageNum))
  imageNum <<- imageNum+1
}

f <- function(xx)
{
  len = length(xx)/4
  imageMatrix=matrix(1,nrow=nrows,ncol=ncols)
  #summ=0
  for (i in 1:len)
  {
    x1=xx[(i-1)*4+1]
    y1=xx[(i-1)*4+2]
    x2=xx[(i-1)*4+3]
    y2=xx[(i-1)*4+4]
    imageMatrix[y1:y2,x1:x2] = 0 
    #summ=summ+sum(logo[y1:y2,x1:x2])
  }
  sub = imageMatrix-logo
  sub[sub<0]<-0
  summ = sum(sub)
  
  sub = logo-imageMatrix
  sub[sub<0]<-0
  summ = summ + sum(sub)
  DrawImage(xx)
  return (summ)
}

dom = c(1,ncols,1,nrows,1,ncols,1,nrows)
m = matrix(dom,nrow=Num,ncol=2,byrow=TRUE)

#res = genoud(fn=f,max.generations=100,nvars=Num,Domains=m,pop.size=100,data.type.int=TRUE,boundary.enforcement=2)
res = genoud(fn=f,max.generations=100,nvars=Num,Domains=m,pop.size=100,data.type.int=TRUE,boundary.enforcement=2,P1=50,P2=10,P3=100,P4=50,P5=20,P6=20,P7=30,P8=20,P9=0)

rectangles = res$par

DrawImage(rectangles,filename="result/out1")

res = genoud(fn=f,max.generations=100,nvars=Num,Domains=m,pop.size=100,data.type.int=TRUE,boundary.enforcement=2,P1=100,P2=0,P3=0,P4=0,P5=0,P6=0,P7=0,P8=0,P9=0)

rectangles = res$par

DrawImage(rectangles,filename="result/out2")

res = genoud(fn=f,max.generations=100,nvars=Num,Domains=m,pop.size=100,data.type.int=TRUE,boundary.enforcement=2,P1=50,P2=50,P3=0,P4=0,P5=0,P6=0,P7=0,P8=0,P9=0)

rectangles = res$par

DrawImage(rectangles,filename="result/out3")

res = genoud(fn=f,max.generations=100,nvars=Num,Domains=m,pop.size=100,data.type.int=TRUE,boundary.enforcement=2,P1=50,P2=50,P3=50,P4=0,P5=0,P6=0,P7=0,P8=0,P9=0)

rectangles = res$par

DrawImage(rectangles,filename="result/out4")

res = genoud(fn=f,max.generations=100,nvars=Num,Domains=m,pop.size=100,data.type.int=TRUE,boundary.enforcement=2,P1=50,P2=10,P3=50,P4=50,P5=0,P6=0,P7=0,P8=0,P9=0)

rectangles = res$par

DrawImage(rectangles,filename="result/out5")

res = genoud(fn=f,max.generations=100,nvars=Num,Domains=m,pop.size=100,data.type.int=TRUE,boundary.enforcement=2,P1=50,P2=10,P3=50,P4=50,P5=0,P6=0,P7=0,P8=0,P9=0)

rectangles = res$par

DrawImage(rectangles,filename="result/out6")

res = genoud(fn=f,max.generations=100,nvars=Num,Domains=m,pop.size=100,data.type.int=TRUE,boundary.enforcement=2,P1=50,P2=10,P3=50,P4=50,P5=50,P6=0,P7=0,P8=0,P9=0)

rectangles = res$par

DrawImage(rectangles,filename="result/out7")

res = genoud(fn=f,max.generations=100,nvars=Num,Domains=m,pop.size=100,data.type.int=TRUE,boundary.enforcement=2,P1=50,P2=10,P3=50,P4=50,P5=50,P6=50,P7=0,P8=0,P9=0)

rectangles = res$par

DrawImage(rectangles,filename="result/out8")

res = genoud(fn=f,max.generations=100,nvars=Num,Domains=m,pop.size=100,data.type.int=TRUE,boundary.enforcement=2,P1=50,P2=10,P3=50,P4=50,P5=50,P6=50,P7=50,P8=0,P9=0)

rectangles = res$par

DrawImage(rectangles,filename="result/out9")

res = genoud(fn=f,max.generations=100,nvars=Num,Domains=m,pop.size=100,data.type.int=TRUE,boundary.enforcement=2,P1=50,P2=10,P3=50,P4=50,P5=50,P6=50,P7=50,P8=50,P9=0)

rectangles = res$par

DrawImage(rectangles,filename="result/out10")

res = genoud(fn=f,max.generations=100,nvars=Num,Domains=m,pop.size=100,data.type.int=TRUE,boundary.enforcement=2,P1=50,P2=10,P3=50,P4=50,P5=50,P6=50,P7=50,P8=50,P9=50)

rectangles = res$par

DrawImage(rectangles,filename="result/out11")


res = genoud(fn=f,max.generations=100,nvars=Num,Domains=m,pop.size=10,data.type.int=TRUE,boundary.enforcement=2,P1=50,P2=0,P3=0,P4=50,P5=50,P6=50,P7=50,P8=50,P9=0,gradient.check=FALSE, BFGS=FALSE)

rectangles = res$par

DrawImage(rectangles,filename="out12.png")

#outImageMatrix=matrix(1,nrow=nrows,ncol=ncols)
#
#len = length(rectangles)/4
#i=0
#for (i in 1:len)
#{
#  x1=rectangles[(i-1)*4+1]
#  y1=rectangles[(i-1)*4+2]
#  x2=rectangles[(i-1)*4+3]
#  y2=rectangles[(i-1)*4+4]
#  outImageMatrix[y1:y2,x1:x2] = 0 
#}

#writePNG(outImageMatrix,"out.png")