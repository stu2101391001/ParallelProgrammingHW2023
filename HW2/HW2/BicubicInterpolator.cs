using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW2
{
    internal class BicubicInterpolator
    {
        double scalingFactor = 0;
        string source = string.Empty;
        Bitmap oldImage;
        Bitmap newImage;
        int newImageWidth;
        int newImageHeight;
        //Ratios: used to calculate around where the new pixels lie in the original image.
        double ratioX = 0, ratioY = 0;
        //Shifts: used to sample from the centre, instead of the boundaries of the new pixels.
        double shiftX = 0, shiftY = 0;
        CancellationTokenSource cts = new CancellationTokenSource();
        public BicubicInterpolator() { }
        public BicubicInterpolator(string source, double scalingFactor)
        {
            Source = source;
            ScalingFactor = scalingFactor;
        }
        
        public string Source { get { return source; }
            set { if (value is not null) { source = value; oldImage = new Bitmap(Source); } }
        }
        public double ScalingFactor { get { return scalingFactor; } 
            set { if (value <= 0) throw new ArgumentException();
                else scalingFactor = value; }
        }

        public void InitializeNewImage()
        {
            newImageHeight = (int)Math.Round(scalingFactor*oldImage.Height);
            newImageWidth = (int)Math.Round(scalingFactor * oldImage.Width);
            ratioX = (oldImage.Width-1.0) / newImageWidth;
            ratioY = (oldImage.Height - 1.0) / newImageHeight;
            shiftX = ratioX / 2; 
            shiftY = ratioY / 2;
            newImage = new Bitmap(newImageWidth, newImageHeight);
        }
        public Bitmap beginInterpolationSequential()
        {
            beginInterpolation((0, newImageHeight, oldImage, cts.Token));
            return newImage;
        }
        public Bitmap beginInterpolationParallel(bool twice)
        {
            int numOfThreads = twice ? 2*Environment.ProcessorCount:Environment.ProcessorCount;
            Thread[] threads = new Thread[numOfThreads];
            int ptsPerThread = newImageHeight / numOfThreads;
            int lastBoundary = (numOfThreads-1)*ptsPerThread;
            threads[numOfThreads - 1] = new Thread(beginInterpolation);
            threads[numOfThreads - 1].Start((lastBoundary, newImageHeight, new Bitmap(oldImage), cts.Token));
            for (int i = 0; i < numOfThreads-1; i++)
            {
                threads[i] = new Thread(beginInterpolation);
                threads[i].Start((ptsPerThread*i, ptsPerThread*(i+1), new Bitmap(oldImage), cts.Token));
            }
            foreach (Thread thread in threads) thread.Join();
            return newImage;

        }
        private void beginInterpolation(object p)
        {
            (int start,int end, Bitmap, CancellationToken ct) boundary = ((int,int,Bitmap,CancellationToken))p;
            Bitmap image = boundary.Item3;
            for( int y = boundary.start; y < boundary.end; y++)
            {
                for(int x = 0; x < newImageWidth; x++)
                {
                    double xPos = findNewXPosition(x) - shiftX;
                    int xNewWhole = (int)Math.Floor(xPos);
                    double xNewFrac = xPos - xNewWhole;
                    double yPos = findNewYPosition(y) - shiftY;
                    int yNewWhole = (int)Math.Floor(yPos);
                    double yNewFrac = yPos - yNewWhole;
                    if (yNewWhole != 0 && yNewWhole != image.Height - 2)
                    {
                        if (boundary.ct.IsCancellationRequested) return;
                        (double, double, double, double)[] interpValues = new (double, double, double, double)[4];
                        if (xNewWhole != 0 && xNewWhole != image.Width - 2)
                        {
                            interpValues[0] = interpolateX4points(image.GetPixel(xNewWhole - 1, yNewWhole - 1),
                                image.GetPixel(xNewWhole, yNewWhole - 1), image.GetPixel(xNewWhole + 1, yNewWhole - 1),
                                image.GetPixel(xNewWhole + 2, yNewWhole - 1), xNewFrac);
                            interpValues[1] = interpolateX4points(image.GetPixel(xNewWhole - 1, yNewWhole),
                                image.GetPixel(xNewWhole, yNewWhole), image.GetPixel(xNewWhole + 1, yNewWhole),
                                image.GetPixel(xNewWhole + 2, yNewWhole), xNewFrac);
                            interpValues[2] = interpolateX4points(image.GetPixel(xNewWhole - 1, yNewWhole + 1),
                                image.GetPixel(xNewWhole, yNewWhole + 1), image.GetPixel(xNewWhole + 1, yNewWhole + 1),
                                image.GetPixel(xNewWhole + 2, yNewWhole + 1), xNewFrac);
                            interpValues[3] = interpolateX4points(image.GetPixel(xNewWhole - 1, yNewWhole + 2),
                                image.GetPixel(xNewWhole, yNewWhole + 2), image.GetPixel(xNewWhole + 1, yNewWhole + 2),
                                image.GetPixel(xNewWhole + 2, yNewWhole + 2), xNewFrac);

                        }
                        else if (xNewWhole == 0)
                        {
                            interpValues[0] = interpolateX3pointsl(image.GetPixel(xNewWhole, yNewWhole - 1), image.GetPixel(xNewWhole + 1, yNewWhole - 1),
                                image.GetPixel(xNewWhole + 2, yNewWhole - 1), xNewFrac);
                            interpValues[1] = interpolateX3pointsl(image.GetPixel(xNewWhole, yNewWhole), image.GetPixel(xNewWhole + 1, yNewWhole),
                                image.GetPixel(xNewWhole + 2, yNewWhole), xNewFrac);
                            interpValues[2] = interpolateX3pointsl(image.GetPixel(xNewWhole, yNewWhole + 1), image.GetPixel(xNewWhole + 1, yNewWhole + 1),
                                image.GetPixel(xNewWhole + 2, yNewWhole + 1), xNewFrac);
                            interpValues[3] = interpolateX3pointsl(image.GetPixel(xNewWhole, yNewWhole + 2), image.GetPixel(xNewWhole + 1, yNewWhole + 2),
                                image.GetPixel(xNewWhole + 2, yNewWhole + 2), xNewFrac);

                        }
                        else
                        {
                            interpValues[0] = interpolateX3pointsr(image.GetPixel(xNewWhole - 1, yNewWhole - 1),
                                image.GetPixel(xNewWhole, yNewWhole - 1), image.GetPixel(xNewWhole + 1, yNewWhole - 1), xNewFrac);
                            interpValues[1] = interpolateX3pointsr(image.GetPixel(xNewWhole - 1, yNewWhole),
                                image.GetPixel(xNewWhole, yNewWhole), image.GetPixel(xNewWhole + 1, yNewWhole), xNewFrac);
                            interpValues[2] = interpolateX3pointsr(image.GetPixel(xNewWhole - 1, yNewWhole + 1),
                                image.GetPixel(xNewWhole, yNewWhole + 1), image.GetPixel(xNewWhole + 1, yNewWhole + 1), xNewFrac);
                            interpValues[3] = interpolateX3pointsr(image.GetPixel(xNewWhole - 1, yNewWhole + 2),
                                image.GetPixel(xNewWhole, yNewWhole + 2), image.GetPixel(xNewWhole + 1, yNewWhole + 2), xNewFrac);
                        }
                        (double, double, double, double) color = interpolateY4points(interpValues, yNewFrac);
                        lock(newImage){
                            newImage.SetPixel(x, y, getColorFromDouble(color)); 
                        }
                    }
                    else if (yNewWhole == 0)
                    {
                        (double, double, double, double)[] interpValues = new (double, double, double, double)[3];
                        if (xNewWhole != 0 && xNewWhole != image.Width - 2)
                        {
                            interpValues[0] = interpolateX4points(image.GetPixel(xNewWhole - 1, yNewWhole),
                                image.GetPixel(xNewWhole, yNewWhole), image.GetPixel(xNewWhole + 1, yNewWhole),
                                image.GetPixel(xNewWhole + 2, yNewWhole), xNewFrac);
                            interpValues[1] = interpolateX4points(image.GetPixel(xNewWhole - 1, yNewWhole + 1),
                                image.GetPixel(xNewWhole, yNewWhole + 1), image.GetPixel(xNewWhole + 1, yNewWhole + 1),
                                image.GetPixel(xNewWhole + 2, yNewWhole + 1), xNewFrac);
                            interpValues[2] = interpolateX4points(image.GetPixel(xNewWhole - 1, yNewWhole + 2),
                                image.GetPixel(xNewWhole, yNewWhole + 2), image.GetPixel(xNewWhole + 1, yNewWhole + 2),
                                image.GetPixel(xNewWhole + 2, yNewWhole + 2), xNewFrac);

                        }
                        else if (xNewWhole == 0)
                        {
                            interpValues[0] = interpolateX3pointsl(image.GetPixel(xNewWhole, yNewWhole), image.GetPixel(xNewWhole + 1, yNewWhole),
                                image.GetPixel(xNewWhole + 2, yNewWhole), xNewFrac);
                            interpValues[1] = interpolateX3pointsl(image.GetPixel(xNewWhole, yNewWhole + 1), image.GetPixel(xNewWhole + 1, yNewWhole + 1),
                                image.GetPixel(xNewWhole + 2, yNewWhole + 1), xNewFrac);
                            interpValues[2] = interpolateX3pointsl(image.GetPixel(xNewWhole, yNewWhole + 2), image.GetPixel(xNewWhole + 1, yNewWhole + 2),
                                image.GetPixel(xNewWhole + 2, yNewWhole + 2), xNewFrac);

                        }
                        else
                        {
                            interpValues[0] = interpolateX3pointsr(image.GetPixel(xNewWhole - 1, yNewWhole),
                                image.GetPixel(xNewWhole, yNewWhole), image.GetPixel(xNewWhole + 1, yNewWhole), xNewFrac);
                            interpValues[1] = interpolateX3pointsr(image.GetPixel(xNewWhole - 1, yNewWhole + 1),
                                image.GetPixel(xNewWhole, yNewWhole + 1), image.GetPixel(xNewWhole + 1, yNewWhole + 1), xNewFrac);
                            interpValues[2] = interpolateX3pointsr(image.GetPixel(xNewWhole - 1, yNewWhole + 2),
                                image.GetPixel(xNewWhole, yNewWhole + 2), image.GetPixel(xNewWhole + 1, yNewWhole + 2), xNewFrac);
                        }
                        (double, double, double, double) color = interpolateY3pointsl(interpValues, yNewFrac);
                        lock(newImage){
                            newImage.SetPixel(x, y, getColorFromDouble(color));
                        }
                    }
                    else {
                        (double, double, double, double)[] interpValues = new (double, double, double, double)[3];
                        if (xNewWhole != 0 && xNewWhole != image.Width - 2)
                        {
                            interpValues[0] = interpolateX4points(image.GetPixel(xNewWhole - 1, yNewWhole - 1),
                                image.GetPixel(xNewWhole, yNewWhole - 1), image.GetPixel(xNewWhole + 1, yNewWhole - 1),
                                image.GetPixel(xNewWhole + 2, yNewWhole - 1), xNewFrac);
                            interpValues[1] = interpolateX4points(image.GetPixel(xNewWhole - 1, yNewWhole),
                                image.GetPixel(xNewWhole, yNewWhole), image.GetPixel(xNewWhole + 1, yNewWhole),
                                image.GetPixel(xNewWhole + 2, yNewWhole), xNewFrac);
                            interpValues[2] = interpolateX4points(image.GetPixel(xNewWhole - 1, yNewWhole + 1),
                                image.GetPixel(xNewWhole, yNewWhole + 1), image.GetPixel(xNewWhole + 1, yNewWhole + 1),
                                image.GetPixel(xNewWhole + 2, yNewWhole + 1), xNewFrac);

                        }
                        else if (xNewWhole == 0)
                        {
                            interpValues[0] = interpolateX3pointsl(image.GetPixel(xNewWhole, yNewWhole - 1), image.GetPixel(xNewWhole + 1, yNewWhole - 1),
                                image.GetPixel(xNewWhole + 2, yNewWhole - 1), xNewFrac);
                            interpValues[1] = interpolateX3pointsl(image.GetPixel(xNewWhole, yNewWhole), image.GetPixel(xNewWhole + 1, yNewWhole),
                                image.GetPixel(xNewWhole + 2, yNewWhole), xNewFrac);
                            interpValues[2] = interpolateX3pointsl(image.GetPixel(xNewWhole, yNewWhole + 1), image.GetPixel(xNewWhole + 1, yNewWhole + 1),
                                image.GetPixel(xNewWhole + 2, yNewWhole + 1), xNewFrac);

                        }
                        else
                        {
                            interpValues[0] = interpolateX3pointsr(image.GetPixel(xNewWhole - 1, yNewWhole - 1),
                                image.GetPixel(xNewWhole, yNewWhole - 1), image.GetPixel(xNewWhole + 1, yNewWhole - 1), xNewFrac);
                            interpValues[1] = interpolateX3pointsr(image.GetPixel(xNewWhole - 1, yNewWhole),
                                image.GetPixel(xNewWhole, yNewWhole), image.GetPixel(xNewWhole + 1, yNewWhole), xNewFrac);
                            interpValues[2] = interpolateX3pointsr(image.GetPixel(xNewWhole - 1, yNewWhole + 1),
                                image.GetPixel(xNewWhole, yNewWhole + 1), image.GetPixel(xNewWhole + 1, yNewWhole + 1), xNewFrac);
                        }
                        (double, double, double, double) color = interpolateY3pointsr(interpValues, yNewFrac);
                        lock (newImage)
                        {
                            newImage.SetPixel(x, y, getColorFromDouble(color));
                        }
                    }
                }
            }
        }
        public void Cancel()
        {
            cts.Cancel();
        }
        static private (double, double, double, double) interpolateX4points(Color p0, Color p1, Color p2, Color p3, double fraction)
        {
            double r1 = interpolate4points(p0.A, p1.A, p2.A, p3.A, fraction);
            double r2 = interpolate4points(p0.R, p1.R, p2.R, p3.R, fraction);
            double r3 = interpolate4points(p0.G, p1.G, p2.G, p3.G, fraction);
            double r4 = interpolate4points(p0.B, p1.B, p2.B, p3.B, fraction);
            return (r1, r2, r3, r4);
        }
        static private (double, double, double, double) interpolateX3pointsl(Color p1, Color p2, Color p3, double fraction)
        {
            double r1 = interpolate3pointsl(p1.A, p2.A, p3.A, fraction);
            double r2 = interpolate3pointsl(p1.R, p2.R, p3.R, fraction);
            double r3 = interpolate3pointsl(p1.G, p2.G, p3.G, fraction);
            double r4 = interpolate3pointsl(p1.B, p2.B, p3.B, fraction);
            return (r1, r2, r3, r4);
        }
        static private (double, double, double, double) interpolateX3pointsr(Color p0, Color p1, Color p2, double fraction)
        {
            double r1 = interpolate3pointsr(p0.A, p1.A, p2.A, fraction);
            double r2 = interpolate3pointsr(p0.R, p1.R, p2.R, fraction);
            double r3 = interpolate3pointsr(p0.G, p1.G, p2.G, fraction);
            double r4 = interpolate3pointsr(p0.B, p1.B, p2.B, fraction);
            return (r1, r2, r3, r4);
        }
        static private (double, double, double, double) interpolateY4points((double,double,double,double)[] points, double fraction)
        {
            double r1 = interpolate4points(points[0].Item1, points[1].Item1, points[2].Item1, points[3].Item1, fraction);
            double r2 = interpolate4points(points[0].Item2, points[1].Item2, points[2].Item2, points[3].Item2, fraction);
            double r3 = interpolate4points(points[0].Item3, points[1].Item3, points[2].Item3, points[3].Item3, fraction);
            double r4 = interpolate4points(points[0].Item4, points[1].Item4, points[2].Item4, points[3].Item4, fraction);
            return (r1, r2, r3, r4);
        }
        static private (double, double, double, double) interpolateY3pointsl((double, double, double, double)[] points, double fraction)
        {
            double r1 = interpolate3pointsl(points[0].Item1, points[1].Item1, points[2].Item1, fraction);
            double r2 = interpolate3pointsl(points[0].Item2, points[1].Item2, points[2].Item2, fraction);
            double r3 = interpolate3pointsl(points[0].Item3, points[1].Item3, points[2].Item3, fraction);
            double r4 = interpolate3pointsl(points[0].Item4, points[1].Item4, points[2].Item4, fraction);
            return (r1, r2, r3, r4);
        }
        static private (double, double, double, double) interpolateY3pointsr((double, double, double, double)[] points, double fraction)
        {
            double r1 = interpolate3pointsr(points[0].Item1, points[1].Item1, points[2].Item1, fraction);
            double r2 = interpolate3pointsr(points[0].Item2, points[1].Item2, points[2].Item2, fraction);
            double r3 = interpolate3pointsr(points[0].Item3, points[1].Item3, points[2].Item3, fraction);
            double r4 = interpolate3pointsr(points[0].Item4, points[1].Item4, points[2].Item4, fraction);
            return (r1, r2, r3, r4);
        }

        static private Color getColorFromDouble((double,double,double,double) color)
        {
            int alpha = color.Item1 > 255 ? 255 : color.Item1 < 0 ? 0 : (int)Math.Round(color.Item1);
            int red = color.Item2 > 255 ? 255 : color.Item2 < 0 ? 0 : (int)Math.Round(color.Item2);
            int green = color.Item3 > 255 ? 255 : color.Item3 < 0 ? 0 : (int)Math.Round(color.Item3);
            int blue = color.Item4 > 255 ? 255 : color.Item4 < 0 ? 0 : (int)Math.Round(color.Item4);
            return Color.FromArgb(alpha, red, green, blue);
        }

        static private double interpolate4points(double p0, double p1, double p2, double p3, double frac) {
            return 0.5 * (-p0 + 3 * p1 - 3 * p2 + p3) * frac * frac * frac + 0.5 * (2 * p1 - 5 * p1 + 4 * p2 - p3) * frac * frac + 0.5 * (-p0 + p2) * frac + p1;
        }
        static private double interpolate3pointsl(double p1, double p2, double p3, double frac)
        {
            return 0.5 * ( p1 - 2 * p2 + p3) * frac * frac + 0.5 * (-3*p1 + 4* p2 -p3) * frac + p1;
        }
        static private double interpolate3pointsr(double p0, double p1, double p2, double frac)
        {
            return 0.5 * (p0 - 2 * p1 + p2) * frac * frac + 0.5 * (-p0 + p2) * frac + p1;
        }
        private double findNewXPosition(int x)
        {
            return ratioX * (x+1);
        }
        private double findNewYPosition(int y)
        {
            return ratioY * (y+1);
        }
    }
}
