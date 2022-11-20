using System;
using System.Threading;
using System.Text;
namespace Donut_ASCII
{
    class Donut
    {
        static void Main(string[] args)
        {
            int res = 55; //Window rendering width and height in the console.
            int framerate = 100;
            char[] asciiTable = new char[9] {'.',':','-','=','+','*','#','%','@'}; //Black to total white grayscale. " .:-=+*#%@"
            double radius1 = 100; //Radius of circle which has midpoint of (radius2, 0)
            double radius2 = 150; //Radius from point of origin (0,0)
            //We use to this to secure that we never go over the resoloution, we want to see the entire donut!
            double distance = 5000; //Distance of donut from camera plane
            double distance2 = res * distance * (3 / (8 * (radius1 + radius2))); //Distance of viewer to camera plane
            double rotatingAngleX = 0.12*10/framerate; //Rotating speed over x axis
            double rotatingAngleZ = 0.08*10/framerate; //Rotating speed over z axis
            int x_offset = res / 2;
            int y_offset = res / 2;
            double currentAngleX = 1;
            double currentAngleZ = 1;
            while(true){  //For each frame, the animation goes forever therefore while true.
                //The currentAngles cosine and sin values used for rotating.
                double cosAngleZ = Math.Cos(currentAngleZ); 
                double sinAngleZ = Math.Sin(currentAngleZ);
                double cosAngleX = Math.Cos(currentAngleX); 
                double sinAngleX = Math.Sin(currentAngleX); 
                //CREATE A GRID AND BUFFER. 
                char[,] grid = new char[res, res]; //Default the grid.
                double[,] zBuffer = new double[res, res]; //We can render multiple points at the same x y positions due to the z axis,
                //therefore we keep track over the z axis positions and use these to determine the one which is closest to the camera
                for(int x = 0; x < res; x++) {
                    for(int y = 0; y < res; y++) {
                        grid[y, x] = ' ';
                        zBuffer[y, x] = 0;
                    }
                }
                for(double i = 0; i < 6.28; i+=0.07){  //628 IS THE SAME AS 2PI * 100. used to create a circle to start the torus. (A slice of a donut is a circle.)
                    double cosI = Math.Cos(i);
                    double sinI = Math.Sin(i);
                    double x1 = radius2 + radius1 * cosI; //x cordinate of 2d circle
                    double y1 = radius1 * sinI; //y cordinate of 2d circle
                    for(double j = 0; j < 6.28; j+=0.02){  //Expand the 2d circle into the 3d dimension by rotating the circle and adding these new points.
                        double cosJ = Math.Cos(j);
                        double sinJ = Math.Sin(j); //Here we calculate the new positions which also includes rotation.
                        //Using matrix multiplication we get the following as the new positiones.
                        //We also calculate the camera plane, in order to render it as 3D and not 2D. More info in documentation.
                        double x2 = x1 * (cosAngleZ * cosJ + sinAngleX * sinAngleZ * sinJ) - (y1 * cosAngleX * sinAngleZ);
                        double y2 = x1 * (cosJ * sinAngleZ - cosAngleZ * sinAngleX * sinJ) + (y1 * cosAngleX * cosAngleZ);
                        double z = distance + radius1 * sinAngleX * sinI + cosAngleX * sinJ * x1; //Camera plane Z
                        double inverseZ = 1 / z; //One over Z, aka inverse of Z (not as function, but ^-1)
                        //x and y projection on the screen Z plane.
                        int xPosition = (int)Math.Floor(x2 * distance2 * inverseZ);
                        int yPosition = (int)Math.Floor(y2 * distance2 * inverseZ);
                        //Calculate luminance based on dot product between normal vector of donut, and light vector.
                        double luminance = cosJ * cosI * sinAngleZ - cosAngleX * cosI * sinJ- sinAngleX * sinI + cosAngleZ * (cosAngleX * sinI - cosI * sinAngleX * sinJ);
                        if (luminance > -0.8) {
                            luminance = Math.Abs(luminance);
                            if (inverseZ > zBuffer[yPosition+y_offset,xPosition+x_offset]){  //Check if point is closer towards the camera plane than previous stored point.
                                zBuffer[yPosition+y_offset, xPosition+x_offset] = inverseZ;
                                int charIndex = (int)(Math.Round(luminance * ((asciiTable.Length-1)/1.414)));
                                grid[yPosition+y_offset, xPosition+x_offset] = asciiTable[charIndex];
                            }
                        }
                    }
                }
                currentAngleX =(currentAngleX+rotatingAngleX)%6.28;
                currentAngleZ = (currentAngleZ+rotatingAngleZ)%6.28;
                //GET ASCII => RENDER FRAME.
                StringBuilder frameOutput = new StringBuilder();
                for(int y = res-1; y >= 0; y--){
                    for(int x = 0; x < res; x++){
                        frameOutput.Append(grid[y, x]);
                    }
                    if (y != 0) { frameOutput.Append('\n'); }
                }
                Console.SetCursorPosition(0, 0);
                Console.Write(frameOutput); //Print the new frame.
                Thread.Sleep(1000/framerate); //Sleep so to show the frame before rendering next, we need some time to see!
            }
        }
    }
}