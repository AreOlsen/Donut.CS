using System;
using System.Threading;
using System.Text;
namespace Donut_ASCII
{
    class Donut
    {
        static void Main(string[] args)
        {
            int res=55;
            int fps=100;
            char[] aT=new char[9] {'.',':','-','=','+','*','#','%','@'};
            double r1=100; 
            double r2=150; 
            double d=5000;
            double d2=res*d*(3/(8*(r1+r2)));
            double rAX=0.12*10/fps;
            double rAZ=0.08*10/fps;
            int xO=res/2;
            int yO=res/2;
            double cAX=1;
            double cAZ=1;
            while (true){
                double cosZ=Math.Cos(cAZ); 
                double sinZ=Math.Sin(cAZ);
                double cosX=Math.Cos(cAX); 
                double sinX=Math.Sin(cAX); 
                char[,] grid=new char[res,res];
                double[,] zBuffer=new double[res,res];
                for(int x=0; x<res; x++) {
                    for(int y=0; y<res; y++) {
                        grid[y,x]=' ';
                        zBuffer[y,x]=0;
                    }
                }
                for(double i=0; i<6.28; i+=0.07){
                    double cosI=Math.Cos(i);
                    double sinI=Math.Sin(i);
                    double x1=r2+r1*cosI;
                    double y1=r1*sinI;
                    for(double j=0; j<6.28; j+=0.02){
                        double cosJ=Math.Cos(j);
                        double sinJ=Math.Sin(j);
                        double x2=x1*(cosZ*cosJ+sinX*sinZ*sinJ)-(y1*cosX*sinZ);
                        double y2=x1*(cosJ*sinZ-cosZ*sinX*sinJ)+(y1*cosX*cosZ);
                        double z=d+r1*sinX*sinI+cosX*sinJ*x1;
                        double iZ=1/z;
                        int xP=(int)Math.Floor(x2*d2*iZ);
                        int yP=(int)Math.Floor(y2*d2*iZ);
                        double l=cosJ*cosI*sinZ-cosX*cosI*sinJ-sinX*sinI+cosZ*(cosX*sinI-cosI*sinX*sinJ);
                        if (l>-0.8) {
                            l=Math.Abs(l);
                            if (iZ>zBuffer[yP+yO,xP+xO]){
                                zBuffer[yP+yO,xP+xO]=iZ;
                                int cI=(int)(Math.Floor(l*((aT.Length-1)/1.414)));
                                grid[yP+yO,xP+xO]=aT[cI];
                            }
                        }
                    }
                }
                cAX=(cAX+rAX)%6.28;
                cAZ=(cAZ+rAZ)%6.28;
                StringBuilder fO=new StringBuilder();
                for(int y=res-1; y >= 0; y--){
                    for(int x=0; x<res; x++){
                        fO.Append(grid[y,x]);
                    }
                    fO.Append('\n'); 
                }
                Console.SetCursorPosition(0,0);
                Console.Write(fO);
                Thread.Sleep(1000/fps);
            }
        }
    }
}